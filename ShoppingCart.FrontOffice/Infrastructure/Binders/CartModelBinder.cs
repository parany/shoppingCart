﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            
            CartViewModel cart = null;
            if (controllerContext.HttpContext.Session != null)
            {
                cart = (CartViewModel)controllerContext.HttpContext.Session[sessionKey];
            }
            
            if (cart == null)
            {
                cart = new CartViewModel();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }
            
            return cart;
        }

        public static void ResetBinding(ControllerContext controllerContext)
        {
            controllerContext.HttpContext.Session[sessionKey] = null;
        }
    }
}