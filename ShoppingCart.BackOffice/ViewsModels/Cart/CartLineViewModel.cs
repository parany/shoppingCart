using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.BackOffice.ViewsModels
{
    public class CartLineViewModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}