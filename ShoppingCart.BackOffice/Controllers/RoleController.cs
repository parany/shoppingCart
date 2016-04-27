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
using ShoppingCart.Models;
using System.Net;
using System.Data.Entity;

namespace ShoppingCart.BackOffice.Controllers
{
    public class RoleController : Controller
    {
        private ShoppingCartDbContext _db = new ShoppingCartDbContext();

        // GET: Role
        [Authorize(Roles = "AllPermissions, Read, ReadWrite")]
        public ActionResult Index()
        {
            var rolesList = new List<RoleViewModel>();
            foreach (var role in _db.Roles)
            {
                var roleModel = new RoleViewModel(role);
                rolesList.Add(roleModel);
            }
            return View(rolesList);
        }

        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Create(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Create([Bind(Include =
            "RoleName,Description")]RoleViewModel model)
        {
            string message = "That role name has already been used";
            if (ModelState.IsValid)
            {
                var role = new ApplicationRole(model.RoleName, model.Description);
                var idManager = new IdentityManager(_db);

                if (idManager.RoleExists(model.RoleName))
                {
                    return View(message);
                }
                else
                {
                    idManager.CreateRole(model.RoleName, model.Description);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Edit(string id)
        {
            // It's actually the Role.Name tucked into the id param:
            var role = _db.Roles.First(r => r.Name == id);
            var roleModel = new EditRoleViewModel(role);
            return View(roleModel);
        }


        [HttpPost]
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Edit([Bind(Include =
            "RoleName,OriginalRoleName,Description")] EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = (ApplicationRole)_db.Roles.First(r => r.Name == model.OriginalRoleName);
                role.Name = model.RoleName;
                role.Description = model.Description;
                _db.Entry(role).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = _db.Roles.First(r => r.Name == id);
            var model = new RoleViewModel(role);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult DeleteConfirmed(string id)
        {
            var role = _db.Roles.First(r => r.Name == id);
            var idManager = new IdentityManager(_db);
            idManager.DeleteRole(role.Id);
            return RedirectToAction("Index");
        }
    }
}