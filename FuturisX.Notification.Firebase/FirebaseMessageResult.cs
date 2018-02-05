using Newtonsoft.Json;

namespace FuturisX.Notification.Firebase
{
    public class FirebaseMessageResult
    {
        [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
        public string MessageId { get; set; }

        [JsonProperty("registration_id", NullValueHandling = NullValueHandling.Ignore)]
        public string CanonicalRegistrationId { get; set; }

        [JsonIgnore]
        public FirebaseResponseStatus ResponseStatus { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error
        {
            get
            {
                switch (ResponseStatus)
                {
                    case FirebaseResponseStatus.Ok:
                        return null;
                    case FirebaseResponseStatus.Unavailable:
                        return "Unavailable";
                    case FirebaseResponseStatus.QuotaExceeded:
                        return "QuotaExceeded";
                    case FirebaseResponseStatus.NotRegistered:
                        return "NotRegistered";
                    case FirebaseResponseStatus.MissingRegistrationId:
                        return "MissingRegistration";
                    case FirebaseResponseStatus.MissingCollapseKey:
                        return "MissingCollapseKey";
                    case FirebaseResponseStatus.MismatchSenderId:
                        return "MismatchSenderId";
                    case FirebaseResponseStatus.MessageTooBig:
                        return "MessageTooBig";
                    case FirebaseResponseStatus.InvalidTtl:
                        return "InvalidTtl";
                    case FirebaseResponseStatus.InvalidRegistration:
                        return "InvalidRegistration";
                    case FirebaseResponseStatus.InvalidDataKey:
                        return "InvalidDataKey";
                    case FirebaseResponseStatus.InternalServerError:
                        return "InternalServerError";
                    case FirebaseResponseStatus.DeviceQuotaExceeded:
                        return null;
                    case FirebaseResponseStatus.CanonicalRegistrationId:
                        return null;
                    case FirebaseResponseStatus.Error:
                        return "Error";
                    default:
                        return null;
                }
            }
        }
    }
}