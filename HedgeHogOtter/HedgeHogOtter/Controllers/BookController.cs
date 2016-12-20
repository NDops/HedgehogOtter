using HedgeHogOtter.Models;
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
using System.Xml;
using System.IdentityModel;
using System.IdentityModel.Tokens.Jwt;

namespace HedgeHogOtter.Controllers
{
    public class BookController : Controller
    {
        public ActionResult FirstAjax(string id)
        {
            
            var handler = new JwtSecurityTokenHandler();

            var jsonToken = handler.ReadToken(id);
            var temp = jsonToken.ToString();
            return Json(temp, JsonRequestBehavior.AllowGet);
        }
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
            
            return View(bookList);
        }

        [HttpPost]
        [ActionName("Admin")]
        public ActionResult AdminPost(string key)
        {
            var stringArr = Request["flagged"].Split(',');
            int[] intArr = new int[stringArr.Length];
            
            MessageBox.Show(Request["flagged"]);
            for (int j = 0; j < stringArr.Length; j++)
            {
                if (stringArr[j] != "false")
                    intArr[j] = Convert.ToInt32(stringArr[j]);
                else
                    intArr[j] = 0;
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
            postAbeBookData(b, "UPDATE");
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
            postAbeBookData(b, "add");

            return RedirectToAction("admin");
        }

        [HttpPost]  
        public void get_abebooks_data(int id)
        {
            Book b = db.Books.Find(id);
            string url = "http://search2.abebooks.com/search?clientkey=320eb188-fb10-490a-9633-aa360b41df82&vendorid=51369542&isbn=" + b.ISBN;
            XmlDocument xdoc = new XmlDocument();//xml doc used for xml parsing

            xdoc.Load(url);

            XmlNodeList xNodelst = xdoc.DocumentElement.SelectNodes("listingUrl");

            foreach (XmlNode xNode in xNodelst)//traversing XML 
            {
                MessageBox.Show(xNode.Value);
            }
        }

        [HttpPost]
        [ActionName("AddBooks")]
        public void postAbeBookData(Book book, string requestType)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri("https://inventoryupdate.abebooks.com:10027"));
            req.Method = "POST";
            req.ContentType = "application/xml";
            req.Accept = "application/xml";

            XElement request =
                 new XElement("inventoryUpdateRequest",
                     new XAttribute("version", "1.0"),
                     new XElement("action",
                         new XAttribute("name", "bookupdate"),
                         new XElement("username", Environment.GetEnvironmentVariable("client")),
                         new XElement("password", Environment.GetEnvironmentVariable("password"))
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
                //Uncomment This To Read Response From API!
                //MessageBox.Show(r);
            }
            
        }

        // POST: Book/Delete/5
        public ActionResult Delete(int id)
        {
            Book b = db.Books.Find(id);
            postAbeBookData(b, "DELETE");
            db.Books.Remove(b);
            db.SaveChanges();
            return RedirectToAction("Admin");
        }
    }
}