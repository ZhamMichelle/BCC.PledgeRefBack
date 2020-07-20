using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using BCC.PledgeRefBack.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BCC.PledgeRefBack
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
            services.AddCors();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
