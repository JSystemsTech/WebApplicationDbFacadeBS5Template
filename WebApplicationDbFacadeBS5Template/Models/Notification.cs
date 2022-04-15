using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationDbFacadeBS5Template.Models
{
    public class Notification
    {
        [JsonProperty("title")]
        public string Title { get; internal set; }

        [JsonProperty("type")]
        public string Type { get; internal set; }

        [JsonProperty("message")]
        public string Message { get; internal set; }

        [JsonProperty("delay")]
        public int? Delay { get; internal set; }

        [JsonProperty("autohide")]
        public bool Autohide { get; internal set; }

        [JsonProperty("html")]
        public bool Html { get; internal set; }

        [JsonProperty("icon")]
        public string Icon { get; internal set; }

        public Notification (string title, string message, string type)
        {
            Title = title;
            Type = type;
            Message = message;
            Delay = 6000;
            Autohide = true;
            Icon = "fa-info-circle";
        }
        public static Notification Primary(string title, string message) => new Notification(title, message, "primary") { Icon = "fa-info-circle"};
        public static Notification Secondary(string title, string message) => new Notification(title, message, "secondary") { Icon = "fa-info-circle" };
        public static Notification Info(string title, string message) => new Notification(title, message, "info") { Icon = "fa-info-circle" };
        public static Notification Success(string title, string message) => new Notification(title, message, "success") { Icon = "fa-check-circle" };
        public static Notification Warning(string title, string message) => new Notification(title, message, "warning") { Icon = "fa-exclamation-circle" };
        public static Notification Danger(string title, string message) => new Notification(title, message, "danger") { Icon = "fa-exclamation-triangle" };
        public static Notification Error(string title, Exception ex) => new Notification(title, ex.Message, "danger") { Icon = "fa-exclamation-triangle" };
    }
}