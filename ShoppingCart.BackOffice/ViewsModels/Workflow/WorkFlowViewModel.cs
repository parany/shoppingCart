using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCart.CommonController.Tools;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.BackOffice.ViewsModels.Workflow
{
    public class WorkFlowViewModel
    {
        public Cart Cart { get; set; }
        public IList<CartProcessTree.NodeObject> Options { get; set; }

        public string status { get; set; }
        public IList<string> Evolution { get; set; }
    }
}