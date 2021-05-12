using System.Linq;
using System.Threading.Tasks;
using Business;
using Microsoft.AspNetCore.Identity;

namespace DataAccess
{
    public class TestData
    {
        public static async Task InsertData(ElearningCatalogContext context, UserManager<User> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new User
                {
                    FullName = "Vladimir Paniagua",
                    UserName = "Vladi",
                    Email = "vladimir_paniagua@hotmail.com" 
                };
                await userManager.CreateAsync(user,"Password 123");
            }
        }
    }
}