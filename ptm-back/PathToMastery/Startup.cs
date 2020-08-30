using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using PathToMastery.Models;
using PathToMastery.Services;
using PathToMastery.Services.Abstract;

namespace PathToMastery
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });

            services.AddSingleton<ConcurrencyService>();
            services.AddSingleton<IDbService, MongoService>();
            services.AddSingleton<ISocialService, VkService>();
            services.AddSingleton<PathService>();

            services.AddHostedService<NotificationService>();
        }

        public void Configure(IApplicationBuilder app, IDbService dbService)
        {
            app.UseRouting();
            app.UseFileServer();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            dbService.Init("pathtomastery", type =>
            {
                if (type == typeof(User)) return "users";
                throw new ArgumentOutOfRangeException(nameof(type), $"No collection for type: {type.FullName}");
            });
        }
    }
}