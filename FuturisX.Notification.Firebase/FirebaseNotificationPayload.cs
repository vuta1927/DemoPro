using FuturisX.Json;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace FuturisX.Notification.Firebase
{
    public class FirebaseNotificationPayload
    {
        /// <summary>
        /// The notification's title.
        /// This field is not visible on iOS phones and tablets.
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// The notification's body text.
        /// </summary>
        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        /// <summary>
        /// The sound to play when the device receives the notification.
        /// </summary>
        [JsonProperty(PropertyName = "sound")]
        public string Sound { get; set; }

        /// <summary>
        /// Indicates the badge on the client app home icon.
        /// 
        /// If not specified, the badge is not changed.
        /// 
        /// If set to 0, the badge is removed.
        /// </summary>
        [JsonProperty(PropertyName = "badge")]
        public string Badge { get; set; }

        /// <summary>
        /// The action associated with a user click on the notification.
        /// </summary>
        [JsonProperty(PropertyName = "click_action")]
        public string ClickAction { get; set; }

        /// <summary>
        /// The notification's subtitle.
        /// </summary>
        [CanBeNull]
        [JsonProperty(PropertyName = "subtitle")]
        public string Subtitle { get; set; }

        /// <summary>
        /// The notification's icon.
        /// </summary>
        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        /// <summary>
        /// The notification's icon color, expressed in #rrggbb format.
        /// </summary>
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}