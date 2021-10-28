// using System;
// using System.Collections.Generic;
// using Core.Infrastructure.Contexts;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//
// namespace Core.Infrastructure.Seeders
// {
//     public static class SecurityDbInitizalizer
//     {
//         public static void SeedUsers(UserManager<IdentityUser> userManager)
//         {
//             // string[] roles = new string[] { "Staff", "Patient" };
//             //
//             // foreach (string role in roles)
//             // {
//             //     var roleStore = new RoleStore<IdentityRole>(context);
//             //
//             //     if (!context.Roles.Any(r => r.Name == role))
//             //     {
//             //         roleStore.CreateAsync(new IdentityRole(role));
//             //     }
//             // }
//             List<Tuple<string,bool>> emails = new List<Tuple<string,bool>>()
//             {
//                 new Tuple<string, bool>("Danmaarkaas@gmail.com",true),
//                 new Tuple<string, bool>("Danmaarkaas1@gmail.com",true),
//                 new Tuple<string, bool>("Danmaarkaas2@gmail.com",false),
//                 new Tuple<string, bool>("Danmaarkaas3@gmail.com",false)
//             };
//             emails.ForEach(e =>
//             {
//                 if (userManager.FindByEmailAsync(e.Item1).Result==null)
//                 {
//                     IdentityUser user = new IdentityUser
//                     {
//                         UserName = e.Item1,
//                         Email = e.Item1,
//                     };
//
//                     IdentityResult result = userManager.CreateAsync(user, "ZaanseMayo").Result;
//
//                     if (e.Item2)
//                     {
//                        
//                     }
//                     else
//                     {
//                         
//                     }
//                 }       
//             });
//             
//         }   
//     }
// }