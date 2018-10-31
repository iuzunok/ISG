﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISGWebSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult Deneme()
        {
            ViewBag.Message = "Deneme sayfa.";
            return View();
        }

        public ActionResult Deneme2()
        {
            ViewBag.Message = "Deneme sayfa2.";
            return View();
        }

       

    }
}