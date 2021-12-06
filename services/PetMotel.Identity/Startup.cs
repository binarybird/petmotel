using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetMotel.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using PetMotel.Common;
using PetMotel.Common.Rest;
using PetMotel.Common.Rest.Entity;
using PetMotel.Identity.Email;

namespace PetMotel.Identity
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
            services.AddDbContext<PetMotelIdentityContext>(options =>
                options.UseInMemoryDatabase(databaseName: "PetMotelIdentity"));

            services.AddIdentity<PetMotelUser, PetMotelRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<PetMotelIdentityContext>();
            
            byte[] key = Convert.FromBase64String(Configuration.GetValue<String>(Constants.Config.TokenSigningKey));//Encoding.UTF8.GetBytes(Configuration[Constants.Config.TokenSigningKey])
            var signingKey = new SymmetricSecurityKey(key);
            services.AddAuthentication(options =>  
            {  
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  
            }).AddJwtBearer(config =>  
            {  
                config.RequireHttpsMetadata = false;  
                config.SaveToken = true;  
                config.TokenValidationParameters = new TokenValidationParameters()  
                {  
                    IssuerSigningKey = signingKey,  
                    ValidateAudience = true,  
                    ValidAudience = this.Configuration[Constants.Config.TokenAudience],  
                    ValidateIssuer = true,  
                    ValidIssuer = this.Configuration[Constants.Config.TokenIssuer],  
                    ValidateLifetime = true,  
                    ValidateIssuerSigningKey = true  
                };  
            });  
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "PetMotel.Identity", Version = "v1"});
            });

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(c =>
            {
                c.SendGridKey = "somekey";
                c.SendGridUser = "jamesrichardson2@gmail.com";
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });
            
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env,
            UserManager<PetMotelUser> userManager, 
            RoleManager<PetMotelRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetMotel.Identity v1"));
                DataGenerator.SeedData(userManager, roleManager);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            
        }
    }
}
