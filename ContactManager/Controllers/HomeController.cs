using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactManager.Core;
using ContactManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = AppSettings.Title;

            using (AppDbContext db = new AppDbContext())
            {
                var query = from contact in db.Contacts
                            orderby contact.Id ascending
                            select contact;
                List<Contact> model = query.ToList();
                return View(model);
            }
        }

        public IActionResult AddContact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddContact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                using (AppDbContext db = new AppDbContext())
                {
                    db.Contacts.Add(contact);
                    db.SaveChanges();
                    ViewBag.Message = "Contact is added successfully!";
                }
            }

            return View(contact);
        }

        public IActionResult DeleteContact(int id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                var contact = (from c in db.Contacts
                               where c.Id == id
                               select c).SingleOrDefault();
                db.Contacts.Remove(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}