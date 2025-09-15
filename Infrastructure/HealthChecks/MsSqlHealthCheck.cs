using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.HealthChecks
{
    // normalde IHealthCheck interfacesini kullanmaya gerek yok çünkü projede dış servisler yok
    public class MsSqlHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

       

        public MsSqlHealthCheck(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    return HealthCheckResult.Healthy();
                }
            }
            catch (Exception) // 'ex' parametresini kullanmadığımız için kaldırabiliriz.
            {
                // Artık hata mesajı eklemiyoruz, sadece Unhealthy durumunu döndürüyoruz.
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}