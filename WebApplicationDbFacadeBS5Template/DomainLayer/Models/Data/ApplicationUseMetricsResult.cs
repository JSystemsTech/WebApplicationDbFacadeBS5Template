using DbFacade.DataLayer.Models;
using System;
using WebApplicationDbFacadeBS5Template.Services.Configuration;

namespace WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data
{
    public class ApplicationUseMetricsResult:DbDataModel
    {
        public int Value { get; private set; }
        public string Label { get; private set; }
        public int Count { get; private set; }

        protected override void Init()
        {
            Value = GetColumn<int>("Value");
            Label = GetColumn<string>("Label");
            Count = GetColumn<int>("Count");
        }
    }
    public class ApplicationUseByDayMetricsResult : DbDataModel
    {
        public string Label { get; private set; }
        public DateTime Date { get; private set; }
        public double Average { get; private set; }
        public double Min { get; private set; }
        public double Max { get; private set; }
        public double Total { get; private set; }
        public int Users { get; private set; }
        public int Logins { get; private set; }

        protected override void Init()
        {
            Date = GetColumn<DateTime>("Date");
            Label = GetFormattedDateTimeStringColumn("Date", DateTimeFormatConfiguration.Date);
            Average = GetColumn<double>("AverageLength");
            Min = GetColumn<double>("MinLength");
            Max = GetColumn<double>("MaxLength");
            Total = GetColumn<double>("TotalTime");
            Users = GetColumn<int>("Users");
            Logins = GetColumn<int>("Logins");
        }
    }
    public class ApplicationUseSearchResult : DbDataModel
    {
        public Guid Guid { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string EDIPI { get; private set; }
        public double Average { get; private set; }
        public double Min { get; private set; }
        public double Max { get; private set; }
        public double Total { get; private set; }
        public int Logins { get; private set; }

        protected override void Init()
        {
            Guid = GetColumn<Guid>("Guid");
            Name = GetColumn<string>("Name");
            Email = GetColumn<string>("Email");
            EDIPI = GetColumn<string>("EDIPI");
            Average = GetColumn<double>("AverageLength");
            Min = GetColumn<double>("MinLength");
            Max = GetColumn<double>("MaxLength");
            Total = GetColumn<double>("TotalTime");
            Logins = GetColumn<int>("Logins");
        }
    }

    public class ApplicationErrorMetricsResult : DbDataModel
    {
        public string Label { get; private set; }
        public int Count { get; private set; }
        public DateTime Date { get; private set; }

        protected override void Init()
        {
            Label = GetFormattedDateTimeStringColumn("Date", DateTimeFormatConfiguration.Date);
            Count = GetColumn<int>("Count");
            Date = GetColumn<DateTime>("Date");
        }
    }
    public class ApplicationErrorResult : DbDataModel
    {
        public Guid Guid { get; private set; }
        public string Type { get; private set; }
        public string Message { get; private set; }
        public string StackTrace { get; private set; }
        public string Source { get; private set; }
        public string InnerMessage { get; private set; }
        public string InnerStackTrace { get; private set; }
        public string InnerSource { get; private set; }
        public DateTime CreateDate { get; private set; }
        public string CreateDateDisplay { get; private set; }

        protected override void Init()
        {
            Guid = GetColumn<Guid>("Guid");
            Type = GetColumn<string>("Type");
            Message = GetColumn<string>("Message");
            StackTrace = GetColumn<string>("StackTrace");
            Source = GetColumn<string>("Source");
            InnerMessage = GetColumn<string>("InnerMessage");
            InnerStackTrace = GetColumn<string>("InnerStackTrace");
            InnerSource = GetColumn<string>("InnerSource");
            CreateDate = GetColumn<DateTime>("CreateDate");
            CreateDateDisplay = GetFormattedDateTimeStringColumn("CreateDate", DateTimeFormatConfiguration.Date);
        }
    }
}