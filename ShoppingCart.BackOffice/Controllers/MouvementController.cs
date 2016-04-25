using ShoppingCart.Models.Log;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.BackOffice.Controllers
{
    public class MouvementController : Controller
    {
        private IGenericRepository<ChangeTracking> _ChangeTrackingRepository { get; }

        public MouvementController(IGenericRepository<ChangeTracking> changeTrackingRepository)
        {
            _ChangeTrackingRepository = changeTrackingRepository;
        }

        // GET: Mouvement
        public ActionResult Index()
        {
            return View(_ChangeTrackingRepository.GetAll());
        }
    }
}