using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationDbFacadeBS5Template.Models
{
    public class ListManagementItem
    {
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public object Value { get; internal set; }
        public bool Selected { get; internal set; }
    }
    public class ListManagementRequestItem<T>
    {
        public T Value { get; set; }
        public bool Selected { get; set; }
    }
    public class ListManagementVM
    {
        public string Url { get; private set; }
        public IEnumerable<ListManagementItem> Items { get; private set; }

        public static ListManagementVM Create<T>(string url, IEnumerable<T> data, Func<T,string> getName, Func<T, string> getDescription, Func<T, object> getValue, Func<T, bool> isSelected)
        {
            return new ListManagementVM()
            {
                Url = url,
                Items = data.Select(m => new ListManagementItem() { Name = getName(m),Description= getDescription(m), Value = getValue(m), Selected = isSelected(m) })
            };
        }
    }
}