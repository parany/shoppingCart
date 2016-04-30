using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using ShoppingCart.BackOffice.ViewsModels;
using ShoppingCart.CommonController.Infrastructure.Identity;
using ShoppingCart.Models.Models.User;
using ShoppingCart.CommonController.ViewModels;
using ShoppingCart.Models;
using System.Collections.Generic;
using static ShoppingCart.CommonController.Controllers.BasicManageController;
using System.Linq;

namespace ShoppingCart.BackOffice.Controllers
{
    public class UserController : Controller
    {
        ShoppingCartDbContext Db = new ShoppingCartDbContext();

        private ApplicationUserManager UserManager
        {
            get {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        // GET: User
        [Authorize(Roles = "AllPermissions, Read, ReadWrite")]
        public ActionResult Index()
        {
            var users = Db.Users;
            var model = new List<EditUserViewModel>();
            foreach (var user in users)
            {
                var u = new EditUserViewModel(user);
                model.Add(u);
            }
            return View(model);
        }

        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = model.GetUser();
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

        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Edit(string id, ManageMessageId? Message = null)
        {
            var user = Db.Users.First(u => u.UserName == id);
            var model = new EditUserViewModel(user);
            ViewBag.MessageId = Message;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Db.Users.First(u => u.UserName == model.UserName);
                // Update the user data:
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
                user.Email = model.Email;
                Db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Delete(string id = null)
        {
            var user = Db.Users.First(u => u.UserName == id);
            var model = new EditUserViewModel(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = Db.Users.First(u => u.UserName == id);
            Db.Users.Remove(user);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult UserGroups(string id)
        {
            var user = Db.Users.First(u => u.UserName == id);
            var model = new SelectUserGroupsViewModel(user);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult UserGroups(SelectUserGroupsViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var idManager = new IdentityManager(Db);
                var user = Db.Users.First(u => u.UserName == model.UserName);
                idManager.ClearUserGroups(user.Id);
                foreach (var group in model.Groups)
                {
                    if (group.Selected)
                    {
                        idManager.AddUserToGroup(user.Id, group.GroupId);
                    }
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize(Roles = "AllPermissions, ReadWrite, Read")]
        public ActionResult UserPermissions(string id)
        {
            var user = Db.Users.First(u => u.UserName == id);
            var model = new UserPermissionsViewModel(user);
            return View(model);
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