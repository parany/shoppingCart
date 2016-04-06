using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.Web.Mvc;
using ShoppingCart.Models.Models.Payments;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.BackOffice.ViewsModels.Provider;

namespace ShoppingCart.BackOffice.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProvidersController : Controller
    {
        private IGenericRepository<Provider> ProviderRepository { get; set; }

        public ProvidersController(IGenericRepository<Provider> providerRepository)
        {
            ProviderRepository = providerRepository;
        }

        // GET: Providers
        public ActionResult Index()
        {
            return View(ProviderRepository.GetAll());
        }

        // GET: Providers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = ProviderRepository.GetSingle(x => x.Id == id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        // GET: Providers/Create
        public ActionResult Create()
        {
            var payments = new Payments();
            string path = Server.MapPath("~/App_Data/payment.xml");
            payments.InitPaymentsList(path);
            ViewBag.PaymentMethods = payments.Modules;
            return View();
        }

        // POST: Providers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Address,PaymentMethods")] ProviderViewModel providerViewModel)
        {
            Provider provider = new Provider();
            if (ModelState.IsValid)
            {
                var sbPaymentMethods = new StringBuilder();
                int i = 1;
                foreach (var p in providerViewModel.PaymentMethods)
                {
                    sbPaymentMethods.Append(p);
                    if (i < providerViewModel.PaymentMethods.Count())
                    {
                        sbPaymentMethods.Append(',');
                    }
                    i++;
                }

                provider.Address = providerViewModel.Address;
                provider.Name = providerViewModel.Name;
                provider.PaymentMethods = sbPaymentMethods.ToString();

                ProviderRepository.Add(provider);
                return RedirectToAction("Index");
            }

            var payments = new Payments();
            string path = Server.MapPath("~/App_Data/payment.xml");
            payments.InitPaymentsList(path);
            ViewBag.PaymentMethods = payments.Modules;

            return View(provider);
        }

        // GET: Providers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = ProviderRepository.GetSingle(x => x.Id == id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        // POST: Providers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address")] Provider provider)
        {
            if (ModelState.IsValid)
            {
                ProviderRepository.Update(provider);
                return RedirectToAction("Index");
            }
            return View(provider);
        }

        // GET: Providers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = ProviderRepository.GetSingle(x => x.Id == id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Provider provider = ProviderRepository.GetSingle(x => x.Id == id);
            if (provider != null)
                ProviderRepository.Delete(provider);
            return RedirectToAction("Index");
        }

    }
}
