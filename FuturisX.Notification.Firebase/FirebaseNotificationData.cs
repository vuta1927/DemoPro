﻿using System;
using System.Collections.Generic;
using System.Linq;
using FuturisX.Helpers.Extensions;
using FuturisX.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FuturisX.Notification.Firebase
{
    [Serializable]
    public class FirebaseNotificationData : NotificationData
    {
        public static FirebaseNotificationData FromJsonString(string firebaseNotificationData)
        {
            return JsonConvert.DeserializeObject<FirebaseNotificationData>(firebaseNotificationData);
        }

        public static FirebaseNotificationData ForSingleResult(FirebaseResponse response, int resultIndex)
        {
            var result = new FirebaseNotificationData
            {
                Tag = response.OriginalNotification.Tag,
                MessageId = response.OriginalNotification.MessageId
            };

            if (response.OriginalNotification.RegistrationIds != null &&
                response.OriginalNotification.RegistrationIds.Count >= resultIndex + 1)
            {
                result.RegistrationIds.Add(response.OriginalNotification.RegistrationIds[resultIndex]);
            }

            result.CollapseKey = response.OriginalNotification.CollapseKey;
            result.Data = response.OriginalNotification.Data;
            result.DelayWhileIdle = response.OriginalNotification.DelayWhileIdle;
            result.ContentAvailable = response.OriginalNotification.ContentAvailable;
            result.DryRun = response.OriginalNotification.DryRun;
            result.Priority = response.OriginalNotification.Priority;
            result.To = response.OriginalNotification.To;

            return result;
        }

        public FirebaseNotificationData()
        {
            RegistrationIds = new List<string>();
            CollapseKey = string.Empty;
            Data = null;
            DelayWhileIdle = null;
        }

        [JsonIgnore]
        public object Tag { get; set; }

        [JsonProperty("message_id")]
        public string MessageId {
            get
            {
                if (Properties.ContainsKey("MessageId"))
                    return Properties["MessageId"].ToString();
                return null;
            }
            internal set => Properties["MessageId"] = value;
        }

        /// <summary>
        /// Registration ID of the Device(s).  Maximum of 1000 registration Id's per notification.
        /// </summary>
        [JsonProperty("registration_ids")]
        public List<string> RegistrationIds
        {
            get => Properties["RegistrationIds"].ToString().Split(',').ToList();
            set => Properties["RegistrationIds"] = string.Join(",", value);
        }

        /// <summary>
        /// Registration ID or Group/Topic to send notification to.  Overrides RegsitrationIds.
        /// </summary>
        /// <value>To.</value>
        [JsonProperty("to")]
        public string To { get; set; }

        /// <summary>
        /// This parameter specifies a logical expression of conditions that determine the message target.
        /// 
        /// Supported condition: Topic, formatted as "'yourTopic' in topics". This value is case-insensitive.
        /// 
        /// Supported operators: &&, ||. Maximum two operators per topic message supported.
        /// </summary>
        [JsonProperty(PropertyName = "condition")]
        public string Condition
        {
            get
            {
                if (Properties.ContainsKey("Condition"))
                    return Properties["Condition"].ToString();
                return null;
            }
            set => Properties["Condition"] = value;
        }

        /// <summary>
        /// Only the latest message with the same collapse key will be delivered
        /// </summary>
        [JsonProperty("collapse_key")]
        public string CollapseKey
        {
            get
            {
                if (Properties.ContainsKey("CollapseKey"))
                    return Properties["CollapseKey"].ToString();
                return null;
            }
            set => Properties["CollapseKey"] = value;
        }

        /// <summary>
        /// Indicates a sound to play when the device receives a notification. Sound files can be in the main bundle of the client app or in the Library/Sounds folder of the app's data container.
        /// See the iOS Developer Library for more information.
        /// </summary>
        [JsonProperty(PropertyName = "sound")]
        public string Sound
        {
            get
            {
                if (Properties.ContainsKey("Sound"))
                    return Properties["Sound"].ToString();
                return null;
            }
            set => Properties["Sound"] = value;
        }

        /// <summary>
        /// JSON Payload to be sent in the message
        /// </summary>
        [JsonProperty("data")]
        public JObject Data
        {
            get
            {
                if (Properties.ContainsKey("Data"))
                    return Properties["Data"] as JObject;
                return null;
            }
            set => Properties["Data"] = value;
        }

        /// <summary>
        /// Notification JSON payload
        /// </summary>
        /// <value>The notification payload.</value>
        [JsonProperty("notification")]
        public FirebaseNotificationPayload Notification
        {
            get
            {
                if (Properties.ContainsKey("Notification"))
                    return JsonConvert.DeserializeObject<FirebaseNotificationPayload>(
                        Properties["Notification"].ToString());
                return null;
            }
            set => Properties["Notification"] = value.ToString();
        }

        /// <summary>
        /// If true, GCM will only be delivered once the device's screen is on
        /// </summary>
        [JsonProperty("delay_while_idle")]
        public bool? DelayWhileIdle
        {
            get
            {
                if (Properties.ContainsKey("DelayWhileIdle"))
                    return Properties["DelayWhileIdle"] as bool?;
                return null;
            }
            set => Properties["DelayWhileIdle"] = value;
        }

        /// <summary>
        /// Time in seconds that a message should be kept on the server if the device is offline.  Default (and maximum) is 4 weeks.
        /// </summary>
        [JsonProperty("time_to_live")]
        public int? TimeToLive
        {
            get
            {
                if (Properties.ContainsKey("TimeToLive"))
                    return Properties["TimeToLive"] as int?;
                return null;
            }
            set => Properties["TimeToLive"] = value;
        }

        /// <summary>
        /// If <c>true</c>, dry_run attribute will be sent in payload causing the notification not to be actually sent, but the result returned simulating the message
        /// </summary>
        [JsonProperty("dry_run")]
        public bool? DryRun
        {
            get
            {
                if (Properties.ContainsKey("DryRun"))
                    return Properties["DryRun"] as bool?;
                return null;
            }
            set => Properties["DryRun"] = value;
        }

        /// <summary>
        /// On iOS, use this field to represent content-available in the APNS payload.
        /// When a notification or message is sent and this is set to <c>true</c>, an inactive client app is awoken.
        /// On Android, data messages wake the app by default.
        /// On Chrome, currently not supported.
        /// </summary>
        /// <value>The content available.</value>
        [JsonProperty("content_available")]
        public bool? ContentAvailable
        {
            get
            {
                if (Properties.ContainsKey("ContentAvailable"))
                    return Properties["ContentAvailable"] as bool?;
                return null;
            }
            set => Properties["ContentAvailable"] = value;
        }

        /// <summary>
        /// Corresponds to iOS APNS priorities (Normal is 5 and high is 10).  Default is Normal.
        /// </summary>
        /// <value>The priority.</value>
        [JsonProperty("priority"), JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public FirebaseNotificationPriority? Priority
        {
            get
            {
                if (Properties.ContainsKey("Priority"))
                    return Properties["Priority"].To<FirebaseNotificationPriority>();
                return null;
            }
            set => Properties["Priority"] = value;
        }
        
        internal string GetJson()
        {
            // If 'To' was used instead of RegistrationIds, let's make RegistrationId's null
            // so we don't serialize an empty array for this property
            // otherwise, google will complain that we specified both instead
            if (RegistrationIds != null && RegistrationIds.Count <= 0 && !string.IsNullOrEmpty(To))
                RegistrationIds = null;

            // Ignore null value
            return JsonConvert.SerializeObject(this,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
        }

        public override string ToString()
        {
            return GetJson();
        }
    }
}