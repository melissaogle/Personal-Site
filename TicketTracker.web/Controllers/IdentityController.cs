using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketTracker.web.Controllers
{
    [Authorize]
    public class IdentityController : Controller
    {
        [AllowAnonymous]        
        public ActionResult Index()
        {
            return View();
        }      
        

      [Authorize(Roles ="Manager, Admin")]
        public ActionResult Manager()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminOnly()
        {
            return View();
        }

        [Authorize(Roles = "Technicion")]
        public ActionResult TechnicionsOnly()
        {
            return View();
        }



    }
}