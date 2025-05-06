using Messendger.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Messendger.Pages
{

    public class RegistrationModel : PageModel
    {
        public MessendgerDb db { get; set; }

        UserManager<User> userManager;
        public RegistrationModel(UserManager<User> _userManager, MessendgerDb db )
        {
            userManager = _userManager;
            this.db = db;
        }
        [BindProperty]
        public Input input { get; set; }
        public class Input
        {
            public string Login { get; set; }
            public string Password { get; set; }
            public string Surname { get; set; }
            public string Name { get; set; }
            public string Lastname { get; set; }
            public DateOnly Birthday { get; set; }
            public string IdJob { get; set; }
        }
        public async Task<ActionResult> OnPostAsync()
        {
            try
            {
                var newUser = new User() { Email = input.Login, LastLoginDate = DateOnly.FromDateTime(DateTime.Now), UserName = input.Login };
                var result = await userManager.CreateAsync(newUser, input.Password);
                if (result.Succeeded)
                {
                    var newInfo = new UserInfo();
                    newInfo.Surname = input.Surname;
                    newInfo.Name = input.Name;
                    newInfo.Lastname = input.Lastname;
                    newInfo.Birthday = input.Birthday;
                    newInfo.IdJob = int.Parse(input.IdJob);
                    newInfo.Id = db.Users.Where(x => x.Email == input.Login).First().Id;
                    db.UserInfos.Add(newInfo);
                    db.SaveChanges();
                    return Redirect("/");
                }
                string er = "";
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
                return Redirect($"/Registration/");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return NotFound();
            }
        }
        [BindProperty]
        [Required]
        public string NameJob { get; set; }
        public ActionResult OnPostAddSpecialty()
        {
            if (string.IsNullOrWhiteSpace(NameJob))
                return Redirect("/Registration");
            db.Jobs.Add(new Job() { Name =  NameJob });
            db.SaveChanges();
            return Redirect("/Registration");
        }
    }
}
