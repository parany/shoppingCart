using ShoppingCart.Models.Models.User;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.CommonController.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(15, MinimumLength=3)]
        [Display(Name="Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        [Required]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public ApplicationUser GetUser()
        {
            var user = new ApplicationUser()
            {
                UserName = this.UserName,
                Address = this.Address,
                PhoneNumber = this.PhoneNumber,
                Email = this.Email,
            };
            return user;
        }
    }
}