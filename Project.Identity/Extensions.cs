using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Project.Common.Types;
using Project.Core;

namespace Project.Identity
{
    public static class Extensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims

            var jwtOptions = new JwtOptions();
            configuration.GetSection(JwtOptions.SectionName).Bind(jwtOptions);
            services.AddSingleton(jwtOptions);
            services.AddTransient<IJwtHandler, JwtHandler>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtOptions.JwtIssuer,
                        ValidAudience = jwtOptions.JwtIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.JwtKey)),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            return services;
        }
        public static void AddCustomIdentity(this IServiceCollection services)
        {
            services.AddTransient<UserManager>(s =>
            {
                var passwordHasher = s.GetRequiredService<IPasswordHasher<User>>();
                var userStore = s.GetRequiredService<IRepository<User>>();

                if (userStore == null)
                    throw new Exception("Provide user store of type <IRepository<User>>");

                return new UserManager(userStore, passwordHasher);
            });
        }

        public static string GetId(this ClaimsPrincipal userPrincipal)
            => userPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        public static T GetId<T>(this ClaimsPrincipal userPrincipal)
            => (T)Convert.ChangeType(userPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, typeof(T));

        public static IEnumerable<string> GetRoles(this ClaimsPrincipal userPrincipal)
            => userPrincipal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
    }
}
