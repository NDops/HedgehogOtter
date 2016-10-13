using HedgeHogOtter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HedgeHogOtter.Controllers
{
    public class BookController : Controller
    {

        private HedgeHogOtterEntities db = new HedgeHogOtterEntities();
        static List<Book> globalBookList = new List<Book>();

        public ActionResult Index()
        {
            globalBookList = db.Books.ToList();
            return View(globalBookList);
        }

        // GET: Book
        public ActionResult CRUD()
        {
            
            var bookList = db.Books.ToList();
            var bookDisplayList = new List<Book>();
            String table = "";
            for (int i =0; i < bookList.Count; i++)
            {
                table += " <tr><td><a href = 'EditBook?id="+bookList.ElementAt(i).Id +"'>" + bookList.ElementAt(i).Title + "</a></td><td>"+ bookList.ElementAt(i).Author +"</td> <td> "+bookList.ElementAt(i).Quantity +" </td ><td><button onclick = 'verify(0,"+bookList.ElementAt(i).Id +")' > Delete </button></td> </tr> ";

            }
            ViewBag.table = table;
            return View();
        }
        public ActionResult AddBook(List<string> book)
        {

            return View();
        }
        public ActionResult EditBook()
        {
            int id = int.Parse(Request.QueryString["id"]);
            var Book = db.Books.Find(id);

            String table = "";

            
            table += "<table><tr><td>Title:</td> <td><input name = 'Title' type='text' value='"+Book.Title+"'/></td></tr><br>";
            table += "<tr><td>Author:</td> <td><input  name = 'Author' type='text' value='" + Book.Author + "'/></td></tr><br>";
            table += "<tr><td>Book Condition:</td><td> <input  name = 'BookCnd' type='text' value='" + Book.BookCondition + "'/></td></tr><br>";
            table += "<tr><td>Description:</td> <td><input  name = 'Description' type='text' value='" + Book.Description + "'/></td></tr><br>";
            table += "<tr><td>Subject:</td><td> <input  name = 'Subject' type='text' value='" + Book.Subject + "'/></td></tr><br>";
            table += "<tr><td>Quantity:</td> <td><input name = 'Qty' type='text' value='" + Book.Quantity + "'/></td></tr><br>";
            table += "<tr><td>Price:</td><td> <input  name = 'Price' type='text' value='" + Book.Price + "'/></td></tr><br>";
            table += "<tr><td>ISBN #:</td><td> <input name = 'ISBN' type='text' value='" + Book.ISBN + "'/></td></tr><br>";
            table += "<tr><td>Publisher:</td><td> <input  name = 'Publisher' type='text' value='" + Book.Publisher + "'/></td></tr><br>";
            table += "<tr><td>Publisher Place:</td> <td><input  name = 'PublisherPlace'  type='text' value='" + Book.PublisherPlace + "'/></td></tr><br>";
            table += "<tr><td>Publish Year:</td><td> <input  name = 'PublisherYear' type='text' value='" + Book.PublishYear + "'/></td></tr></table><br>";
            table += "<input  name = 'Id' type='hidden' value='" + Book.Id + "'/></table><br>";

            ViewBag.table = table;
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(db.Books.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Title,Author,BookCondition,Description,Subject,Quantity,Price,ISBN,Publisher,PublisherPlace,PublishYear")] Book b)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", b);
            }
            db.Entry(b).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
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

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateBook()
        {
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        public ActionResult CreateBook(FormCollection collection)
        {
            try
            {
                    Book b = new Book();

                
                    b.Title = Request.Form["Title"];
                    b.Author = Request.Form["Author"];
                    b.BookCondition = Request.Form["BookCnd"];
                    b.Description = Request.Form["Description"];
                    b.Subject = Request.Form["Subject"];
                    b.Quantity = int.Parse(Request.Form["Qty"]);
                    b.Price = int.Parse(Request.Form["Price"]);
                    b.ISBN = Request.Form["ISBN"];
                    b.Publisher = Request.Form["Publisher"];
                    b.PublisherPlace = Request.Form["PublisherPlace"];
                    b.PublishYear = int.Parse(Request.Form["PublisherYear"]);

                    db.Books.Add(b);
                    db.SaveChanges();
                    return RedirectToAction("CRUD");

            }
            catch
            {
                return View();
            }
        }

        // POST: Book/Edit/5
        [HttpPost]
        public ActionResult EditBook(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                Book b = new Book();
                int tempID = int.Parse(Request.Form["id"]);

                b.Title = Request.Form["Title"];
                b.Author = Request.Form["Author"];
                b.BookCondition = Request.Form["BookCnd"];
                b.Description = Request.Form["Description"];
                b.Subject = Request.Form["Subject"];
                b.Quantity = int.Parse(Request.Form["Qty"]);
                b.Price = int.Parse(Request.Form["Price"]);
                b.ISBN = Request.Form["ISBN"];
                b.Publisher = Request.Form["Publisher"];
                b.PublisherPlace = Request.Form["PublisherPlace"];
                b.PublishYear = int.Parse(Request.Form["PublisherYear"]);

                db.Books.Find(tempID).Title = b.Title;
                db.Books.Find(tempID).Author = b.Author;
                db.Books.Find(tempID).BookCondition = b.BookCondition;
                db.Books.Find(tempID).Description = b.Description;
                db.Books.Find(tempID).Subject = b.Subject;
                db.Books.Find(tempID).Quantity = b.Quantity;
                db.Books.Find(tempID).Price = b.Price;
                db.Books.Find(tempID).ISBN = b.ISBN;
                db.Books.Find(tempID).Publisher = b.Publisher;
                db.Books.Find(tempID).PublisherPlace = b.PublisherPlace;
                db.Books.Find(tempID).PublishYear = b.PublishYear;

                db.SaveChanges();
                return RedirectToAction("CRUD");
            }
            catch
            {
                return RedirectToAction("CRUD");
            }
        }

        public ActionResult Delete(int id)
        {
            db.Books.Remove(db.Books.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Book/Delete/5
        [HttpPost]
        public ActionResult DeleteBook(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                int deleteID = int.Parse(Request.Form["id"]);
                db.Books.Remove(db.Books.Find(deleteID));
                db.SaveChanges();
                return RedirectToAction("CRUD");
            }
            catch
            {
                return View();
            }
        }
    }
}
