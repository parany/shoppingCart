using System.Web.Mvc;

using ShoppingCart.CommonController.Controllers;

namespace ShoppingCart.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AccountController : BasicAccountController
    {
    }
}