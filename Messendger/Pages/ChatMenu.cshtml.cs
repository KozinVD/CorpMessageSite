using Messendger.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Messendger.Pages
{
    [Authorize]
    public class ChatMenuModel : PageModel
    {
        public MessendgerDb db;
        public ChatMenuModel(MessendgerDb db)
        {
            this.db = db;

        }
        public UserInfo info { get; set; }
        public string IdUser { get; set; }
        
        public void OnGet()
        {
            IdUser = db.Users.Where(x => x.UserName == User.Identity.Name).First().Id;
            info = db.UserInfos.Find(IdUser);
        }
    }
}
