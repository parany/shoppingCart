using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using ShoppingCart.Models.Models.User;
using ShoppingCart.BackOffice.ViewsModels;
using ShoppingCart.CommonController.Infrastructure.Identity;
using System.Collections.ObjectModel;

namespace ShoppingCart.BackOffice.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public RoleController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public ApplicationUserManager UserManager {
            get {
                return this._userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        public ApplicationRoleManager RoleManager {
            get {
                return this._roleManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        // GET: Role
        public ActionResult Index()
        {
            IEnumerable<ApplicationRole> roles = RoleManager.Roles.ToList();
            var model = new Collection<RoleIndexModel>();

            foreach (var role in roles)
            {
                var users = UserManager.Users.Where(x => x.Roles.Select(r => r.RoleId).Contains(role.Id)).ToList();
                var usersCollection = new Collection<ApplicationUser>();

                foreach (var user in users)
                {
                    var userTemp = UserManager.FindById(user.Id);
                    usersCollection.Add(userTemp);
                }

                model.Add(new RoleIndexModel { Role = role, Users = usersCollection });
            }
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(RoleCreateModel roleCreateModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole role = new ApplicationRole(roleCreateModel.Name, roleCreateModel.Description);
                IdentityResult result = await RoleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else {
                    AddErrorsFromResult(result);
                }
            }
            return View(roleCreateModel);
        }

        public async Task<ActionResult> Edit(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();
            IEnumerable<ApplicationUser> members = UserManager.Users.Where(x => memberIDs.Any(y => y == x.Id));
            IEnumerable<ApplicationUser> nonMembers = UserManager.Users.Except(members);
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    result = await UserManager.AddToRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    result = await UserManager.RemoveFromRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                return RedirectToAction("Index");
            }
            return View("Error", new string[] { "Role Not Found" });
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else {
                    return View("Error", result.Errors);
                }
            }
            else {
                return View("Error", new string[] { "Role Not Found" });
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