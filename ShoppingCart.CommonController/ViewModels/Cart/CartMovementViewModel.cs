using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace ShoppingCart.CommonController.ViewModels.Cart
{
    public class CartMovementViewModel
    {
        public Models.Models.Entities.Cart Cart { get; set; }
        public XmlNodeList OptopnsList { get; set; }
    }
}