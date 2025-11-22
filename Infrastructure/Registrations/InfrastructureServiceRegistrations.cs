using Application.Contracts.EmailSender;
using Application.Contracts.Repositories;
using Application.Contracts.Seeders;
using Application.Contracts.Services;
using Infrastructure.Data;
using Infrastructure.Data.Seeders;
using Infrastructure.IdentityEntities;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure.Extensions
{
    public static class InfrastructureServiceRegistrations
    {
        public static void AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
     
            
            services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            
            services.AddScoped<IProductSeeder, ProductSeeder>();

            services.AddTransient<IEmailService,EmailService>();

            services.Configure<EmailOptions>(configuration.GetSection("EmailSettings"));

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IDeliveryMethodSeeder, DeliveryMethodSeeder>();

            services.AddScoped<IPaymentService, PaymentService>();

            services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                var connectionString = configuration.GetConnectionString("Redis");
                if (connectionString == null) throw new Exception("cannot get redis connection string");
                var configurationOption = ConfigurationOptions.Parse(connectionString, true);
                return ConnectionMultiplexer.Connect(configurationOption);

            });
            services.AddSingleton<ICartService, CartService>();

            services.AddIdentityCore<AppUser>()
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddUserManager<UserManager<AppUser>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
