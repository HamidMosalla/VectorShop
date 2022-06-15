using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using PagedList;
using VectorShop.Helpers;
using VectorShop.Models;
using VectorShop.Models.ViewModels;

namespace VectorShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class NewsLetterController : Controller
    {
        private VectorShopDb _db = new VectorShopDb();

        [Route("NewsLetter/ManageNewsLetter/{page?}")]
        public ActionResult ManageNewsLetter(int? page)
        {
            var model = _db.NewsLetters.OrderByDescending(c => c.NewsLetterId);
            var pageNumber = page ?? 1;
            var onePageOfLetters = model.ToPagedList(pageNumber, 2);
            //TODO: Increase the paging size when project finished.

            return View(onePageOfLetters);
        }

        [AllowAnonymous]
        public ActionResult Subscribe()
        {
            var priCatlistItems = new List<SelectListItem>();

            foreach (var item in _db.PriCats)
            {
                priCatlistItems.Add(new SelectListItem
                {
                    Text = item.PriCatTitle,
                    Value = item.PriCatId.ToString()
                });
            }

            ViewBag.priCatList = priCatlistItems;



            return View();
        }

        public ActionResult SendMail()
        {

            return View();
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> SendMail(EmailViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var message = new MailMessage();
                    message.To.Add(new MailAddress("ghamran@gmail.com", "اعضای خبرنامه وکتورشاپ"));

                    var emailList = _db.NewsLetters.Where(n => n.IsSelected).Select(n => new { n.NewsLetterEmail, n.NewsLetterSubscriberName }).Distinct();
                    foreach (var item in emailList)
                    {
                        message.Bcc.Add(new MailAddress(item.NewsLetterEmail, item.NewsLetterSubscriberName));
                    }

                    //message.From = new MailAddress("info@webfor.ir", "VectorShop"); we set it in web.config
                    //TODO don't forget to chage the values to the real customer value
                    message.Subject = viewModel.Subject;

                    var model = _db.Products.OrderByDescending(p => p.ProductId).Take(6);
                    ViewBag.Body = viewModel.Body;
                    //ViewBag.MessagePreview = viewModel.Body.SplitOnDot();
                    message.Body = RenderRazorViewToString("EmailBody", model);

                    message.IsBodyHtml = true;

                    #region we add what we need in web.config, I only keep it for future reference.
                    //using (var smtp = new SmtpClient())
                    //{
                    //    var credential = new NetworkCredential
                    //    {
                    //        UserName = "info@webfor.ir",
                    //        Password = "password"
                    //    };
                    //    smtp.Credentials = credential;
                    //    smtp.Host = "mail.webfor.ir";
                    //    smtp.Port = 25;
                    //    //smtp.EnableSsl = true;
                    //    await smtp.SendMailAsync(message);
                    //}
                    #endregion

                    using (var smtp = new SmtpClient())
                    {
                        await smtp.SendMailAsync(message);
                    }
                    return View("Success");
                }
                catch //(Exception ex)
                {
                    return View(viewModel);
                }

            }
            return View(viewModel);
        }

        [Route("NewsLetter/SelectEmail/{page?}")]
        public ActionResult SelectEmail(int? page, string priCat, string secCat)
        {
            IQueryable<NewsLetter> model = null;
            var pcat = _db.PriCats;
            var scat = _db.SecCats;
            int priCatInt;
            int secCatInt;
            int.TryParse(priCat, out priCatInt);
            int.TryParse(secCat, out secCatInt);

            if (priCatInt == 0 && secCatInt == 0 && priCat != "nullValue" && secCat != "nullValue")
            {
                model = _db.NewsLetters.OrderByDescending(n => n.NewsLetterId);
            }

            if (priCatInt != 0 && secCatInt == 0 && secCat != "nullValue")
            {
                model = _db.NewsLetters.OrderByDescending(n => n.NewsLetterId).Where(n => n.PriCatIDfk == priCatInt);
            }
            else if (secCatInt != 0 && priCatInt == 0 && priCat != "nullValue")
            {
                model = _db.NewsLetters.OrderByDescending(n => n.NewsLetterId).Where(n => n.SecCatIDfk == secCatInt);
            }
            else if (priCatInt != 0 && secCatInt != 0)
            {
                model = _db.NewsLetters.OrderByDescending(
                    n => n.NewsLetterId).Where(n => n.PriCatIDfk == priCatInt && n.SecCatIDfk == secCatInt);
            }
            else if (priCat == "nullValue" && secCat != "nullValue" && secCatInt == 0)
            {
                model = _db.NewsLetters.OrderByDescending(n => n.NewsLetterId).Where(n => n.PriCatIDfk == null);
            }
            else if (secCat == "nullValue" && priCat != "nullValue" && priCatInt == 0)
            {
                model = _db.NewsLetters.OrderByDescending(n => n.NewsLetterId).Where(n => n.SecCatIDfk == null);
            }
            else if (priCat == "nullValue" && secCat == "nullValue")
            {
                model = _db.NewsLetters.OrderByDescending(
                    n => n.NewsLetterId).Where(n => n.PriCatIDfk == null && n.SecCatIDfk == null);
            }
            else if (priCat == "nullValue" && secCat != "nullValue")
            {
                model = _db.NewsLetters.OrderByDescending(
                    n => n.NewsLetterId).Where(n => n.PriCatIDfk == null && n.SecCatIDfk == secCatInt);
            }
            else if (secCat == "nullValue" && priCat != "nullValue")
            {
                model = _db.NewsLetters.OrderByDescending(
                    n => n.NewsLetterId).Where(n => n.SecCatIDfk == null && n.PriCatIDfk == priCatInt);
            }

            ViewBag.SelectedMail = _db.NewsLetters.Count(n => n.IsSelected);
            ViewBag.PriCategory = pcat;
            ViewBag.SecCategory = scat;

            var pageNumber = page ?? 1;
            var onePageOfLetters = model.ToPagedList(pageNumber, 5);
            //TODO: Increase the paging size when project finished.

            return View(onePageOfLetters);
        }

        [HttpPost]
        [Route("NewsLetter/SelectEmail/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult SelectEmail(int id)
        {
            var model = _db.NewsLetters.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                model.IsSelected = !model.IsSelected;
                _db.SaveChanges();
                return Json(new { Status = "Success" });
            }

            catch (Exception eX)
            {
                return Json(new { Status = "Error", eXMessage = eX.Message });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectAllEmail(int[] Id, bool isChecked)
        {
            try
            {
                if (isChecked)
                {
                    foreach (var item in Id)
                    {
                        _db.NewsLetters.Single(n => n.NewsLetterId == item).IsSelected = true;
                    }
                    _db.SaveChanges();
                }

                if (!isChecked)
                {
                    foreach (var item in Id)
                    {
                        _db.NewsLetters.Single(n => n.NewsLetterId == item).IsSelected = false;
                    }
                    _db.SaveChanges();
                }

                return Json(new { Status = "Success" });
            }

            catch (Exception eX)
            {
                return Json(new { Status = "Error", eXMessage = eX.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Subscribe(NewsLetterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var newsLetter = new NewsLetter
                {
                    NewsLetterEmail = viewModel.NewsLetterEmail.Trim(),
                    NewsLetterSubscriberName = viewModel.NewsLetterSubscriberName,
                    PriCatIDfk = viewModel.PriCatIDfk,
                    SecCatIDfk = viewModel.SecCatIDfk == 0 ? null : viewModel.SecCatIDfk,
                    IsActive = true
                };
                _db.NewsLetters.Add(newsLetter);
                _db.SaveChanges();

                return View("Success");
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public JsonResult SubscribeAjax(NewsLetterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //if (!_db.NewsLetters.Any(n => n.NewsLetterEmail == viewModel.NewsLetterEmail)) { } changed my mind, I use distinct on the other side instead.

                var newsLetter = new NewsLetter
                {
                    NewsLetterEmail = viewModel.NewsLetterEmail.Trim(),
                    NewsLetterSubscriberName = viewModel.NewsLetterSubscriberName,
                    PriCatIDfk = viewModel.PriCatIDfk,
                    SecCatIDfk = viewModel.SecCatIDfk == 0 ? null : viewModel.SecCatIDfk,
                    IsActive = true
                };
                _db.NewsLetters.Add(newsLetter);
                _db.SaveChanges();

                return Json(new { Status = "Success" });
            }

            return Json(new { Status = "Failed" });
        }

        [AllowAnonymous]
        public ActionResult UnSubscribe()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult UnSubscribe(string newsLetterEmail)
        {
            if (ModelState.IsValid)
            {
                var model = _db.NewsLetters.Single(n => n.NewsLetterEmail.Trim() == newsLetterEmail.Trim());
                if (model != null)
                {
                    model.IsActive = false;
                    _db.SaveChanges();
                }
                return View("Success");
            }

            return View();
        }

        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.NewsLetters.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            var priCatlistItems = new List<SelectListItem>();

            foreach (var item in _db.PriCats)
            {
                priCatlistItems.Add(new SelectListItem
                {
                    Text = item.PriCatTitle,
                    Value = item.PriCatId.ToString()
                });
            }
            var secCatlistItems = new List<SelectListItem>();

            var secItem = _db.SecCats.SingleOrDefault(s => s.SecCatId == model.SecCatIDfk);

            if (secItem != null)
                secCatlistItems.Add(new SelectListItem { Text = secItem.SecCatTitle, Value = secItem.SecCatId.ToString() });

            ViewBag.priCatList = priCatlistItems;
            ViewBag.secCatList = secCatlistItems;

            var viewModel = Mapper.Map<NewsLetter, NewsLetterViewModel>(model);

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsLetterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var newsLetter = _db.NewsLetters.Find(viewModel.NewsLetterId);
                newsLetter.NewsLetterEmail = viewModel.NewsLetterEmail;
                newsLetter.NewsLetterSubscriberName = viewModel.NewsLetterSubscriberName;
                newsLetter.PriCatIDfk = viewModel.PriCatIDfk;
                newsLetter.SecCatIDfk = viewModel.SecCatIDfk == 0 ? null : viewModel.SecCatIDfk;
                newsLetter.IsActive = viewModel.IsActive;
                _db.SaveChanges();
                return RedirectToAction("ManageNewsLetter");
            }
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = _db.NewsLetters.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            try
            {
                _db.NewsLetters.Remove(model);
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
