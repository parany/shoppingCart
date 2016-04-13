using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ShoppingCart.BackOffice.ViewsModels;
using ShoppingCart.CommonController.Infrastructure.Identity;
using ShoppingCart.Models.Models.User;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.BackOffice.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {

        private ApplicationUserManager UserManager { get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); } }

        // GET: User
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber
                };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
                
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<ActionResult> Edit(string id, string username, string email, string password, string address, string phonenumber)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                user.UserName = username;
                user.Address = address;
                user.PhoneNumber = phonenumber;
                IdentityResult validUser = await UserManager.UserValidator.ValidateAsync(user);
                if (!validUser.Succeeded)
                {
                    AddErrorsFromResult(validUser);
                }
                IdentityResult validPass = null;
                if (password != string.Empty)
                {
                    validPass = await UserManager.PasswordValidator.ValidateAsync(password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash =
                        UserManager.PasswordHasher.HashPassword(password);
                    }
                    else {
                        AddErrorsFromResult(validPass);
                    }
                }
                if ((validUser.Succeeded && validPass == null) || (validUser.Succeeded
                && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else {
                    return View("Error", result.Errors);
                }
            }
            else {
                return View("Error", new string[] { "User Not Found" });
            }
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}