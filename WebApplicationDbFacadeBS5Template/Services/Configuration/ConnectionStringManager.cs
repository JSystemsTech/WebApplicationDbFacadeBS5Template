using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApplicationDbFacadeBS5Template.Services.Configuration
{
    public class ConnectionStringManager
    {
        private static ConcurrentDictionary<string, (string connectionString, string connectionProvider)> ConnectionStrings = new ConcurrentDictionary<string, (string connectionString, string connectionProvider)>();

        private static (string connectionString, string connectionProvider) ConfigureConnectionString(string key)
        {
            //TODO: fetech actual values from config or another source
            string connectionString = "Data Source=JMAC_LG_PC\\SQLEXPRESS;Initial Catalog=WebAppDbTemplate;Persist Security Info=True;User ID=WebAppUser;Password=K$j254a2778899;Trusted_Connection=True";
            string connectionProvider = "System.Data.SqlClient";


            var config = (connectionString, connectionProvider);
            ConnectionStrings.TryAdd(key, config);
            return config;
        }
        public static (string connectionString, string connectionProvider) GetConnectionStringConfig(string key)
        {
            if(ConnectionStrings.TryGetValue(key, out (string connectionString, string connectionProvider) config)){
                return config;
            }
            return ConfigureConnectionString(key);
        }
        public static async Task<(string connectionString, string connectionProvider)> GetConnectionStringConfigAsync(string key)
        {
            if (ConnectionStrings.TryGetValue(key, out (string connectionString, string connectionProvider) config))
            {
                await Task.CompletedTask;
                return config;
            }
            await Task.CompletedTask;
            return ConfigureConnectionString(key);
        }
    }
}