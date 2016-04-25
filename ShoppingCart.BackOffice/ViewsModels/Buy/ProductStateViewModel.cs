using ShoppingCart.Models.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.BackOffice.ViewsModels.Buy
{
    public enum ProductState
    {
        inStock = 0,
        New = 1
    };

    public class ProductStateViewModel
    {
        public Product Product { get; set; }
        public ProductState State { get; set; }

        public void ChangeProductState(IList<Product> productsInStock)
        {
            Product SameValue = productsInStock.FirstOrDefault(p => p.ProductReference == this.Product.ProductReference);

            if (SameValue != null)
                State = ProductState.inStock;
            else
                State = ProductState.New;
        }
    }
}