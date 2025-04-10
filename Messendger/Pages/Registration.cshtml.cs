using Messendger.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Messendger.Pages
{
    [IgnoreAntiforgeryToken]
    public class RegistrationModel : PageModel
    {
        UserManager<User> userManager;
        public RegistrationModel(UserManager<User> _userManager )
        {
            userManager = _userManager;
        }
        [BindProperty]
        public Input input { get; set; }
        public class Input
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }
        public async Task<ActionResult> OnPostAsync()
        {
            try
            {
                var newUser = new User() { Email = input.Login, LastLoginDate = DateOnly.FromDateTime(DateTime.Now), UserName = input.Login };
                var result = await userManager.CreateAsync(newUser, input.Password);
                if (result.Succeeded)
                {
                    return Redirect("/");
                }
                Console.WriteLine(input.Password);
                foreach (var i in  result.Errors)
                {
                   
                    Console.WriteLine(i.Description);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return NotFound();
            }
        }
    }
}
