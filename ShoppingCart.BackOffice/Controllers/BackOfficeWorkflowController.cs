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
    public class BackOfficeWorkflowController : BasicCartsController
    {
        
        public BackOfficeWorkflowController(IGenericRepository<Product> productRepository,
                               IGenericRepository<Cart> cartRepository,
                               IGenericRepository<ShippingDetail> shipRepository,
                               IGenericRepository<CartLine> cartlineRepository) : base(productRepository, cartRepository, shipRepository, cartlineRepository)
        {
            
        }
        

        public ActionResult CartPositionAndPossibilities()
        {
            
            IList<Cart> carts = _CartRepository.GetAll();
            IList<SampleViewModel.BoxContent> list = new List<SampleViewModel.BoxContent>();
            foreach (Cart c in carts)
            {
                CartProcessTree _Xml = new CartProcessTree("e:/shop/workflow.xml");

                list.Add(new SampleViewModel.BoxContent()
                {
                    Cart = c,
                    Descriptions = _Xml.Descriptions(c.WorkflowStatus),
                    Options = _Xml.ForwardOptions(c.WorkflowStatus)
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
            Cart carts = _CartRepository.GetSingle(c => c.Id == Id);
            carts.WorkflowStatus = carts.WorkflowStatus + "/Options/" + newState;
            _CartRepository.Update(carts);
            return RedirectToAction("OneCartMove", new RouteValueDictionary(
    new { controller = "BackOfficeWorkflow", action = "OneCartMove", id = id }));
        }

        public ActionResult OneCartMove(string id)
        {
            Guid Id = new Guid(id);
            Cart cart = _CartRepository.GetSingle(c => c.Id == Id);
            CartProcessTree _Xml = new CartProcessTree("e:/shop/workflow.xml");

            OneCartViewModel model = new OneCartViewModel()
            {
                Cart = cart,
                Forms = _Xml.Descriptions(cart.WorkflowStatus),
                Options = _Xml.ForwardOptions(cart.WorkflowStatus),
                status = _Xml.CurrentTreeState(cart.WorkflowStatus)
            };

            return View(model);
        }

        public ActionResult Reset(string id)
        {
            Guid Id = new Guid(id);
            Cart carts = _CartRepository.GetSingle(c => c.Id == Id);
            carts.WorkflowStatus = carts.TransactionType.ToString();
            _CartRepository.Update(carts);
            return RedirectToAction("OneCartMove", new RouteValueDictionary(
                new
                {
                    controller = "BackOfficeWorkflow", action = "OneCartMove", id = id
                }));
        }

        public ActionResult Drop(string id)
        {
            Guid Id = new Guid(id);
            Cart cart = _CartRepository.GetSingle(c => c.Id == Id);
            _CartRepository.Delete(cart);

            return RedirectToAction("CartPositionAndPossibilities", "BackOfficeWorkflow");
        }




    }
}