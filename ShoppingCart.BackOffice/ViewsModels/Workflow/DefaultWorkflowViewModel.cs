using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Microsoft.Ajax.Utilities;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.BackOffice.ViewsModels.Workflow
{
    public class DefaultWorkflowViewModel
    {
        public IList<Cart> carts { get; set; } 
    }
}