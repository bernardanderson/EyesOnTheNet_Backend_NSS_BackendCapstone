using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using EyesOnTheNet.Private;
using EyesOnTheNet.TokenProvider;
using Microsoft.AspNetCore.Http;

namespace EyesOnTheNet
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            ///// Start of JWT Middleware
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(PrivateParameters.JWTSecretKey)); // JWTSecretKey is Private and known to the server ONLY

            /// Start of JWT Cookie Authentication 
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = "EotWServer",

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = "EotWUser",

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                //LoginPath = new PathString("/Account/Unauthorized/"),
                //AccessDeniedPath = new PathString("/Account/Forbidden/"),
                LoginPath = new PathString("/"),
                AccessDeniedPath = new PathString("/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                AuthenticationScheme = "Cookie",
                CookieName = "access_token",
                TicketDataFormat = new CustomJwtDataFormat(
                    SecurityAlgorithms.HmacSha256,
                    tokenValidationParameters)
            });
            /// End of JWT Cookie Authentication 

            /// Start of JWT generation endpoint
            TokenProviderOptions options = new TokenProviderOptions
            {
                Audience = "EotWUser",
                Issuer = "EotWServer",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };

            // Technically not part of the JWT middleware but needed for CORS acc
            app.UseCors(builder =>
                builder
            //    .WithOrigins("http://www.eyesonthenet.com").AllowCredentials().AllowAnyHeader().AllowAnyMethod()  // For a specific domain
            .WithOrigins("http://10.0.0.136:4200", "http://10.0.0.136:5000", "http://localhost:4200", "http://192.168.56.102", "http://192.168.56.102:4200", "http://192.168.56.102:5000", "http://192.168.0.229", "http://192.168.0.229:4200", "http://192.168.0.229:5000").AllowCredentials().AllowAnyHeader().AllowAnyMethod()  // For a specific domain
            //.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()  // This works but opens the whole thing up!
            //builder.AllowAnyOrigin().WithMethods("GET", "OPTIONS")
            );

            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(options));
            /// End of JWT generation endpoint
            ///// End of JWT Authentication Middleware

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
