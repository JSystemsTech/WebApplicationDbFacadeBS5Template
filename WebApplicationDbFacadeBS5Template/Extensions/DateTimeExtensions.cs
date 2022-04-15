using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationDbFacadeBS5Template.Services.Configuration;

namespace WebApplicationDbFacadeBS5Template.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToDateString(this DateTime d) => d.ToString(DateTimeFormatConfiguration.Date);
        public static string ToDateTimeString(this DateTime d) => d.ToString(DateTimeFormatConfiguration.DateTime);
        public static string To24HourDateTimeString(this DateTime d) => d.ToString(DateTimeFormatConfiguration.DateTime24Hour);
        public static string ToDateTimeWithSecondsString(this DateTime d) => d.ToString(DateTimeFormatConfiguration.DateTimeWithSeconds);
        public static string To24HourDateTimeWithSecondsString(this DateTime d) => d.ToString(DateTimeFormatConfiguration.DateTime24HourWithSeconds);
        public static string ToMomentSafeDateTimeString(this DateTime d) => d.ToString(DateTimeFormatConfiguration.DateTimeMomentSafe);


        #region DateTime? Extensions
        public static string FormatDateTime(this DateTime? d, string format) => d is DateTime dateTime ? dateTime.ToString(format) : "";
        public static string ToDateString(this DateTime? d) => d.FormatDateTime(DateTimeFormatConfiguration.Date);
        public static string ToDateTimeString(this DateTime? d) => d.FormatDateTime(DateTimeFormatConfiguration.DateTime);
        public static string To24HourDateTimeString(this DateTime? d) => d.FormatDateTime(DateTimeFormatConfiguration.DateTime24Hour);
        public static string ToDateTimeWithSecondsString(this DateTime? d) => d.FormatDateTime(DateTimeFormatConfiguration.DateTimeWithSeconds);
        public static string To24HourDateTimeWithSecondsString(this DateTime? d) => d.FormatDateTime(DateTimeFormatConfiguration.DateTime24HourWithSeconds);
        public static string ToMomentSafeDateTimeString(this DateTime? d) => d.FormatDateTime(DateTimeFormatConfiguration.DateTimeMomentSafe);
        #endregion
    }
}