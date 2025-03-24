using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetMan.Pages
{
    public class ProfilModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public ProfilModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }

}