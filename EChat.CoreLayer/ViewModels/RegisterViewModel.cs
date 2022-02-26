using System.ComponentModel.DataAnnotations;

namespace EChat.CoreLayer.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please Enter {0}")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter {0}")]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Please Enter {0}")]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password),ErrorMessage = "Passwords does NOT match")]
        public string ConfirmPassword { get; set; }
    }
}