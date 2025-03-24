using BudgetMan.Data;
using BudgetMan.Modeles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetMan.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;

        public LoginModel(SignInManager<User> signInManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Veuillez écrire votre courriel")]
            [EmailAddress(ErrorMessage = "Veuillez saisir un courriel valide")]
            [DisplayName("Courriel")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Veuillez écrire votre mot de passe")]
            [DataType(DataType.Password)]
            [DisplayName("Mot de passe")]
            public string Password { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl)
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _context.Users.FirstOrDefaultAsync(a => a.Email == Input.Email);

            var result = await _signInManager.PasswordSignInAsync(
                user?.UserName ?? "", Input.Password, true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                TempData["Message"] = $"Bonjour {user?.UserName}";
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError("", "Courriel ou mot de passe incorrect.");
            }
            return Page();
        }
    }
}
