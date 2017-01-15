using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            //last 3 comments
            var allComments = db.Notes.ToList();
            var lastComments = allComments.OrderByDescending(p => p.DateCreated).Take(3);

            return View(lastComments);
        }

        [Authorize]
        public ActionResult ListAllUpdates()
        {
            //all comments
            List<Note> noteList = new List<Note>();
            var notes = db.Notes.OrderByDescending(p => p.DateCreated);
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