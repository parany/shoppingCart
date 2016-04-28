using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

using ShoppingCart.Models.Models.Payments;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.ViewModels;
using ShoppingCart.Models.Repositories.Interface;
using ShoppingCart.Services.Interface;
using ShoppingCart.GeneralLib.Util;

namespace ShoppingCart.BackOffice.Controllers
{
    public class ProvidersController : Controller
    {
        private IGenericRepository<Provider> ProviderRepository { get; set; }
        private IProvidersService ProvidersService { get; set; }

        public ProvidersController(IGenericRepository<Provider> providerRepository,
                                   IProvidersService providersService)
        {
            ProviderRepository = providerRepository;
            ProvidersService = providersService;
        }

        private List<Payment> GetPaymentMethods()
        {
            var payments = new Payments();
            payments.InitPaymentsListFromResourceString(ResourcesHelper.payment);
            return payments.Modules;
        }

        // GET: Providers
        [Authorize(Roles = "AllPermissions, Read, ReadWrite")]
        public ActionResult Index()
        {
            return View(ProviderRepository.GetAll());
        }

        // GET: Providers/Details/5
        [Authorize(Roles = "AllPermissions, Read, ReadWrite")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var providerViewModel = ProvidersService.GetDetails(id);
            ViewBag.PaymentMethods = GetPaymentMethods();
            if (providerViewModel == null)
            {
                return HttpNotFound();
            }
            return View(providerViewModel);
        }

        // GET: Providers/Create
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Create()
        {
            ViewBag.PaymentMethods = GetPaymentMethods();
            return View();
        }

        // POST: Providers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Create([Bind(Include = "Name,Address,PaymentMethods")] ProviderViewModel providerViewModel)
        {
            Provider provider = new Provider();
            if (ModelState.IsValid)
            {
                ProvidersService.AddProvider(providerViewModel);
                return RedirectToAction("Index");
            }
            
            ViewBag.PaymentMethods = GetPaymentMethods();

            return View(provider);
        }

        // GET: Providers/Edit/5
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProviderEditViewModel provider = ProvidersService.EditProvider(id);
            ViewBag.PaymentMethods = provider.PaymentMethods;
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
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,PaymentMethods")] ProviderViewModel providerViewModel)
        {
            if (ModelState.IsValid)
            {
                ProvidersService.UpdateProvider(providerViewModel);
                return RedirectToAction("Index");
            }
            return View(providerViewModel);
        }

        // GET: Providers/Delete/5
        [Authorize(Roles = "AllPermissions, ReadWrite")]
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
        [Authorize(Roles = "AllPermissions, ReadWrite")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Provider provider = ProviderRepository.GetSingle(x => x.Id == id);
            if (provider != null)
                ProviderRepository.Delete(provider);
            return RedirectToAction("Index");
        }

    }
}
