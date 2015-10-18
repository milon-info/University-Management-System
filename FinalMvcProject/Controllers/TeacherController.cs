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
    public class TeacherController : Controller
    {
        private UniversityDbContext db = new UniversityDbContext();

        // GET: /Teacher/
        public ActionResult Index()
        {
            var teachers = db.Teachers.Include(t => t.Department).Include(t => t.Designation);
            return View(teachers.ToList());
        }

       
        // GET: /Teacher/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "Name");
            return View();
        }

        // POST: /Teacher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="TeacherId,Name,Address,Email,ContactNo,DesignationId,DepartmentId,CreditTaken")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Teachers.Add(teacher);
                db.SaveChanges();
                TempData["Success"] = "Teacher Added Successfully!!";
                return RedirectToAction("Create");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", teacher.DepartmentId);
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "Name", teacher.DesignationId);
            return View(teacher);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult IsUserEmailUnique(string email)
        {
            var teacher = db.Teachers.FirstOrDefault(x => x.Email == email);
            if (teacher != null)
            {
                return Json("Teacher Email is already registered ", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
