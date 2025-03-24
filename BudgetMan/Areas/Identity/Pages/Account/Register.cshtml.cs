using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BudgetMan.Enums;
using BudgetMan.Modeles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetMan.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public RegisterModel(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();
        public class InputModel
        {
            [Required(ErrorMessage = "Nom d'utilisateur obligatoire")]
            [MaxLength(50, ErrorMessage = "Le nom d'utlisateur ne doit pas dépasser les {1} caractères")]
            [DisplayName("Nom d'utilisateur")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Veuillez saisir votre nom")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Veuillez saisir votre prénom")]
            [DisplayName("Prénom")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Veuillez saisir votre courriel")]
            [EmailAddress(ErrorMessage = "Veuillez saisir un courriel valide")]
            [DisplayName("Courriel")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Vous devez avoir un avatar")]
            public IFormFile AvatarUpload { get; set; }

            [Required(ErrorMessage = "Veuillez saisir votre pays")]
            public string Country { get; set; }

            [Required(ErrorMessage = "Le mot de passe est obligatoire")]
            [DataType(DataType.Password)]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit avoir au moins 6 caractères")]
            [DisplayName("Mot de passe")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Veuillez confirmer votre mot de passe.")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas.")]
            [Display(Name = "Confirmer le mot de passe")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Création user
            var user = new User
            {
                UserName = Input.UserName,
                LastName = Input.LastName,
                FirstName = Input.FirstName,
                Email = Input.Email,
                Country = Input.Country
            };

            //Gestion avatar
            var uploadPath = _configuration["Images:UploadPath"];
            if (string.IsNullOrEmpty(uploadPath))
            {
                ModelState.AddModelError("", "Erreur interne : le chemin pour le téléversement est mal configuré dans le appsettings.json.");
                return Page();
            }
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), uploadPath);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Input.AvatarUpload.FileName);
            var filePath = Path.Combine(fullPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Input.AvatarUpload.CopyToAsync(stream);
            }

            user.Avatar = fileName;

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Role.User.ToString());
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
