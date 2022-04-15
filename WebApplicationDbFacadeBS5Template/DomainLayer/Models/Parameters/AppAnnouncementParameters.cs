using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationDbFacadeBS5Template.DomainLayer.Models.Parameters
{
    public class AppAnnouncementAddParameters
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string PriorityCode { get; set; }
        public DateTime ActiveDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public Guid? ImageFileGuid { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
    public class AppAnnouncementUpdateParameters
    {
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string PriorityCode { get; set; }
        public DateTime? ActiveDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public Guid? ImageFileGuid { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}