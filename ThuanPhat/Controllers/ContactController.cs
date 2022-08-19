using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuanPhat.DAL;
using ThuanPhat.Models;
using ThuanPhat.ViewModel;

namespace ThuanPhat.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        #region Contact
        public ActionResult ListContact(int? page, string name)
        {
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var contact = _unitOfWork.ContactRepository.GetQuery(orderBy: l => l.OrderByDescending(a => a.Id));

            if (!string.IsNullOrEmpty(name))
            {
                contact = contact.Where(l => l.FullName.Contains(name));
            }
            var model = new ListContactViewModel
            {
                Contacts = contact.ToPagedList(pageNumber, pageSize),
                Name = name
            };
            return View(model);
        }
        [HttpPost]
        public bool DeleteContact(int contactId = 0)
        {
            var contact = _unitOfWork.ContactRepository.GetById(contactId);
            if (contact == null)
            {
                return false;
            }
            _unitOfWork.ContactRepository.Delete(contact);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Subcribe
        public ActionResult ListSubcribe(int? page, string name)
        {
            var pageNumber = page ?? 1;
            var pageSize = 15;
            var subcribes = _unitOfWork.SubcribeRepository.GetQuery(orderBy: l => l.OrderByDescending(a => a.Id));
            if (!string.IsNullOrEmpty(name))
            {
                subcribes = subcribes.Where(l => l.Email.Contains(name));
            }
            var model = new ListSubcribeViewModel
            {
                Subcribes = subcribes.ToPagedList(pageNumber, pageSize),
                Name = name
            };
            return View(model);
        }
        [HttpPost]
        public bool DeleteSubcribe(int subId = 0)
        {
            var subcribe = _unitOfWork.SubcribeRepository.GetById(subId);
            if (subcribe == null)
            {
                return false;
            }
            _unitOfWork.SubcribeRepository.Delete(subcribe);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Recruit
        public ActionResult ListRecruit(int? page, string name)
        {
            var pageNumber = page ?? 1;
            var pageSize = 15;
            var recruits = _unitOfWork.RecruitRepository.GetQuery(orderBy: l => l.OrderByDescending(a => a.Id));
            if (!string.IsNullOrEmpty(name))
            {
                recruits = recruits.Where(l => l.FullName.Contains(name));
            }
            var model = new ListRecruitViewModel
            {
                Recruits = recruits.ToPagedList(pageNumber, pageSize),
                Name = name
            };
            return View(model);
        }
        [HttpPost]
        public bool DeleteRecruit(int recruitId = 0)
        {
            var recruit = _unitOfWork.RecruitRepository.GetById(recruitId);
            if (recruit == null)
            {
                return false;
            }
            _unitOfWork.RecruitRepository.Delete(recruit);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region Career
        [ChildActionOnly]
        public ActionResult ListCareer()
        {
            var careers = _unitOfWork.CareerRepository.Get(orderBy: q => q.OrderBy(c => c.Sort));
            return PartialView(careers);
        }
        public ActionResult Career(string result = "")
        {
            ViewBag.Career = result;
            var career = new Career { Sort = 1, Active = true };
            return View(career);
        }
        [HttpPost]
        public ActionResult Career(Career model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CareerRepository.Insert(model);
                _unitOfWork.Save();
                return RedirectToAction("Career", new { result = "success" });
            }
            return View(model);
        }
        public ActionResult EditCareer(int careerId = 0)
        {
            var career = _unitOfWork.CareerRepository.GetById(careerId);
            if (career == null)
            {
                return RedirectToAction("Career");
            }
            return View(career);
        }
        [HttpPost]
        public ActionResult EditCareer(Career model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CareerRepository.Update(model);
                _unitOfWork.Save();
                return RedirectToAction("Career", new { result = "update" });
            }
            return View(model);
        }
        [HttpPost]
        public bool DeleteCareer(int careerId = 0)
        {
            var career = _unitOfWork.CareerRepository.GetById(careerId);
            if (career == null)
            {
                return false;
            }

            _unitOfWork.CareerRepository.Delete(careerId);
            _unitOfWork.Save();
            return true;
        }

        public ActionResult UpdateCareerLang(int careerId, int result = 0)
        {
            ViewBag.Result = result;
            var career = _unitOfWork.CareerRepository.GetById(careerId);
            return View(career);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateCareerLang(FormCollection fc)
        {
            var careerId = Convert.ToInt32(fc["CareerId"]);
            var langId = Convert.ToInt32(fc["LangId"]);
            var name = fc["Name"];

            var conceptLang = _unitOfWork.CareerLangRepository.GetQuery(a => a.CareerId == careerId && a.LanguageId == langId).SingleOrDefault();

            if (conceptLang == null)
            {
                _unitOfWork.CareerLangRepository.Insert(new CareerLang
                {
                    CareerId = careerId,
                    LanguageId = langId,
                    Name = name,
                });
                _unitOfWork.Save();
                return RedirectToAction("UpdateCareerLang", new { careerId, result = 1 });
            }

            conceptLang.Name = name;

            _unitOfWork.Save();
            return RedirectToAction("UpdateCareerLang", new { careerId, result = 1 });
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}