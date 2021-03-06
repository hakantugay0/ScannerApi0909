using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Scanner.Data.Context;
using Scanner.Helper.Extensions;
using Scanner.Helper.Response;
using Scanner.Helper.Security.Encription;
using Scanner.Helper.Security.JWT;
using System;
using System.Collections.Generic;

namespace Scanner.API
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

            string hostServer = Configuration["HOST_SERVER"] ?? ".";
            string serverPort = Configuration["HOST_PORT"] ?? "1433";
            string databaseName = Configuration["DATABASE_NAME"] ?? "Scanner";
            string userName = Configuration["USERNAME"];
            string password = Configuration["SA_PASSWORD"];

            string connectionString;

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                connectionString = Configuration["ConnectionStrings:SqlConStr"];
            }
            else
            {
                connectionString = $"Server={hostServer},{serverPort};Database={databaseName};User Id={userName};Password={password};";
            }

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            services.AddSwaggerDocumentation();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
                    ClockSkew = TimeSpan.Zero
                    //Selam
                };
            });

           

            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (true || env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerDocumentation();
            }

            app.UseApiResponseAndExceptionWrapper(new ApiResponseOptions { ApiVersion = "1.0.0" });


            app.UseCors(options => options.AllowAnyOrigin());

            app.UseHttpsRedirection();

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
