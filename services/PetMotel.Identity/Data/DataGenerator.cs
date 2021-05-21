using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetMotel.Identity.Data
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new PetMotelIdentityContext(serviceProvider.GetRequiredService<DbContextOptions<PetMotelIdentityContext>>()))
            {
                if(context.Users.Any())
                {
                    return;
                }

                context.Users.AddRange(
                    new PetMotelUser
                    {
                        AccessFailedCount = 0,
                        ConcurrencyStamp = "null",
                        Email = "user@example.com",
                        EmailConfirmed = true,
                        Id = "1",
                        LockoutEnabled = false,
                        LockoutEnd = null,
                        NormalizedEmail = "user@example.com",
                        NormalizedUserName = null,
                        PasswordHash = null,
                        PhoneNumber = null,
                        PhoneNumberConfirmed = false,
                        SecurityStamp = null,
                        TwoFactorEnabled = false,
                        UserName = "root"
                    }
                );
            }
        }
    }
}
