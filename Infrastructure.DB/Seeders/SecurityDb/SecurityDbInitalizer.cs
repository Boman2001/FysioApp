using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Core.Infrastructure.Seeders.SecurityDb
{
    public class SecurityDbInitalizer
    {
        public static void SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync("Drik@deDoktor.com").Result == null)
            {
                List<Tuple<string, bool>> usernames = new List<Tuple<string, bool>>()
                {
                    new Tuple<string, bool>("Drik@deDoktor.com", true),
                    new Tuple<string, bool>("stefan@DeStudent.com", true),
                    new Tuple<string, bool>("Paula@vanderpatientenberg.com", false),
                    new Tuple<string, bool>("Pavlov@PatientStan.com", false),
                };

                usernames.ForEach(username =>
                {
                    var user = new IdentityUser()
                    {
                        UserName = username.Item1,
                        Email = username.Item1,
                        NormalizedUserName = username.Item1.ToUpper(),
                        NormalizedEmail = username.Item1.ToUpper()
                    };

                    var password = "GxEZMx8QUJTn8Z3";

                    var result = userManager.CreateAsync(user, password).Result;

                    if (result.Succeeded)
                    {
                        if (username.Item2)
                        {
                            userManager.AddToRoleAsync(user, "Staff").Wait();
                        }
                        else
                        {
                            userManager.AddToRoleAsync(user, "Patient").Wait();
                        }
                    }
                });
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Staff").Result)
            {
                List<IdentityRole> identityRoles = new List<IdentityRole>()
                {
                    new IdentityRole()
                    {
                        Name = "Staff"
                    },
                    new IdentityRole()
                    {
                        Name = "Patient"
                    },
                };

                identityRoles.ForEach(role => { roleManager.CreateAsync(role); });
            }
        }
    }
}