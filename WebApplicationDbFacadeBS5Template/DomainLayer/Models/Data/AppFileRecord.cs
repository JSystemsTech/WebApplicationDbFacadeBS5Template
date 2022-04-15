using DbFacade.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data
{
    public class AppFileRecord: DbDataModel
    {
        public Guid Guid { get; private set; }
        public string FileName { get; private set; }
        public string MIMEType { get; private set; }
        public byte[] Data { get; private set; }
        public DateTime CreateDate { get; private set; }
        protected override void Init()
        {
            Guid = GetColumn<Guid>("Guid");
            FileName = GetColumn<string>("FileName");
            MIMEType = GetColumn<string>("MIMEType");
            Data = GetColumn<byte[]>("Data");
            CreateDate = GetColumn<DateTime>("CreateDate");
        }
    }
}