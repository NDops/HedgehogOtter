﻿using HedgeHogOtter.Models;
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
        public ActionResult Admin()
        {
            
            var bookList = db.Books.ToList();
            var bookDisplayList = new List<Book>();
            String table = "";
            for (int i =0; i < bookList.Count; i++)
            {
                table += "<tr><td><a href = 'Edit/"+bookList.ElementAt(i).Id +"'>" + bookList.ElementAt(i).Title + "</a></td><td>"+ bookList.ElementAt(i).Author +"</td> <td> "+bookList.ElementAt(i).Quantity +" </td ><td><button onclick = 'verify(0,"+bookList.ElementAt(i).Id +")' > Delete </button></td> </tr> ";
            }
            ViewBag.table = table;
            return View();
        }
       

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("index");
            }else{
                return View(db.Books.Find(id));

            }
            
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
                return RedirectToAction("Admin");
            }
            catch
            {
                return View();
            }
        }
    }
}