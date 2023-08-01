using Microsoft.AspNetCore.SignalR;

namespace Innamoramelo
{
    public class Startup
    {
        public IConfiguration _Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        //public void Configuration(IAppBuilder app)
        //{
        //    var idProvider = new CustomUserIdProvider();

        //    GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

        //    // Any connection or hub wire up and configuration should go here
        //    app.MapSignalR();
        //}

        public void ConfigureServices(IServiceCollection services)
        {
            // ... altri servizi

            services.AddDistributedMemoryCache(); // Opzionale, ma richiesto per l'uso delle sessioni in-process
            services.AddSession(options =>
            {
                options.Cookie.Name = "aspnetcore.session";
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            // ... altri servizi
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ... altri middleware

            app.UseRouting();

            app.UseSession(); // Aggiungi questo middleware prima del middleware MVC

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // ... altre route
            });

            // ... altri middleware
        }
    }
}
