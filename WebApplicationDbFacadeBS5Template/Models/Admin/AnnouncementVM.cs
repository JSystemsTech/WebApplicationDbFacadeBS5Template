using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data;
using WebApplicationDbFacadeBS5Template.DomainLayer.Models.Parameters;
using WebApplicationDbFacadeBS5Template.Extensions.Html;

namespace WebApplicationDbFacadeBS5Template.Models.Admin
{
    internal static class AnnouncementExtensions
    {
        internal const string DefaultValue = "N";
        internal static IEnumerable<(int order, string code, string name)> AnnoucmentPriority = new (int order, string code, string name)[] {
            (3, "L", "Low"),
            (2, "N", "Normal"),
            (1, "H", "High")
        };
        internal static object[][] AnnoucmentPriorityAsSelectArray => AnnoucmentPriority.Select(m => new object[] { m.order, m.name }).ToArray();
        internal static int GetPriorityOrder(string code = DefaultValue)
            => AnnoucmentPriority.Any(m => m.code == code) ?
            AnnoucmentPriority.FirstOrDefault(m => m.code == code).order :
            AnnoucmentPriority.FirstOrDefault(m => m.code == DefaultValue).order;
        internal static string GetPriorityName(string code = DefaultValue)
            => AnnoucmentPriority.Any(m => m.code == code) ?
            AnnoucmentPriority.FirstOrDefault(m => m.code == code).name :
            AnnoucmentPriority.FirstOrDefault(m => m.code == DefaultValue).name;
        internal static string GetPriorityCode(string code = DefaultValue)
            => AnnoucmentPriority.Any(m => m.code == code) ? code : DefaultValue;
        public static SelectList<(int order, string code, string name)> GetAnnoucmentPrioritySelectList(string selectedValue = DefaultValue)
            => AnnoucmentPriority.OrderBy(m => m.order).ToSelectList(m=> new { }, m => m.code, m => m.name, selectedValue);
        public static IEnumerable<AnnouncementVM> ToAnnouncmentList(this IEnumerable<AppAnnouncement> data)
            => data.Select(m => AnnouncementVM.Create(m)).OrderBy(m => m.PriorityOrder).ThenBy(m=> m.ActiveDate);
    }
    public class AnnouncementVM
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }

        [Display(Name = "Image")]
        public HttpPostedFileBase Image { get; set; }

        [Required]
        [Display(Name = "Priority")]
        public string PriorityCode { get; set; }

        [Required]
        [Display(Name = "Active Date")]
        public DateTime ActiveDate { get; set; }

        [Display(Name = "Expire Date")]
        public DateTime? ExpireDate { get; set; }

        [Required]
        public Guid AnnouncementGuid { get; set; }
        public Guid? ImageFileGuid { get; set; }
        public bool IsClone { get; private set; }

        internal int PriorityOrder => AnnouncementExtensions.GetPriorityOrder(PriorityCode);
        public string PriorityName => AnnouncementExtensions.GetPriorityName(PriorityCode);
        public string Action { get; private set; }
        public SelectList<(int order, string code, string name)> PrioritySelectList => AnnouncementExtensions.GetAnnoucmentPrioritySelectList(PriorityCode);
        public static AnnouncementVM Create(string action = "AddAnnouncement", string priorityCode = AnnouncementExtensions.DefaultValue) => new AnnouncementVM() {
            PriorityCode = AnnouncementExtensions.GetPriorityCode(priorityCode),
            ActiveDate = DateTime.Now,
            Action = action
        };

        public static AnnouncementVM Create(AppAnnouncement vm, string action= "AddAnnouncement") => new AnnouncementVM() {
            AnnouncementGuid = vm.Guid,
            Title = vm.Title,
            Message = vm.Message,
            ActiveDate = vm.ActiveDate,
            ExpireDate = vm.ExpireDate,
            ImageFileGuid = vm.ImageFileGuid,
            PriorityCode = AnnouncementExtensions.GetPriorityCode(vm.PriorityCode),
            Action = action
        };
        public AppAnnouncementAddParameters ToAddParameters() => new AppAnnouncementAddParameters()
        {
            Title = Title,
            Message = Message,
            ActiveDate = ActiveDate,
            ExpireDate = ExpireDate,
            ImageFileGuid = ImageFileGuid,
            PriorityCode = PriorityCode,
            ImageFile = Image
        };
        public AppAnnouncementUpdateParameters ToUpdateParameters() => new AppAnnouncementUpdateParameters()
        {
            Guid = AnnouncementGuid,
            Title = Title,
            Message = Message,
            ActiveDate = ActiveDate,
            ExpireDate = ExpireDate,
            ImageFileGuid = ImageFileGuid,
            PriorityCode = PriorityCode,
            ImageFile = Image
        };
    }
}