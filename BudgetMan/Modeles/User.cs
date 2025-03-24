using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BudgetMan.Modeles
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Write your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Write your last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Write your country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "You need an avatar")]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only jpg, jpeg and png files are allowed")]
        public string Avatar { get; set; }
    }
}
