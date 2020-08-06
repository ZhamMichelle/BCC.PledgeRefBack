using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Bcc.Pledg.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Bcc.Pledg.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Bcc.Pledg.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bcc.Pledg
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = false,
                           ValidateAudience = false,
                           ValidateLifetime = true,
                           ValidateIssuerSigningKey = true,
                           IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(Configuration["SecretKey"]))))
                       };
                   });

            services.AddTransient<IAuthorizationHandler, PolicyHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DMOD", policy => policy.Requirements.Add(new PolicyRequirement(new List<string> { "DMOD" })));
            });

            //services.AddControllers().AddNewtonsoftJson(options =>
            //{
            //    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            //});

            services.AddDbContext<PostgresContext>(options =>
            options.UseNpgsql($"{Configuration.GetConnectionString("DefaultConnection")};Password={Configuration.GetConnectionString("DefaultPassword")}")
            );

            
            services.
                AddMvc(o => o.Conventions.Add(
                    new GenericControllerRouteConvention()
                )).
                ConfigureApplicationPartManager(m =>
                    m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider()
                ));
            services.AddControllers();
            services.AddHealthChecks();
            services.AddHostedService<LoaderService>();
            services.AddReference<SectorsCity>("Resources/SectorsCity.json");

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/"), builder =>
            {
                builder.Use((context, next) =>
                {
                    return Task.Run(() =>
                    {
                        var body = new StringBuilder();
                        foreach (var res in ReferenceContext.Resources)
                        {
                            body.Append($"<a href='/api/base/{res.Key.Name}'>/api/base/{res.Key.Name}</a><br/>");
                            body.AppendLine();
                        }
                        context.Response.WriteAsync(body.ToString());
                    });
                });
            });

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHealthChecks("/");
                endpoints.MapControllers();
            });
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReference<T>(this IServiceCollection services, string jsonFileName)
        {
            ReferenceContext.Resources.TryAdd(typeof(T), jsonFileName);
            return services;
        }
    }
}
