﻿using HedgeHogOtter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Web;

namespace HedgeHogOtter.Controllers
{
    public class BookController : Controller
    {

        private HedgeHogOtterContext db = new HedgeHogOtterContext();
        static List<Book> globalBookList = new List<Book>();


        public ActionResult Index()
        {
            var searchText = Request.Form["searchBar"];
            if (searchText == null || searchText.Equals(""))
            {
                globalBookList = db.Books.ToList();
            }
            else
            {
                globalBookList = db.Books.Select(b => b).Where(b => b.Title.ToLower().Contains(searchText.ToLower()) || b.Author.ToLower().Contains(searchText.ToLower())).ToList();
            }

            return View(globalBookList);
        }

        // GET: Book
        public ActionResult Admin()
        {

            var bookList = db.Books.ToList();
            var bookDisplayList = new List<Book>();
            string checkedBox;
            String table = "";


            for (int i = 0; i < bookList.Count; i++)
            {
                if (bookList.ElementAt(i).FeatureFlag == 1)  //featured flag 
                {
                    checkedBox = " type ='checkbox' checked />";
                }
                else
                {
                    checkedBox = " type ='checkbox' />";
                }
                table += "<tr style='border: 1px solid black;'>";
                table += "<td style='border: 1px solid black; '><input name = 'flaggedbox'  datac = '" + bookList.ElementAt(i).FeatureFlag + "' value = '" + bookList.ElementAt(i).Id + @"'" + checkedBox + " <td style='border: 1px solid black; '><a href = 'Edit/" + bookList.ElementAt(i).Id + "'>" + bookList.ElementAt(i).Title + "</a></td>";
                table += "<td style='border: 1px solid black; '>" + bookList.ElementAt(i).Author + "</td> <td style='border: 1px solid black;'> " + bookList.ElementAt(i).Quantity + "</td>";
                table += "<td style='border: 1px solid black; '><button type = 'button' onclick = 'verify(0," + bookList.ElementAt(i).Id + ")' > Delete </button></td> </tr> ";
            }
            ViewBag.table = table;


            return View();
        }
        [HttpPost]
        [ActionName("Admin")]
        public ActionResult AdminPost(string key)
        {

            var stringArr = Request["flaggedbox"].Split(',');
            int[] intArr = new int[stringArr.Length];


            for (int j = 0; j < stringArr.Length; j++)
            {
                intArr[j] = Convert.ToInt32(stringArr[j]);
            }

            var items = db.Books.ToList();

            for (int i = 0; i < items.Count; i++)
            {
                Book b = items.ElementAt(i);

                if (intArr.Contains(b.Id))
                {
                    b.FeatureFlag = 1;
                    db.Entry(b).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    b.FeatureFlag = 0;
                    db.Entry(b).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Admin");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("admin");
            }
            else
            {
                return View(db.Books.Find(id));
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Author,BookCondition,Description,Subject,Quantity,Price,ISBN,Publisher,PublisherPlace,PublishYear")] Book b)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", b);
            }
            db.Entry(b).State = EntityState.Modified;
            db.SaveChanges();
            postAbeBookData(b.Id, "UPDATE");
            return RedirectToAction("admin");
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            var b = db.Books.Find(id);

            return View(b);
        }

        // GET: Book/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Author,BookCondition,Description,Subject,Quantity,Price,ISBN,Publisher,PublisherPlace,PublishYear")] Book b)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", b);
            }
            db.Books.Add(b);
            db.SaveChanges();
            postAbeBookData(b.Id, "add");

            return RedirectToAction("admin");
        }

        [HttpPost]
        [ActionName("AddBooks")]
        public void postAbeBookData(int id, string requestType)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri("https://inventoryupdate.abebooks.com:10027"));
            req.Method = "POST";
            req.ContentType = "application/xml";
            req.Accept = "application/xml";
            Book book = db.Books.Find(id);
            XElement request =
                 new XElement("inventoryUpdateRequest",
                     new XAttribute("version", "1.0"),
                     new XElement("action",
                         new XAttribute("name", "bookupdate"),
                         new XElement("username", "PhilipKaveny"),
                         new XElement("password", "EE1939aa")
                     ),
                     new XElement("AbebookList",
                         new XElement("Abebook",
                             new XElement("transactionType", requestType),
                             new XElement("vendorBookID", book.Id),
                             new XElement("author", book.Author),
                             new XElement("title", book.Title),
                             new XElement("publisher", book.Publisher),
                             new XElement("subject", book.Subject),
                             new XElement("price",
                                 new XAttribute("currency", "USD"),
                                 book.Price),
                             new XElement("description", book.Description),
                             new XElement("bookCondition", book.BookCondition),
                             new XElement("isbn", book.ISBN),
                             new XElement("publishPlace", book.PublisherPlace),
                             new XElement("publishYear", book.PublishYear),
                             new XElement("quantity",
                                 new XAttribute("amount", book.Quantity))
                         )
                     )
                 );

            byte[] bytes = Encoding.UTF8.GetBytes(request.ToString());

            req.ContentLength = bytes.Length;

            using (Stream putStream = req.GetRequestStream())
            {
                putStream.Write(bytes, 0, bytes.Length);
            }

            // Log the response from Abebooks for testing 
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string r = reader.ReadToEnd();
            }

        }

        // POST: Book/Delete/5
        [HttpPost]
        public ActionResult DeleteBook(int id, System.Web.Mvc.FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                int deleteID = int.Parse(Request.Form["id"]);
                db.Books.Remove(db.Books.Find(deleteID));
                db.SaveChanges();
                return RedirectToAction("Admin");
            }
            catch
            {
                return View();
            }
        }
    }
}