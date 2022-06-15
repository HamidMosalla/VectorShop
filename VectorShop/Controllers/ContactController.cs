using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using VectorShop.Models;
using VectorShop.Models.ViewModels;

namespace VectorShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class ContactController : Controller
    {
        private VectorShopDb _db = new VectorShopDb();

        [Route("Contact/ManageContact/{page?}")]
        public ActionResult ManageContact(int? page)
        {
            var model = _db.Contacts.OrderByDescending(c=>c.ContactId);
            var pageNumber = page ?? 1;
            var onePageOfContacts = model.ToPagedList(pageNumber, 10);
            //TODO: Increase the paging size when project finished.

            return View(onePageOfContacts);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.Contacts.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create(ContactViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var contact = new Contact
                {
                    ContactName = viewModel.ContactName,
                    ContactEmail = viewModel.ContactEmail,
                    ContactPhone = viewModel.ContactPhone,
                    ContactBody = viewModel.ContactBody,
                    ContactDate = DateTime.Now
                };
                _db.Contacts.Add(contact);
                _db.SaveChanges();
                return View("Success");
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public JsonResult CreateAjax(ContactViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var contact = new Contact
                {
                    ContactName = viewModel.ContactName,
                    ContactEmail = viewModel.ContactEmail,
                    ContactPhone = viewModel.ContactPhone,
                    ContactBody = viewModel.ContactBody,
                    ContactDate = DateTime.Now
                };
                _db.Contacts.Add(contact);
                _db.SaveChanges();
                return Json(new { Status = "Success" });
            }

            return Json(new { Status = "Failed" });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.Contacts.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                _db.Contacts.Remove(model);
                _db.SaveChanges();
                return Json(new { Status = "Deleted" });
            }

            catch (Exception eX)
            {
                return Json(new { Status = "Error", eXMessage = eX.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
