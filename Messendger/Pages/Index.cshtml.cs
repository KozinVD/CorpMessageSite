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
            //Авторизация пользователя
            try
            {
                if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Password))
                    return Redirect("/");
                //Проверка введных данных пользователя
                var result = await signManager.PasswordSignInAsync(input.Email, input.Password, false, false);
                if (result.Succeeded)
                    //Если данные верны переадрисация на главную страницу
                    return Redirect("/ChatMenu");
                Console.WriteLine("Неудача!");
                Console.WriteLine(result);
                //Если данные неверны переадрисация на страницу входа
                return Redirect("/");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
    }
}
