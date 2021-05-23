using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PetMotel.Common;
using PetMotel.Common.Rest.Entity;

namespace PetMotel.Identity.Data
{
    public class DataGenerator
    {
        public static void SeedData(UserManager<PetMotelUser> userManager, RoleManager<PetMotelRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<PetMotelUser> userManager)
        {
            PetMotelUser root = new PetMotelUser
            {
                PublicName = "binarybird",
                UserName = "jamesrichardson2@gmail.com",
                Email = "jamesrichardson2@gmail.com",
                EmailConfirmed = true,
            };
            IdentityResult result = userManager.CreateAsync(root, "Passw0rd!").Result;
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(root, Constants.Roles.Root).Wait();
                userManager.AddToRoleAsync(root, Constants.Roles.Admin).Wait();
                userManager.AddToRoleAsync(root, Constants.Roles.Manager).Wait();
                userManager.AddToRoleAsync(root, Constants.Roles.Employee).Wait();
                userManager.AddToRoleAsync(root, Constants.Roles.Contributor).Wait();
                userManager.AddToRoleAsync(root, Constants.Roles.User).Wait();
            }
            else
            {
                IEnumerable<string> msgs = result.Errors.Select(s => $"Error Code:{s.Code} Message: {s.Description}");
                throw new AuthenticationException(String.Join("\n", msgs));
            }
            
            PetMotelUser user = new PetMotelUser
            {
                PublicName = "binarybird",
                UserName = "user@example.com",
                Email = "user@example.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
            };
            result = userManager.CreateAsync(user, "Passw0rd!").Result;
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, Constants.Roles.User).Wait();
            }
            else
            {
                IEnumerable<string> msgs = result.Errors.Select(s => $"Error Code:{s.Code} Message: {s.Description}");
                throw new AuthenticationException(String.Join("\n", msgs));
            }
        }

        public static void SeedRoles(RoleManager<PetMotelRole> roleManager)
        {
            var root = new PetMotelRole
            {
                ConcurrencyStamp = null,
                Id = Guid.NewGuid().ToString(),
                Name = Constants.Roles.Root,
                NormalizedName = Constants.Roles.Root,
                Description = "Access to root functions"
            };
            IdentityResult roleResult = roleManager.CreateAsync(root).Result;
            if (!roleResult.Succeeded)
            {
                IEnumerable<string> msgs = roleResult.Errors.Select(s => $"Error Code:{s.Code} Message: {s.Description}");
                throw new AuthenticationException(String.Join("\n", msgs));
            }

            var admin = new PetMotelRole
            {
                ConcurrencyStamp = null,
                Id = Guid.NewGuid().ToString(),
                Name = Constants.Roles.Admin,
                NormalizedName = Constants.Roles.Admin,
                Description = "Access to admin functions"
            };
            roleResult = roleManager.CreateAsync(admin).Result;
            if (!roleResult.Succeeded)
            {
                IEnumerable<string> msgs = roleResult.Errors.Select(s => $"Error Code:{s.Code} Message: {s.Description}");
                throw new AuthenticationException(String.Join("\n", msgs));
            }
            
            var manager = new PetMotelRole
            {
                ConcurrencyStamp = null,
                Id = Guid.NewGuid().ToString(),
                Name = Constants.Roles.Manager,
                NormalizedName = Constants.Roles.Manager,
                Description = "Access to Manager functions"
            };
            roleResult = roleManager.CreateAsync(manager).Result;
            if (!roleResult.Succeeded)
            {
                IEnumerable<string> msgs = roleResult.Errors.Select(s => $"Error Code:{s.Code} Message: {s.Description}");
                throw new AuthenticationException(String.Join("\n", msgs));
            }
            
            var employee = new PetMotelRole
            {
                ConcurrencyStamp = null,
                Id = Guid.NewGuid().ToString(),
                Name = Constants.Roles.Employee,
                NormalizedName = Constants.Roles.Employee,
                Description = "Access to Employee functions"
            };
            roleResult = roleManager.CreateAsync(employee).Result;
            if (!roleResult.Succeeded)
            {
                IEnumerable<string> msgs = roleResult.Errors.Select(s => $"Error Code:{s.Code} Message: {s.Description}");
                throw new AuthenticationException(String.Join("\n", msgs));
            }
            
            var contributor = new PetMotelRole
            {
                ConcurrencyStamp = null,
                Id = Guid.NewGuid().ToString(),
                Name = Constants.Roles.Contributor,
                NormalizedName = Constants.Roles.Contributor,
                Description = "Access to ReadOnly Employee functions"
            };
            roleResult = roleManager.CreateAsync(contributor).Result;
            if (!roleResult.Succeeded)
            {
                IEnumerable<string> msgs = roleResult.Errors.Select(s => $"Error Code:{s.Code} Message: {s.Description}");
                throw new AuthenticationException(String.Join("\n", msgs));
            }
            
            var user = new PetMotelRole
            {
                ConcurrencyStamp = null,
                Id = Guid.NewGuid().ToString(),
                Name = Constants.Roles.User,
                NormalizedName = Constants.Roles.User,
                Description = "Access to Manager functions"
            };
            roleResult = roleManager.CreateAsync(user).Result;
            if (!roleResult.Succeeded)
            {
                IEnumerable<string> msgs = roleResult.Errors.Select(s => $"Error Code:{s.Code} Message: {s.Description}");
                throw new AuthenticationException(String.Join("\n", msgs));
            }
        }

        // public static void Initialize(IServiceProvider serviceProvider)
        // {
        //     using (var context =
        //         new PetMotelIdentityContext(serviceProvider
        //             .GetRequiredService<DbContextOptions<PetMotelIdentityContext>>()))
        //     {
        //         InitUsers(context);
        //         InitRoles(context);
        //     }
        // }

        // private static void InitUsers(PetMotelIdentityContext context)
        // {
        //     if (context.Users.Any())
        //     {
        //         return;
        //     }
        //
        //     var rootUser = new PetMotelUser
        //     {
        //         AccessFailedCount = 0,
        //         ConcurrencyStamp = "null",
        //         Email = "user@example.com",
        //         EmailConfirmed = true,
        //         NormalizedEmail = "user@example.com",
        //         UserName = "root",
        //         NormalizedUserName = null,
        //     };
        //     
        //     IdentityResult result = userManager.CreateAsync
        //         (user, "password_goes_here").Result;
        //
        //     if (result.Succeeded)
        //     {
        //         userManager.AddToRoleAsync(user,
        //             "NormalUser").Wait();
        //     }
        //     
        //     
        //     context.Users.AddRange(
        //         new PetMotelUser
        //         {
        //             AccessFailedCount = 0,
        //             ConcurrencyStamp = "null",
        //             Email = "user@example.com",
        //             EmailConfirmed = true,
        //             NormalizedEmail = "user@example.com",
        //             UserName = "root",
        //             NormalizedUserName = null,
        //             
        //             PasswordHash = null,
        //             PhoneNumber = null,
        //             PhoneNumberConfirmed = false,
        //             SecurityStamp = null,
        //             TwoFactorEnabled = false,
        //
        //         }
        //         
        //         
        //     );
        // }
        //
        // private static void InitRoles(PetMotelIdentityContext context)
        // {
        //     if (context.Roles.Any())
        //     {
        //         return;
        //     }
        //
        //     context.Roles.AddRange(new PetMotelRole
        //         {
        //             ConcurrencyStamp = null,
        //             Id = Guid.NewGuid().ToString(),
        //             Name = Roles.Root,
        //             NormalizedName = Roles.Root,
        //             Description = "Access to root functions"
        //         },
        //         new PetMotelRole
        //         {
        //             ConcurrencyStamp = null,
        //             Id = Guid.NewGuid().ToString(),
        //             Name = Roles.Admin,
        //             NormalizedName = Roles.Admin,
        //             Description = "Access to admin functions"
        //         },
        //         new PetMotelRole
        //         {
        //             ConcurrencyStamp = null,
        //             Id = Guid.NewGuid().ToString(),
        //             Name = Roles.Manager,
        //             NormalizedName = Roles.Manager,
        //             Description = "Access to Manager functions"
        //         },
        //         new PetMotelRole
        //         {
        //             ConcurrencyStamp = null,
        //             Id = Guid.NewGuid().ToString(),
        //             Name = Roles.Employee,
        //             NormalizedName = Roles.Employee,
        //             Description = "Access to Employee functions"
        //         },
        //         new PetMotelRole
        //         {
        //             ConcurrencyStamp = null,
        //             Id = Guid.NewGuid().ToString(),
        //             Name = Roles.Contributor,
        //             NormalizedName = Roles.Contributor,
        //             Description = "Access to ReadOnly Employee functions"
        //         },
        //         new PetMotelRole
        //         {
        //             ConcurrencyStamp = null,
        //             Id = Guid.NewGuid().ToString(),
        //             Name = Roles.User,
        //             NormalizedName = Roles.User,
        //             Description = "Access to Manager functions"
        //         });
        // }
    }
}