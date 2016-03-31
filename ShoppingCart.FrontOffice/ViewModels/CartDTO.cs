using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Models.Payments;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShoppingCart.ViewModels
{
    public class CartDTO
    {
        public CartViewModel Cart { get; set; }

        [Required]
        [DisplayName("Ship to")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public string ErrorMessage { get; set; }

        public string PaymentsMethod { get; set; }

        public Payments Payments { get; set; }
    }
}