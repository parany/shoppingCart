using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.ViewModels
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}