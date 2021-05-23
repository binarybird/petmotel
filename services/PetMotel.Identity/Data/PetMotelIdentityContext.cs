using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetMotel.Common.Rest.Entity;
using PetMotel.Common.Rest.Model;

namespace PetMotel.Identity.Data
{
    public class PetMotelIdentityContext : IdentityDbContext<PetMotelUser, PetMotelRole, string>
    {
        public PetMotelIdentityContext(DbContextOptions<PetMotelIdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<LoginRequestModel> LoginModel { get; set; }
    }
}
