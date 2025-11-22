using Application.Options;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



namespace Application.Extensions
{
    public static class ApplicatiosServiceRegistrations
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicatiosServiceRegistrations).Assembly));
            
            services.AddAutoMapper(typeof(ApplicatiosServiceRegistrations).Assembly);

            services.AddValidatorsFromAssembly(typeof(ApplicatiosServiceRegistrations).Assembly);

            

        }
    }
}
