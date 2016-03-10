
using System.Collections;
using System.Collections.Generic;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.ViewModels
{
    public class CategoriesViewModel
    {
        public IList<Category> Categories { get; set; }
        public IList<Category> SelectedCategories { get; set; }

    }
}