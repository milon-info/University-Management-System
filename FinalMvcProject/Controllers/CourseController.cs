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
    public class CourseController : Controller
    {
        private UniversityDbContext db = new UniversityDbContext();

        // GET: /Course/
        public ActionResult Index()
        {
            var courses = db.Courses.Include(c => c.Department).Include(c => c.Semester);
            return View(courses.ToList());
        }


        // GET: /Course/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
            ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "Name");
            return View();
        }

        // POST: /Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CourseId,Code,Name,Credit,Description,DepartmentId,SemesterId")] Course course)
        {
            course.Status = true;
            var list = from p in db.Semesters where p.SemesterId == course.SemesterId select p;
            foreach (var item in list)
            {
                course.SemName = item.Name;
            }
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                TempData["Success"] = "Course Added Successfully!!";
                return RedirectToAction("Create");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", course.DepartmentId);
            ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "Name", course.SemesterId);
            return View(course);
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
            var course = db.Courses.FirstOrDefault(x => x.Code == code);
            if (course != null)
            {
                return Json("Course Code is already registered ", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsUserNameAvailable(string name)
        {
            var course = db.Courses.FirstOrDefault(x => x.Name == name);
            if (course != null)
            {
                return Json("Course Name is already registered ", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
