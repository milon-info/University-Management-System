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
    public class StudentController : Controller
    {
        private UniversityDbContext db = new UniversityDbContext();

        // GET: /Student/
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Department);
            return View(students.ToList());
        }

        // GET: /Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: /Student/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
            return View();
        }

        // POST: /Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="StudentId,Name,Email,ContactNo,Date,Address,DepartmentId,RegistrationId")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.RegistrationId = GenarateRegistationId(student);

                db.Students.Add(student);
                db.SaveChanges();
                TempData["reg"] = student.Name + ". RegistrationId is : " + student.Name;
                ViewBag.RegId = student.Name + ". RegistrationId is : " + student.Name;
               
                return RedirectToAction("Details", new { id = student.StudentId });
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentCode", student.DepartmentId);
            return View(student);
        }

        private string GenarateRegistationId(Student student)
        {
            int id = db.Students.Count(
                    s => (s.DepartmentId == student.DepartmentId) &&
                         (s.Date.Year == student.Date.Year)) + 1;
            Department aDepartment = db.Departments.FirstOrDefault(d => d.DepartmentId == student.DepartmentId);
            string registrationId = aDepartment.Code + "-" + student.Date.Year + "-";
            return registrationId + id.ToString("000");

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult IsEmailUnique(string email)
        {
            var student = db.Students.FirstOrDefault(x => x.Email == email);
            if (student != null)
            {
                return Json("Student Email is already registered ", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
