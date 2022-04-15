﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WebApplicationDbFacadeBS5Template.Models.Helpers.DataTable
{
    public interface IMappable
    {
        object MapValue(string propertyName);
    }
    public class Helper
    {
        public static Func<object, object> CreateGetter(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var getter = property.GetGetMethod();
            if (getter == null)
                throw new ArgumentException("The specified property does not have a public accessor.");

            var genericMethod = typeof(Helper).GetMethod("CreateGetterGeneric");
            MethodInfo genericHelper = genericMethod.MakeGenericMethod(property.DeclaringType, property.PropertyType);
            return (Func<object, object>)genericHelper.Invoke(null, new object[] { getter });
        }

        public static Func<object, object> CreateGetterGeneric<T, R>(MethodInfo getter) where T : class
        {
            Func<T, R> getterTypedDelegate = (Func<T, R>)Delegate.CreateDelegate(typeof(Func<T, R>), getter);
            Func<object, object> getterDelegate = (Func<object, object>)((object instance) => getterTypedDelegate((T)instance));
            return getterDelegate;
        }

    }
    public class PropertyInfo<T>
        where T : class
    {
        private static ConcurrentDictionary<string, Func<object, object>> Getters { get; set; }
        private static IEnumerable<PropertyInfo> _Properties { get; set; }
        public static IEnumerable<PropertyInfo> Properties = _Properties ?? typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        private static IEnumerable<string> _PropertyNames { get; set; }
        public static IEnumerable<string> PropertyNames = _PropertyNames ?? Properties.Select(p => p.Name);
        public static object GetValue(T model, string name)
        {
            if (model is IMappable mappableModel)
            {
                return mappableModel.MapValue(name);
            }
            if (Getters == null)
            {
                Getters = new ConcurrentDictionary<string, Func<object, object>>();
            }

            if (!Getters.ContainsKey(name) && Properties.FirstOrDefault(p => p.Name == name) is PropertyInfo propInfo)
            {
                Getters.TryAdd(name, Helper.CreateGetter(propInfo));
            }
            if (Getters.TryGetValue(name, out Func<object, object> getter))
            {
                return getter(model);
            }
            return null;
        }
    }
    public static class DataTableExtensions
    {
        private static object GetValue<T>(this T model, string name) where T : class
            => PropertyInfo<T>.GetValue(model, name);
        private static string GetValueAsString<T>(this T model, string name) where T : class
        {
            object value = model.GetValue(name);
            return value != null && value is DateTime date ? date.ToString("yyyy/MM/dd HH:mm:ss:fff") : value != null ? value.ToString() : "";
        }
        private static bool IsMatch(this string value, DataTableSearch settings)
        {
            return settings.Regex ? Regex.Match(value, settings.Value, RegexOptions.IgnoreCase).Success : value.ContainsCaseInsensitive(settings.Value);
        }
        private static bool ContainsCaseInsensitive(this string source, string toCheck)
        => source?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        
        private static bool IsMatch<T>(this T model, DataTableColumnSearch settings, DataTableOptions<T> options) where T : class
        {
            if(options != null && options.DisplayedColumns.FirstOrDefault(c => c.Name == settings.Column.Name) is DataTableColumnOptions<T> columnOptions)
            {
                string value = columnOptions.GetValue(model).ToString();
                return settings.Search.Regex ? Regex.Match(value, settings.Search.Value, RegexOptions.IgnoreCase).Success : 
                    value.ContainsCaseInsensitive(columnOptions.GetFilterValue(settings.Search.Value));
                
            }                   
            return model.GetValueAsString(settings.Column.Name).IsMatch(settings.Search);
        }
        private static IEnumerable<T> GeneralSearchDataTable<T>(this IEnumerable<T> data, DataTableRequest request, DataTableOptions<T> options = default(DataTableOptions<T>)) where T : class
        {
            if(request.Download && request.DownloadAll)
            {
                return data;
            }            
            var searchColumns = request.GetTableSearchColumns().Where(c=>c.Search != null);
            return searchColumns.Count()> 0 ? data.Where(item => searchColumns.Any(c => item.IsMatch(c, options))): data;
        }
        private static IEnumerable<T> ColumnSpecificSearchDataTable<T>(this IEnumerable<T> data, DataTableRequest request, DataTableOptions<T> options = default(DataTableOptions<T>)) where T : class
        {
            if (request.Download && request.DownloadAll)
            {
                return data;
            }
            var searchColumns = request.GetSearchColumns().Where(c => c.Search != null);
            return searchColumns.Count() > 0 ? data.Where(item => searchColumns.All(c => item.IsMatch(c, options))) : data;
        }
        private static IEnumerable<T> OrderDataTable<T>(this IEnumerable<T> data, DataTableRequest request,
            DataTableOptions<T> options) where T : class
        {
            IEnumerable<DataTableColumnOrder> orderCols = request.GetOrderColumns();
            if (orderCols.Count() == 0)
            {
                return data;
            }
            IOrderedEnumerable<T> orderedData = null;
            for (int i = 0; i < orderCols.Count(); i++)
            {
                DataTableColumnOrder col = orderCols.ElementAt(i);
                if(options.Columns.FirstOrDefault(c => c.Name == col.Name) is DataTableColumnOptions<T> colOptions)
                {
                    orderedData =
                    i == 0 && col.Dir.ToLower() == "desc" ? data.OrderByDescending(colOptions.GetValue) :
                    i == 0 ? data.OrderBy(colOptions.GetValue) :
                    col.Dir.ToLower() == "desc" ? orderedData.ThenByDescending(colOptions.GetValue) :
                    orderedData.ThenBy(colOptions.GetValue);
                }
            }
            return orderedData;
        }
        private static IEnumerable<T> GetDataTablePage<T>(this IEnumerable<T> data, DataTableRequest request) where T : class
        {
            if (request.Download && request.DownloadAll)
            {
                return data;
            }
            return new ArraySegment<T>(data.ToArray(), request.Start, data.Count() - (request.Start + request.Length) < 0 ? data.Count() - request.Start : request.Length);
        }
        public static DataTableResponse ProcessDataTable<T>(
            this IEnumerable<T> data,
            DataTableRequest request,
            DataTableOptions<T> options) where T : class
        {
            try
            {
                var filteredData = data.GeneralSearchDataTable(request, options).ColumnSpecificSearchDataTable(request, options);
                var orderedData = filteredData.OrderDataTable(request, options);
                var pageData = orderedData.GetDataTablePage(request);

                var serializedData = options.GetData(pageData);
                return new DataTableResponse(request, serializedData, data.Count(), filteredData.Count(), () => options.ToSystemDataTable(pageData, options.DataName));                
            }
            catch (Exception e)
            {
                return new DataTableResponse(request, e);
            }
        }

        
    }
    public class DataTableResponse<T>
        where T : class
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public IEnumerable<T> data { get; set; }
        public string error { get; set; }

        public DataTableResponse(DataTableRequest request, IEnumerable<T> data, int total, int filtered)
        {
            draw = request.Draw;
            this.data = data;
            recordsTotal = total;
            recordsFiltered = filtered;
        }
        public DataTableResponse(DataTableRequest request, Exception e)
        {
            draw = request.Draw;
            data = new T[0];
            recordsTotal = 0;
            recordsFiltered = 0;
            error = e.Message;
        }

    }
    public class DataTableResponse
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public IEnumerable<object> data { get; set; }
        public string error { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        internal Func<System.Data.DataTable> ToSystemDataTable { get; private set; }

        public DataTableResponse(DataTableRequest request, IEnumerable<object> data, int total, int filtered, Func<System.Data.DataTable> toSystemDataTable)
        {
            draw = request.Draw;
            this.data = data;
            recordsTotal = total;
            recordsFiltered = filtered;
            ToSystemDataTable = toSystemDataTable;
        }
        private static System.Data.DataTable EmptyDataTable = new System.Data.DataTable("EmptyTable"); 
        public DataTableResponse(DataTableRequest request, Exception e)
        {
            draw = request.Draw;
            data = new object[0];
            recordsTotal = 0;
            recordsFiltered = 0;
            error = e.Message;
            ToSystemDataTable = () => EmptyDataTable; 
        }

    }
    public class DataTableRequest
    {

        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public bool Download { get; set; }
        public bool DownloadAll { get; set; }
        public string FileName { get; set; }
        public IEnumerable<DataTableOrder> Order { get; set; }
        public IEnumerable<DataTableColumn> Columns { get; set; }
        public DataTableSearch Search { get; set; }

        public IEnumerable<DataTableColumnSearch> GetSearchColumns()
        => Columns.Where(c => c.Searchable).Select(c => new DataTableColumnSearch(c));
        public IEnumerable<DataTableColumnSearch> GetTableSearchColumns()
        => Columns.Where(c => c.Searchable).Select(c => new DataTableColumnSearch(c,Search));
        public IEnumerable<DataTableColumnOrder> GetOrderColumns()
        => Order.Select(o => new DataTableColumnOrder(Columns.ElementAt(o.Column), o.Dir)).Where(c => c.Orderable);
    }
    public class DataTableRequest<T> : DataTableRequest
    where T : class
    {
        public T Parameters { get; set; }
        public DataTableRequest() { Parameters = default(T); }
    }

    public class DataTableOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }
    public class DataTableSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }
    public class DataTableColumn
    {
        public Guid Guid { get; private set; }
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public DataTableSearch Search { get; set; }

        public DataTableColumn()
        {
            Guid = Guid.NewGuid();
        }

    }
    public class DataTableColumnOrder
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public string Dir { get; set; }
        public bool Orderable { get; set; }
        public DataTableColumnOrder(DataTableColumn col, string dir)
        {
            Data = col.Data;
            Name = col.Name;
            Dir = dir;
            Orderable = col.Orderable;
        }
    }
    public class DataTableColumnSearch
    {
        public DataTableSearch Search { get; set; }
        public DataTableColumn Column { get; set; }
        public DataTableColumnSearch(DataTableColumn col)
        {
            Column = col;
            if (col.Search != null && col.Search.Value != null)
            {
                Search = col.Search;
            }
        }
        public DataTableColumnSearch(DataTableColumn col, DataTableSearch search)
        {
            Column = col;
            if (search.Value != null)
            {
                Search = search;
            }
        }
    }

    public abstract class PropertyMapperAttribute : Attribute
    {
        private string _PropertyName { get; set; }

        public PropertyMapperAttribute(string propertyName)
        {
            _PropertyName = propertyName;
        }

        public virtual string PropertyName => _PropertyName;


    }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class FilterOnAttribute : PropertyMapperAttribute
    {
        public FilterOnAttribute(string propertyName) : base(propertyName) { }
    }
}