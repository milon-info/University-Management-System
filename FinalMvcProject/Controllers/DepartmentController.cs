using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalMvcProject.Models;

namespace FinalMvcProject.Controllers
{
    public class DepartmentController : Controller
    {
        private UniversityDbContext db = new UniversityDbContext();

        // GET: /Department/
        public ActionResult Index()
        {
            return View(db.Departments.ToList());
        }

        // GET: /Department/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentId,Code,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();
                TempData["Success"] = "Department Added Successfully!!";
                return RedirectToAction("Create");
            }

            return View(department);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult IsUserCodeAvailable(string code)
        {
            var department = db.Departments.FirstOrDefault(x => x.Code == code);
            if (department != null)
            {
                return Json("Department Code is already registered ", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsUserNameAvailable(string name)
        {
            var department = db.Departments.FirstOrDefault(x => x.Name == name);
            if (department != null)
            {
                return Json("Department Name is already registered ", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
