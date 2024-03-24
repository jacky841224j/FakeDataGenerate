using MySql.Data.MySqlClient;
using System.Data;

namespace FakeDataGenerate.Extensions
{
    public static class DapperExtensions
    {

        public static IServiceCollection AddDapper(this IServiceCollection services, string constr)
        {
            services.AddScoped<IDbConnection, MySqlConnection>(serviceProvider =>
            {
                var conn = new MySqlConnection();
                conn.ConnectionString = constr;
                return conn;
            });
            return services;
        }
    }
}
