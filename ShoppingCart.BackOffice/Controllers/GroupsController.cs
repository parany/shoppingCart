using ShoppingCart.BackOffice.ViewsModels;
using ShoppingCart.Models;
using ShoppingCart.Models.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.BackOffice.Controllers
{
    public class GroupsController : Controller
    {
        private ShoppingCartDbContext db = new ShoppingCartDbContext();

        // GET: Groups
        [Authorize(Roles = "AllPermissions, Read, ReadWrite")]
        public ActionResult Index()
        {
            return View(db.Groups.ToList());
        }

        [Authorize(Roles = "AllPermissions, Read, ReadWrite")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Create([Bind(Include = "Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Edit([Bind(Include = "Id,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            var idManager = new IdentityManager(db);
            idManager.DeleteGroup(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult GroupRoles(int id)
        {
            var group = db.Groups.Find(id);
            var model = new SelectGroupRolesViewModel(group);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        [ValidateAntiForgeryToken]
        public ActionResult GroupRoles(SelectGroupRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var idManager = new IdentityManager(db);
                var Db = new ShoppingCartDbContext();
                var group = Db.Groups.Find(model.GroupId);
                idManager.ClearGroupRoles(model.GroupId);
                // Add each selected role to this group:
                foreach (var role in model.Roles)
                {
                    if (role.Selected)
                    {
                        idManager.AddRoleToGroup(group.Id, role.RoleName);
                    }
                }
                return RedirectToAction("index");
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}