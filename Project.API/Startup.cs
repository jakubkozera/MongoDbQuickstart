using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Project.API.Extensions;
using Project.Core;
using Project.Identity;
using Project.Infrastructure.Mongo;
using Project.Infrastructure.Mongo.Extensions;

namespace Project.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            
            services.AddMongo();
            services.AddMongoRepository<User>("Users");

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", cors =>
                    cors.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCustomMvc();
            services.AddJwtAuthentication();
            services.AddCustomIdentity();
            services.AddSingleton(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceCollection services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            InitalizeDb(services);

            app.UseAuthentication();

            app.UseMvc();
        }

        private void InitalizeDb(IServiceCollection services)
        {
            Task.Run(async () =>
            {
                using (var builder = services.BuildServiceProvider())
                {
                    var dbInitializer = builder.GetRequiredService<IMongoDbInitializer>();
                    if (dbInitializer != null)
                    {
                        await dbInitializer.InitializeAsync();
                    }
                }
            });
        }
    }
}
