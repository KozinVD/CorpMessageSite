using Messendger.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Messendger.Pages
{
    public class IndexModel : PageModel
    {
        SignInManager<User> signManager {  get; set; }
        public IndexModel (SignInManager<User> manager)
        {
            this.signManager = manager;
        }
        [BindProperty]
        public Input input { get; set; }
        public class Input
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public async Task<ActionResult> OnPostAsync()
        {
            var result = await signManager.PasswordSignInAsync(input.Email, input.Password, false, false);
            if (result.Succeeded)
            return Redirect("/ChatMenu");
            Console.WriteLine("Неудача!");
            Console.WriteLine(result);
            return Redirect("/");
        }
        public ActionResult OnGetReg()
        {
            return Redirect("/Registration");
        }
    }
}
