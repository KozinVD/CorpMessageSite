using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Messendger.Pages
{
    [Authorize]
    public class ChatMenuModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
