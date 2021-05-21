using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetMotel.Identity.Areas.Identity.Data;
using PetMotel.Identity.Data;

[assembly: HostingStartup(typeof(PetMotel.Identity.Areas.Identity.IdentityHostingStartup))]
namespace PetMotel.Identity.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<PetMotelIdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("PetMotelIdentityContextConnection")));

                services.AddDefaultIdentity<PetMotelUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<PetMotelIdentityContext>();
            });
        }
    }
}