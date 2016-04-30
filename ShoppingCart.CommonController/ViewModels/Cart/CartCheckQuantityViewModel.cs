using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.CommonController.ViewModels
{
    public class CartCheckQuantityViewModel
    {
        public Product Product { get; set; }
        public string ReturnUrl { get; set; }
        public int QtyOrdered { get; set; }
        public int QtyRemaining { get; set; }
    }
}