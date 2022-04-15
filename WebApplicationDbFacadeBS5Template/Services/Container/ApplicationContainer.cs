using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationDbFacadeBS5Template.Services.Container
{
    public class ApplicationContainer
    {
        private static readonly ConcurrentDictionary<Type, object> Services
        = new ConcurrentDictionary<Type, object>();
        private static readonly ConcurrentDictionary<Type, Func<object>> ServiceMap
            = new ConcurrentDictionary<Type, Func<object>>();

        public static void ConfigureService<T>(Func<T> handler)
        {
            ServiceMap.TryAdd(typeof(T), () => handler());
        }
        private static bool TryGet<T>(out T service)
        {
            if (Services.TryGetValue(typeof(T), out object value) && value is T valueAsService)
            {
                service = valueAsService;
                return true;
            }
            else if (Services.FirstOrDefault(kv => kv.Value is T) is KeyValuePair<Type, object> entry && entry.Value is T valueAsServiceSecondary)
            {
                service = valueAsServiceSecondary;
                return true;
            }
            service = default(T);
            return false;
        }
        public static T Get<T>()
        {
            if (TryGet(out T service))
            {
                return service;
            }
            else
            {
                Func<object> constructor = ServiceMap.TryGetValue(typeof(T), out Func<object> handler) ? handler : () => Activator.CreateInstance(typeof(T));
                T newService = constructor() is T value ? value : default(T);
                Services.TryAdd(typeof(T), newService);
                return newService;
            }
        }
    }
}