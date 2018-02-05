using Newtonsoft.Json;

namespace FuturisX.Notification.Firebase
{
    public interface IFirebaseNotificationData
    {
        [JsonIgnore]
        object Tag { get; set; }

        [JsonProperty("message_id")]
        string MessageId { get; }
    }
}