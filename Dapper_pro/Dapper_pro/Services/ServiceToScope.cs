
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Dapper_pro.Interfaces;
    using Dapper_pro.Services;
    using Dapper_pro.Interfaces;
using Dapper_pro.Utility;

namespace Dapper_pro.Services
{
    public class ServiceToScope
    {
        public ServiceToScope(IConfiguration? configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        public void AddToScope(IServiceCollection services)
        {
            services.AddTransient<IAdmin>(s => (IAdmin)new AdminService(Configuration.GetSection("ConnectionStrings:ConnectionString1").Value));

            services.AddTransient<ITeacher>(s => new TeacherService(Configuration.GetSection("ConnectionStrings:ConnectionString1").Value));

            services.AddTransient<IStudent>(s => new StudentService(Configuration.GetSection("ConnectionStrings:ConnectionString1").Value));
        }
    }
}
    

