using HedgeHogOtter.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HedgeHogOtter.Controllers
{
    public class HomeController : Controller
    {

        private HedgeHogOtterEntities db = new HedgeHogOtterEntities();

        public ActionResult Index()
        {

            var bookList = db.Books.ToList();
            var bookDisplayList = new List<Book>();
            while(bookDisplayList.Count < 3)
            {
                Random rnd = new Random();
                int bookIndex = rnd.Next(bookList.Count);
                if (!bookDisplayList.Contains(bookList.ElementAt(bookIndex)))
                {
                    bookDisplayList.Add(bookList.ElementAt(bookIndex));
                }
            }
            ViewBag.bookDisplayList = bookDisplayList;
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
    }
}