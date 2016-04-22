using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ShoppingCart.CommonController.Tools;
using ShoppingCart.Controllers;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;

namespace ShoppingCart.CommonController.Controllers
{
    public class BasicWorkflowController : BasicCartsController
    {
        private XmlHandler _xmlHandler;

        public BasicWorkflowController(IGenericRepository<Product> productRepository,
                               IGenericRepository<Cart> cartRepository,
                               IGenericRepository<ShippingDetail> shipRepository,
                               IGenericRepository<CartLine> cartlineRepository) : base(productRepository, cartRepository, shipRepository, cartlineRepository)

        {
            _xmlHandler = new XmlHandler("workflow.xml");
        }

        public ActionResult AllCarts()
        {
            
            return View();
        }

        public void InitializeWorkflow(Cart cart)
        {
            cart.WorkflowStatus = cart.TransactionType.ToString();
        }

        public ActionResult MoveForward(Cart cart)
        {
            XmlNode node = _xmlHandler.DirectPathNode(cart.WorkflowStatus);
            XmlNodeList child = null;
            if (node.HasChildNodes)
            {
                child = node.ChildNodes;
            }
            return Json(child);
        }
    }
}