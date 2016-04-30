using ShoppingCart.Models.Models.Entities;
using System.Web.Mvc;
using System.ComponentModel;
using System.Collections.Generic;

namespace ShoppingCart.BackOffice.ViewsModels
{
    public class CreateViewModels
    {
        public Product Product { get; set; }

        [DisplayName("Category")]
        public SelectList CategoryList { get; set; }

        public string[] Providers { get; set; }
    }
}