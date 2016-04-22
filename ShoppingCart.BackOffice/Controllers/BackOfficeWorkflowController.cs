using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ShoppingCart.BackOffice.ViewsModels.Workflow;
using ShoppingCart.CommonController.Tools;
using ShoppingCart.Controllers;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;

namespace ShoppingCart.BackOffice.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BackOfficeWorkflowController : CartController
    {


        public BackOfficeWorkflowController(IGenericRepository<Cart> cartRepository,
                              IGenericRepository<Product> productRepository,
                              IGenericRepository<ShippingDetail> shippingDetailRepository) : base(cartRepository, productRepository, shippingDetailRepository)
        {
            
        }

        public ActionResult CartPositionAndPossibilities()
        {
            string workflowXmlPath = Server.MapPath("~/App_Data/workflow.xml");

            IList<Cart> carts = CartRepository.GetAll();
            IList<SampleViewModel.BoxContent> list = new List<SampleViewModel.BoxContent>();
            foreach (Cart c in carts)
            {
                CartProcessTree _Xml = new CartProcessTree(workflowXmlPath);

                list.Add(new SampleViewModel.BoxContent()
                {
                    Cart = c,
                    Descriptions = _Xml.Descriptions(c.WorkflowStatus),
                    Options = _Xml.ForwardOptions(c.WorkflowStatus),
                });
            }

            SampleViewModel model = new SampleViewModel()
            {
                boxes = list
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
                status = _Xml.CurrentTreeState(cart.WorkflowStatus),
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

            return RedirectToAction("CartPositionAndPossibilities", "BackOfficeWorkflow");
        }




    }
}