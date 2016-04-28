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
            string path = Server.MapPath("~/App_Data/payment.xml");
            payments.InitPaymentsList(path);
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
            Provider provider = ProviderRepository.GetSingle(x => x.Id == id);
            var providerViewModel = new ProviderViewModel();
            providerViewModel.Id = provider.Id;
            providerViewModel.Address = provider.Address;
            providerViewModel.Name = provider.Name;
            if (provider.PaymentMethods != null)
                providerViewModel.PaymentMethods = provider.PaymentMethods.Split(',');
            ViewBag.PaymentMethods = GetPaymentMethods();
            if (provider == null)
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
                Provider provider = new Provider();
                var sbPaymentMethods = new StringBuilder();
                int i = 1;
                if (providerViewModel.PaymentMethods != null)
                {
                    foreach (var p in providerViewModel.PaymentMethods)
                    {
                        sbPaymentMethods.Append(p);
                        if (i < providerViewModel.PaymentMethods.Count())
                        {
                            sbPaymentMethods.Append(',');
                        }
                        i++;
                    }
                }

                provider.Id = providerViewModel.Id;
                provider.Address = providerViewModel.Address;
                provider.Name = providerViewModel.Name;
                provider.PaymentMethods = sbPaymentMethods.ToString();

                ProviderRepository.Update(provider);
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
