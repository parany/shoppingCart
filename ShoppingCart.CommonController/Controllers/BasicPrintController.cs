using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Generator;

namespace ShoppingCart.CommonController.Controllers
{
    public class BasicPrintController
    {
        [HttpPost]
        public void Generate(string filename)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachement; filename=" + filename);
        }
    }
}