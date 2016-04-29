using ShoppingCart.CommonController.Controllers;
using System.Web.Mvc;


namespace ShoppingCart.Controllers
{
    [Authorize(Roles = "AllPermissions")]
    public class AccountController : BasicAccountController
    {
    }
}