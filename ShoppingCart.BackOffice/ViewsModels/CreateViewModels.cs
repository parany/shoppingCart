using System;
using ShoppingCart.Models.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;

namespace ShoppingCart.BackOffice.ViewsModels
{
    public class CreateViewModels
    {
        public Product Product { get; set; }

        [DisplayName("Category")]
        public SelectList CategoryList { get; set; }
    }
}