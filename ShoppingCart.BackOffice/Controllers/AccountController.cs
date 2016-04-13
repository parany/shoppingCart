using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ShoppingCart.CommonController.Controllers;
using ShoppingCart.CommonController.Infrastructure.Identity;
using ShoppingCart.CommonController.ViewModels;
using ShoppingCart.Models.Models.User;
using ShoppingCart.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AccountController : BasicAccountController
    {
    }
}