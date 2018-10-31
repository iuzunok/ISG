using ISGWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISGWebSite.Controllers
{
    public class PersonelController : Controller
    {
        PGDbContext _context;

        public PersonelController()
        {
            _context = new PGDbContext();
        }

        public ActionResult Index()
        {
            return View(_context.Personeller.ToList());
        }

        public ActionResult Personel()
        {
            // Personel oPersonel = _context.Personeller;
            // ViewBag.Message = "Personel sayfa.";
            // return View();
            return View(_context.Personeller.ToList());
        }

        //// GET: Personel
        //public ActionResult Index()
        //{
        //    return View();
        //}
    }
}