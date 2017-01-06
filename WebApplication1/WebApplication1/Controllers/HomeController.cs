using System.Web.Mvc;
using WebApplication1.Enumerations;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication1.Controllers
{
    public class HomeController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            //last 3 comments
            var allComments = db.Notes.ToList();
            var lastComments = allComments.OrderByDescending(p => p.DateCreated).Take(3);

            return View(lastComments);
        }

        public ActionResult ListAllUpdates()
        {
            //all comments
            List<Note> noteList = new List<Note>();
            var notes = db.Notes;
            foreach (var k in notes)
            {
                noteList.Add(k);
            }

            return View(noteList);

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

        public ActionResult LandingPage()
        {
            return View();
        }
    }
}