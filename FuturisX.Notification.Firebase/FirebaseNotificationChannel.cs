using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using FuturisX.Helpers.Extensions;
using FuturisX.Notifications;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace FuturisX.Notification.Firebase
{
    public class FirebaseNotificationChannel : INotificationChannel
    {
        private readonly FirebaseNotificationConfiguration _configuration;
        private readonly IFirebaseService _firebaseService;
        private readonly INotificationStore _notificationStore;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ILogger _logger;

        public FirebaseNotificationChannel(
            FirebaseNotificationConfiguration configuration,
            IFirebaseService firebaseService,
            INotificationStore notificationStore,
            ILogger<FirebaseNotificationChannel> logger)
        {
            _configuration = configuration;
            _firebaseService = firebaseService;
            _notificationStore = notificationStore;
            _logger = logger;

            _httpClient.DefaultRequestHeaders.UserAgent.Clear();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key={0}".FormatWith(configuration.ServerKey));
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Sender", "id={0}".FormatWith(configuration.SenderId));
        }

        public bool CanHandle(Type notificationDataType)
        {
            return notificationDataType == typeof(FirebaseNotificationData);
        }

        public async Task ProcessAsync(UserNotificationInfo userNotificationsInfo)
        {
            if (userNotificationsInfo == null)
            {
                return;
            }
            var notification = await _notificationStore.GetNotificationOrNullAsync(userNotificationsInfo.NotificationId);
            if (notification == null)
            {
                return;
            }

            var firebaseNotificationData = FirebaseNotificationData.FromJsonString(notification.Data);
            firebaseNotificationData.Notification.Badge =
                (await _notificationStore.GetUserNotificationCountAsync(userNotificationsInfo.UserId,
                    UserNotificationState.Unread)).ToString();

            var firebaseRegistrations = await _firebaseService.GetAllRegistrationAsync(userNotificationsInfo.UserId);
            if (firebaseRegistrations.IsNullOrEmpty())
                return;

            firebaseNotificationData.RegistrationIds.AddRange(firebaseRegistrations.Select(x => x.RegistrationId));

            var json = firebaseNotificationData.ToString();
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_configuration.Url, content);

            if (response.IsSuccessStatusCode)
            {
                await ProcessResponseOk(response, firebaseNotificationData);
            }
            else
            {
                await ProcessReponseError(response, firebaseNotificationData);
            }
        }

        private async Task ProcessResponseOk(HttpResponseMessage httpResponse, FirebaseNotificationData notification)
        {

            var result = new FirebaseResponse
            {
                ResponseCode = FirebaseResponseCode.Ok,
                OriginalNotification = notification
            };

            var str = await httpResponse.Content.ReadAsStringAsync();
            var json = JObject.Parse(str);

            result.NumberOfCanonicalIds = json.Value<long>("canonical_ids");
            result.NumberOfFailures = json.Value<long>("failure");
            result.NumberOfSuccesses = json.Value<long>("success");

            var jsonResults = json["result"] as JArray ?? new JArray();

            foreach (var jsonResult in jsonResults)
            {
                var msgResult = new FirebaseMessageResult
                {
                    MessageId = jsonResult.Value<string>("message_id"),
                    CanonicalRegistrationId = jsonResult.Value<string>("registration_id"),
                    ResponseStatus = FirebaseResponseStatus.Ok
                };


                if (!string.IsNullOrEmpty(msgResult.CanonicalRegistrationId))
                    msgResult.ResponseStatus = FirebaseResponseStatus.CanonicalRegistrationId;
                else if (jsonResult["error"] != null)
                {
                    var err = jsonResult.Value<string>("error") ?? "";

                    msgResult.ResponseStatus = GetFirebaseReponseStatus(err);
                }

                result.Results.Add(msgResult);
            }

            var index = 0;
            var multicastResult = new FirebaseMulticastResultException();

            //Loop through every result in the response
            // We will raise events for each individual result so that the consumer of the library
            // can deal with individual registrationid's for the notification
            foreach (var r in result.Results)
            {
                var singleResultNotification = FirebaseNotificationData.ForSingleResult(result, index);

                singleResultNotification.MessageId = r.MessageId;

                if (r.ResponseStatus == FirebaseResponseStatus.Ok)
                { // Success
                    multicastResult.Succeeded.Add(singleResultNotification);
                }
                else if (r.ResponseStatus == FirebaseResponseStatus.CanonicalRegistrationId)
                { //Need to swap reg id's
                    //Swap Registrations Id's
                    var newRegistrationId = r.CanonicalRegistrationId;
                    var oldRegistrationId = string.Empty;

                    if (singleResultNotification.RegistrationIds != null && singleResultNotification.RegistrationIds.Count > 0)
                    {
                        oldRegistrationId = singleResultNotification.RegistrationIds[0];
                    }
                    else if (!string.IsNullOrEmpty(singleResultNotification.To))
                    {
                        oldRegistrationId = singleResultNotification.To;
                    }

                    multicastResult.Failed.Add(singleResultNotification,
                        new DeviceSubscriptionExpiredException(singleResultNotification)
                        {
                            OldSubscriptionId = oldRegistrationId,
                            NewSubscriptionId = newRegistrationId
                        });
                }
                else if (r.ResponseStatus == FirebaseResponseStatus.Unavailable)
                { // Unavailable
                    multicastResult.Failed.Add(singleResultNotification, new FirebaseNotificationException(singleResultNotification, "Unavailable Response Status"));
                }
                else if (r.ResponseStatus == FirebaseResponseStatus.NotRegistered)
                { //Bad registration Id
                    var oldRegistrationId = string.Empty;

                    if (singleResultNotification.RegistrationIds != null && singleResultNotification.RegistrationIds.Count > 0)
                    {
                        oldRegistrationId = singleResultNotification.RegistrationIds[0];
                    }
                    else if (!string.IsNullOrEmpty(singleResultNotification.To))
                    {
                        oldRegistrationId = singleResultNotification.To;
                    }

                    multicastResult.Failed.Add(singleResultNotification,
                                                new DeviceSubscriptionExpiredException(singleResultNotification)
                                                {
                                                    OldSubscriptionId = oldRegistrationId
                                                });
                }
                else
                {
                    multicastResult.Failed.Add(singleResultNotification, new FirebaseNotificationException(singleResultNotification, "Unknown Failure: " + r.ResponseStatus));
                }

                index++;
            }

            // If we only have 1 total result, it is not *multicast*, 
            if (multicastResult.Succeeded.Count + multicastResult.Failed.Count == 1)
            {
                // If not multicast, and succeeded, don't throw any errors!
                if (multicastResult.Succeeded.Count == 1)
                    return;

                _logger.LogWarning("Fail to send notification through Fireabse", multicastResult.Failed.First().Value);
                // Otherwise, throw the one single failure we must have
                throw multicastResult.Failed.First().Value;
            }

            // If we get here, we must have had a multicast message
            // throw if we had any failures at all (otherwise all must be successful, so throw no error
            if (multicastResult.Failed.Count > 0)
            {
                _logger.LogWarning(multicastResult, "Fail to send notification through Firebase");
                throw multicastResult;
            }
        }

        private async Task ProcessReponseError(HttpResponseMessage httpResponse, FirebaseNotificationData notification)
        {
            string responseBody = null;

            try
            {
                responseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                // ignored
            }

            //401 bad auth token
            if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning(new UnauthorizedAccessException("Firebase Authorization Failed"), string.Empty);
                throw new UnauthorizedAccessException("Firebase Authorization Failed");
            }

            if (httpResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                _logger.LogWarning(new FirebaseNotificationException(notification, "HTTP 400 Bad Request", responseBody), string.Empty);
                throw new FirebaseNotificationException(notification, "HTTP 400 Bad Request", responseBody);
            }

            if ((int)httpResponse.StatusCode >= 500 && (int)httpResponse.StatusCode < 600)
            {
                //First try grabbing the retry-after header and parsing it.
                var retryAfterHeader = httpResponse.Headers.RetryAfter;

                if (retryAfterHeader?.Delta != null)
                {
                    var retryAfter = retryAfterHeader.Delta.Value;
                    _logger.LogWarning(new RetryAfterException(notification, "Firebase Requested Backoff", DateTime.UtcNow + retryAfter), string.Empty);
                    throw new RetryAfterException(notification, "Firebase Requested Backoff", DateTime.UtcNow + retryAfter);
                }
            }

            _logger.LogWarning(new FirebaseNotificationException(notification, "Firebase HTTP Error: " + httpResponse.StatusCode, responseBody), string.Empty);
            throw new FirebaseNotificationException(notification, "Firebase HTTP Error: " + httpResponse.StatusCode, responseBody);
        }

        private FirebaseResponseStatus GetFirebaseReponseStatus(string str)
        {
            var enumType = typeof(FirebaseResponseStatus);

            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();

                if (enumMemberAttribute.Value.Equals(str, StringComparison.InvariantCultureIgnoreCase))
                    return (FirebaseResponseStatus)Enum.Parse(enumType, name);
            }

            //Default
            return FirebaseResponseStatus.Error;
        }
    }
}