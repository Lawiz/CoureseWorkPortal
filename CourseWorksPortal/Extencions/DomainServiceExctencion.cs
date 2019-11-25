using CourseWorksPortal.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CourseWorksPortal.Extencions
{
    public static class DomainServiceExctencion
    {
        public static void RegistrService(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
        }
        
    }
}