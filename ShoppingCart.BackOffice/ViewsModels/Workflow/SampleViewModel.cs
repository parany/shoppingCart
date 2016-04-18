using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCart.CommonController.Tools;
using ShoppingCart.Models.Models.Entities;

namespace ShoppingCart.BackOffice.ViewsModels.Workflow
{
    public class SampleViewModel
    {
        public class BoxContent
        {
            public Cart Cart { get; set; }
            public IList<CartProcessTree.NodeObject> Descriptions { get; set; }
            public IList<CartProcessTree.NodeObject> Options { get; set; }
        }

        public IList<BoxContent> boxes { get; set; }
    }
}