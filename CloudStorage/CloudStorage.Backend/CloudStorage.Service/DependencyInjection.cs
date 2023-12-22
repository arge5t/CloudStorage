using CloudStorage.Persistence.Interfaces;
using CloudStorage.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CloudStorage.Services.Implementations;
using CloudStorage.Services.Interfaces;
using CloudStorage.Domain.Entities;

namespace CloudStorage.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ITokenService, TokenService>();

            services.Configure<MailClient>(options => configuration.GetSection("MailClient").Bind(options));

            return services;
        }
    }
}
