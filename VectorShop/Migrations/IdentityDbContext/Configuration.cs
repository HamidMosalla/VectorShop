using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VectorShop.Models;

namespace VectorShop.Migrations.IdentityDbContext
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<VectorShop.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Migrations\IdentityDbContext";
            ContextKey = "VectorShop.Models.ApplicationDbContext";
        }

        protected override void Seed(VectorShop.Models.ApplicationDbContext context)
        {

            if (!context.Users.Any(u => u.UserName == "ghamran@gmail.com"))
            {

                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = new ApplicationUser
                { UserName = "ghamran@gmail.com", EmailConfirmed = true, LockoutEnabled = false };
                userManager.Create(user, "1@3$Su8#GmQ4");

                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                roleManager.Create(new IdentityRole { Name = "admin" });
                userManager.AddToRole(user.Id, "admin");

            }



        }
    }
}
