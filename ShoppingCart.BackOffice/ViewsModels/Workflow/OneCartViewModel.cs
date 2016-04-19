using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCart.CommonController.Tools;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.BackOffice.ViewsModels.Workflow
{
    public class OneCartViewModel
    {
        public Cart Cart { get; set; }
        public IList<CartProcessTree.NodeObject> Options { get; set; }
        public IList<CartProcessTree.NodeObject> Forms { get; set; }

        public string status { get; set; }
    }
}