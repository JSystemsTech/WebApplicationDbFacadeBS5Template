using DbFacade.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data
{
    public class AppAnnouncement: DbDataModel
    {
        public Guid Guid { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public string PriorityCode { get; private set; }
        public DateTime ActiveDate { get; private set; }
        public DateTime? ExpireDate { get; private set; }
        public Guid? ImageFileGuid { get; private set; }
        protected override void Init()
        {
            Guid = GetColumn<Guid>("Guid");
            Title = GetColumn<string>("Title");
            Message = GetColumn<string>("Message");
            PriorityCode = GetColumn<string>("PriorityCode");
            ActiveDate = GetColumn<DateTime>("ActiveDate");
            ExpireDate = GetColumn<DateTime?>("ExpireDate");
            ImageFileGuid = GetColumn<Guid?>("ImageFileGuid");
        }
    }
}