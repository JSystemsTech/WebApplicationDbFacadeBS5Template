using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationDbFacadeBS5Template.Extensions.Html;

namespace WebApplicationDbFacadeBS5Template.Models
{
    public class NavTabVM
    {
        private string BaseId { get; set; }
        public string TabId => $"{BaseId}_Tab";
        public string ContainerId => $"{BaseId}_Container";
        public string Name { get; set; }
        public string Icon { get; set; }
        public string ContentUrl { get; set; }
        public bool IsActive { get; set; }

        public NavTabVM(string name, string contentUrl, bool isActive = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name", "NavTabVM property 'Name' must not be null or whitespace.");
            }
            if (string.IsNullOrWhiteSpace(contentUrl))
            {
                throw new ArgumentNullException("contentUrl", "NavTabVM property 'ContentUrl' must not be null or whitespace.");
            }
            BaseId = HtmlHelperExtensions.UniqueId();
            Name = name;
            ContentUrl = contentUrl;
            IsActive = isActive;
        }
    }
}