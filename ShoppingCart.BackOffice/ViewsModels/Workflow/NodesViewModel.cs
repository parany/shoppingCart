using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace ShoppingCart.BackOffice.ViewsModels.Workflow
{
    public class NodesViewModel
    {
        public XmlNodeList List { get; set; }
        public XmlNode CurrentNode { get; set; }

        public bool Start { get; set; }
    }
}