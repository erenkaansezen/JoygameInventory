using JoygameInventory.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace JoygameInventory.Data.Context
{
    public class IdentitySeedData
    {
        private const string adminUser = "admin";
        private const string adminPassword = "Admin_123";

        public static async void IdentityTestUser(IApplicationBuilder app)
        {
            //var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<StoreContext>();
            //if (context.Database.GetAppliedMigrations().Any())
            //{
            //    context.Database.Migrate();
            //}

            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<JoyUser>>();

            var user = await userManager.FindByNameAsync(adminUser);


            if (user == null)
            {
                user = new JoyUser
                {
                    FirstName = "Administrator",
                    UserName = adminUser,
                    Email = "admin@store.com",
                    PhoneNumber = "1234567890",
                    

                };
                await userManager.CreateAsync(user, adminPassword);

            }
        }
    }
}
