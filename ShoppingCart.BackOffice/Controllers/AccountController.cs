using System.Web.Mvc;

using ShoppingCart.CommonController.Controllers;

namespace ShoppingCart.Controllers
{
    [Authorize(Roles = "AllPermissions")]
    public class AccountController : BasicAccountController
    {
    }
}