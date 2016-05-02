using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using ShoppingCart.BackOffice.ViewsModels.Workflow;
using ShoppingCart.CommonController.Tools;
using ShoppingCart.Controllers;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;

namespace ShoppingCart.BackOffice.Controllers
{
    [Authorize(Roles = "AllPermissions")]
    public class BackOfficeWorkflowController : CartController
    {


        public BackOfficeWorkflowController(IGenericRepository<Cart> cartRepository,
                              IGenericRepository<Product> productRepository,
                              IGenericRepository<ShippingDetail> shippingDetailRepository) : base(cartRepository, productRepository, shippingDetailRepository)
        {    
        }

        /*
         * USE TREE
         *
         *
         */

        public ActionResult ShowAllCart()
        {
            string workflowXmlPath = Server.MapPath("~/App_Data/workflow.xml");

            IList<Cart> carts = CartRepository.GetAll();
            
            IList<CartsViewModel.CartWorkViewModel> list = new List<CartsViewModel.CartWorkViewModel>();

            foreach (Cart c in carts)
            {
                CartProcessTree _Xml = new CartProcessTree(workflowXmlPath);

                // Check if the Worflow is initialised
                string status = c.WorkflowStatus;
                if (status == null)
                {
                    status = c.TransactionType.ToString();
                    c.WorkflowStatus = status;
                    CartRepository.Update(c);
                }

                list.Add(new CartsViewModel.CartWorkViewModel
                {
                    Cart = c,
                    User = c.User,
                    Status = _Xml.CurrentPotitionOnTree(status)
                });
            }

            CartsViewModel model = new CartsViewModel()
            {
                Carts = list
            };

            return View(model);
        }
        
        public ActionResult MoveState(string newState, string id)
        {
            Guid Id = new Guid(id);
            Cart carts = CartRepository.GetSingle(c => c.Id == Id);
            carts.WorkflowStatus = carts.WorkflowStatus + "/Options/" + newState;
            CartRepository.Update(carts);
            return RedirectToAction("OneCartMove", new RouteValueDictionary(
                                    new { controller = "BackOfficeWorkflow", action = "OneCartMove", id = id }));
        }

        public ActionResult OneCartMove(string id)
        {
            string workflowXmlPath = Server.MapPath("~/App_Data/workflow.xml");

            Guid Id = new Guid(id);
            Cart cart = CartRepository.GetSingle(c => c.Id == Id);
            CartProcessTree _Xml = new CartProcessTree(workflowXmlPath);

            var varLines = new List<OneCartViewModel.CartLineViewModel>();
            foreach (var cartLine in cart.CartLines)
            {
                varLines.Add(new OneCartViewModel.CartLineViewModel
                {
                    Product = ProductRepository.GetSingle(x => x.Id == cartLine.ProductId),
                    Quantity = cartLine.Quantity,
                    Total = ProductRepository.GetSingle(x => x.Id == cartLine.ProductId).Price * cartLine.Quantity
                });
            }

            OneCartViewModel model = new OneCartViewModel()
            {
                Cart = cart,
                Forms = _Xml.Descriptions(cart.WorkflowStatus),
                Options = _Xml.ForwardOptions(cart.WorkflowStatus),
                status = _Xml.CurrentPotitionOnTree(cart.WorkflowStatus),
                User = cart.User,
                CartLines = varLines
            };

            return View(model);
        }

        public ActionResult Reset(string id)
        {
            Guid Id = new Guid(id);
            Cart carts = CartRepository.GetSingle(c => c.Id == Id);
            carts.WorkflowStatus = carts.TransactionType.ToString();
            CartRepository.Update(carts);
            return RedirectToAction("OneCartMove", new RouteValueDictionary(
                new
                {
                    controller = "BackOfficeWorkflow", action = "OneCartMove", id = id
                }));
        }

        public ActionResult Drop(string id)
        {
            Guid Id = new Guid(id);
            Cart cart = CartRepository.GetSingle(c => c.Id == Id);
            CartRepository.Delete(cart);

            return RedirectToAction("ShowAllCart", "BackOfficeWorkflow");
        }

        // ================================== PARTIALS ===================== //

        public ActionResult SelectCart(string id)
        {
            string workflowXmlPath = Server.MapPath("~/App_Data/workflow.xml");

            Guid Id = new Guid(id);
            Cart cart = CartRepository.GetSingle(c => c.Id == Id);
            CartProcessTree _Xml = new CartProcessTree(workflowXmlPath);
            XmlNode currentPosition = _Xml.DirectPathNode(cart.WorkflowStatus);
            string currentControl = currentPosition.SelectSingleNode("./Redirection/Control").Value;
            string currentAction = currentPosition.SelectSingleNode("./Redirection/Action").Value;
            return RedirectToAction(currentAction, new RouteValueDictionary(
                                    new { controller = currentControl, action = currentAction, id = cart.Id }));
        }


        // ================================= END PARTIALS ================ //

        
        // ================================ TESTING PARTIALS ============ //
        public void ChangeShippingState(string id, string state)
        {
            Guid Id = new Guid(id);
            Cart cart = CartRepository.GetSingle(c => c.Id == Id);
            switch (state)
            {
                case "Pending":
                    cart.State = ShippingState.Pending;
                    break;
                case "Cancelled":
                    cart.State = ShippingState.Canceled;
                    break;
                case "Delivered":
                    cart.State = ShippingState.Delivered;
                    break;
            }
            CartRepository.Update(cart);
        }


        // =============================== END TEST =================== //

        /*
         * Create Nodes TREE
         *
         *
         */

        public ActionResult ShowTreeBase()
        {
            string workflowXmlPath = Server.MapPath("~/App_Data/workflow.xml");
            CartProcessTree _Xml = new CartProcessTree(workflowXmlPath);
            NodesViewModel model = new NodesViewModel
            {
                List = _Xml.Heads(),
                CurrentNode = _Xml.StartNode(),
                Start = true
            };
            return View("TreeState",model);
        }
        [HttpPost]
        public ActionResult ChangeNodeName(string path, string name, string oldName)
        {
            string workflowXmlPath = Server.MapPath("~/App_Data/workflow.xml");
            CartProcessTree _Xml = new CartProcessTree(workflowXmlPath);
            string newPath = path.Replace(oldName, "");
            _Xml.ChangeNodeName(path, name);
            return RedirectToAction("TreeState", new RouteValueDictionary(
                                    new { controller = "BackOfficeWorkflow", action = "TreeState", path = newPath }));
        }
        public ActionResult DeleteBranch(string path, string branchName)
        {
            string workflowXmlPath = Server.MapPath("~/App_Data/workflow.xml");
            CartProcessTree _Xml = new CartProcessTree(workflowXmlPath);
            string newPath = path.Replace(branchName, "");
            _Xml.DeleteBranch(path);
            return RedirectToAction("TreeState", new RouteValueDictionary(
                                    new { controller = "BackOfficeWorkflow", action = "TreeState", path = newPath }));
        }
        public ActionResult ChangeNodeValue()
        {
            return Json("");
        }

        public ActionResult TreeState(string path = null)
        {
            string workflowXmlPath = Server.MapPath("~/App_Data/workflow.xml");
            CartProcessTree _Xml = new CartProcessTree(workflowXmlPath);
            if (!_Xml.DirectPathNode(path).Name.Equals(_Xml.StartNode().Name) && path != null)
            {
                NodesViewModel model = new NodesViewModel
                {
                    List = _Xml.DirectPathNode(path).ChildNodes,
                    CurrentNode = _Xml.DirectPathNode(path),
                    Start = false
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("ShowTreeBase", new RouteValueDictionary(
                                    new { controller = "BackOfficeWorkflow", action = "ShowTreeBase" }));
            }
        }
    }
}