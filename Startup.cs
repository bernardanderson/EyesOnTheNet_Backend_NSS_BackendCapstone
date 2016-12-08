using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EyesOnTheNet.Private;
using EyesOnTheNet.TokenProvider;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;

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
            ///// Start of JWT Authentication Middleware
            //app.UseStaticFiles();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(PrivateParameters.JWTSecretKey)); // JWTSecretKey is Private and known to the server ONLY

            // Add JWT generation endpoint:
            var options = new TokenProviderOptions
            {
                Audience = "EotWUser",
                Issuer = "EotWServer",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            };

            // Techincally not part of the JWT Middleware, but needed to allow CORS access for the
            //  custom headers I'm using.  THIS needs to be above the app.UseMiddleware below
            app.UseCors(builder =>
                builder.AllowAnyOrigin().WithMethods("GET", "OPTIONS").WithHeaders("Authorization")
            );

            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(options));

            ///// End of JWT Authentication Middleware


            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
