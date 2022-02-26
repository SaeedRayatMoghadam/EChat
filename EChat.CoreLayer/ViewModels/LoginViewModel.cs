using System.ComponentModel.DataAnnotations;

namespace EChat.CoreLayer.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Please Enter {0}")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter {0}")]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}