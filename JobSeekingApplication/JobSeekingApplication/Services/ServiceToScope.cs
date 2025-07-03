//using JobSeekingApplication.Interface;

//namespace JobSeekingApplication.Services
//{
//    public class ServiceToScope
//    {
//        public ServiceToScope(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }
//        public IConfiguration Configuration { get; set; }
//        public void AddToScope(IServiceCollection services)
//        {
//            services.AddTransient<IAuth>(s => (IAuth)new AuthService(Configuration.GetSection("ConnectionStrings:ConnectionString1").Value));
//        }
//    }
//}
