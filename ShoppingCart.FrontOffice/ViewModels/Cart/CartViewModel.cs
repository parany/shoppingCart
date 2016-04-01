using ShoppingCart.Models.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.ViewModels
{
    public class CartViewModel
    {
        private List<CartLineViewModel> lineCollection = new List<CartLineViewModel>();
        public void AddItem(Product product, int quantity)
        {
            CartLineViewModel line = lineCollection
            .Where(p => p.Product.Id == product.Id)
            .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLineViewModel
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else {
                line.Quantity += quantity;
            }
        }
        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(l => l.Product.Id == product.Id);
        }
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Product.Price * e.Quantity);
        }
        public void Clear()
        {
            lineCollection.Clear();
        }
        public int GetQuantity(Product product)
        {
            CartLineViewModel clvm = lineCollection.Find(lc => lc.Product.Id == product.Id);
            if(clvm == null)
            {
                return 0;
            }
            return clvm.Quantity;
        }
        public IEnumerable<CartLineViewModel> Lines
        {
            get { return lineCollection; }
        }

    }

    public class CartLineViewModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}