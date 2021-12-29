using DoAn_CSDL.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_CSDL.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NotFound()
        {
            //404
            ViewBag.BackgroundURL = Constants.ErrorFolder + "404.jpg";

            return View();
        }
    }
}