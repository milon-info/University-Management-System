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
    public class EnrollCourseController : Controller
    {
        private UniversityDbContext db = new UniversityDbContext();

        // GET: /EnrollCourse/
        public ActionResult Index()
        {
            var enrollcourses = db.EnrollCourses.Include(e => e.Student).Include(e => e.Course);
            return View(enrollcourses.ToList());
        }

        
        //
        // GET: /EnrollCourse/Create

        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegistrationId");
            ViewBag.CourseId = new SelectList("", "CourseId", "Code");
            return View();
        }

        //
        // POST: /EnrollCourse/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EnrollCourse enrollcourse)
        {
            if (ModelState.IsValid)
            {
                var result = db.EnrollCourses.Count(u => u.StudentId == enrollcourse.StudentId && u.CourseId == enrollcourse.CourseId) == 0;
                if (result)
                {
                    TempData["success"] = "Course Enrolled For This Student";
                    db.EnrollCourses.Add(enrollcourse);
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }
                else
                {
                    TempData["Already"] = "Student Has Already Enrolled This Course";
                    return RedirectToAction("Create");
                }
            }

            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "RegistrationId", enrollcourse.StudentId);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", enrollcourse.CourseId);
            return View(enrollcourse);
        }

        public PartialViewResult CourseLoad(int? studentId)
        {

            List<Course> courseList = new List<Course>();
            if (studentId != null)
            {
                Student aStudent = db.Students.Find(studentId);
                courseList = db.Courses.Where(e => e.DepartmentId == aStudent.DepartmentId).ToList();
                ViewBag.CourseId = new SelectList(courseList, "CourseId", "Code");
            }
            return PartialView("~/Views/shared/_FilteredCourse.cshtml");
        }

        public PartialViewResult StudentInfoLoad(int? studentId)
        {
            if (studentId != null)
            {
                Student aStudent = db.Students.FirstOrDefault(s => s.StudentId == studentId);
                ViewBag.Name = aStudent.Name;
                ViewBag.Email = aStudent.Email;
                ViewBag.Dept = aStudent.Department.Name;
                return PartialView("~/Views/Shared/_StudentInformation.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_StudentInformation.cshtml");
            }

        }

        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


    }
}
