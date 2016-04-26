using ShoppingCart.CommonController.Controllers;
using System.Web.Mvc;


namespace ShoppingCart.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AccountController : BasicAccountController
    {
    }
}