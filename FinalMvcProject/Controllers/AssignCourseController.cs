using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalMvcProject.Models;

namespace FinalMvcProject.Controllers
{
    public class AssignCourseController : Controller
    {
        private UniversityDbContext db = new UniversityDbContext();



        public string Unassign()
        {
            var list = from p in db.Courses where p.Status.Equals(true) select p;

            foreach (var course in list)
            {
                course.Status = false;
                db.Courses.AddOrUpdate(course);

            }
            db.SaveChanges();
            //db.Courses.SqlQuery("Update Courses Set Status='false'");

             return ViewBag.message = "All Courses are unassigned";
           

        }




        public ActionResult Index()
        {
            var assignCourses = db.AssignCourses.Include(a => a.Course).Include(a => a.Department).Include(a => a.Teacher);
            return View(assignCourses.ToList());
        }

        
        // GET: AssignCourse/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code");
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "Name");
            return View();
        }

        // POST: AssignCourse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssignCourseId,DepartmentId,TeacherId,CreditTaken,RemaingCredit,CourseId")] AssignCourse assignCourse)
        {
            //if (ModelState.IsValid)
            //{
            //    db.AssignCourses.Add(assignCourse);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            // [Bind(Include = "AssignCourseId,DepartmentId,TeacherId,CreditTaken,RemaingCredit,CourseId")]


            assignCourse.Status = true;


            if (ModelState.IsValid)
            {
                var result =
                    db.AssignCourses.Count(
                        r => r.CourseId == assignCourse.CourseId) == 0;

                if (result)
                {
                    Teacher aTeacher = db.Teachers.FirstOrDefault(t => t.TeacherId == assignCourse.TeacherId);
                    Course aCourse = db.Courses.FirstOrDefault(c => c.CourseId == assignCourse.CourseId);
                    List<AssignCourse> assignTeachers =
                        db.AssignCourses.Where(t => t.TeacherId == assignCourse.TeacherId).ToList();
                    AssignCourse assign = null;
                    if (assignTeachers.Count != 0)
                    {

                        assign = assignTeachers.Last();
                        assignCourse.RemaingCredit = assign.RemaingCredit;
                    }
                    else
                    {
                        assignCourse.RemaingCredit = aTeacher.CreditTaken;
                    }

                    //if (assign == null)
                    //    assigncourse.RemaingCredit = aTeacher.CreditTaken;

                    if (assignCourse.RemaingCredit < aCourse.Credit)
                    {
                        Session["Teacher"] = aTeacher;
                        Session["Course"] = aCourse;
                        Session["AssignedCourse"] = assignCourse;
                        Session["AssigneddCourseCheck"] = assign;
                        return RedirectToAction("AskToAssign");
                    }

                    assignCourse.CreditTaken = aTeacher.CreditTaken;

                    if (assign == null)
                    {
                        assignCourse.RemaingCredit = aTeacher.CreditTaken - aCourse.Credit;
                    }
                    else
                    {
                        assignCourse.RemaingCredit = assign.RemaingCredit - aCourse.Credit;
                    }

                    aCourse.AssignTo = aTeacher.Name;

                    db.AssignCourses.Add(assignCourse);
                    db.SaveChanges();
                    TempData["success"] = "Course Assigned Successfully!!";
                    return RedirectToAction("Create");
                }
                else
                {
                    TempData["Already"] = "Course Has Already Been Assigned";
                    return RedirectToAction("Create");
                }
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", assignCourse.CourseId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", assignCourse.DepartmentId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "Name", assignCourse.TeacherId);
            return View(assignCourse);
        }




        public ActionResult AskToAssign()
        {
            Teacher aTeacher = (Teacher)Session["Teacher"];
            Course aCourse = (Course)Session["Course"];
            AssignCourse assign = (AssignCourse)Session["AssigneddCourseCheck"];
            double remainingCredite = 0.0;
            if (assign == null)
                remainingCredite = aTeacher.CreditTaken;
            else
            {
                remainingCredite = assign.RemaingCredit;
            }
            if (remainingCredite < 0)
            {
                ViewBag.Message = aTeacher.Name
                + " Credit Limit Is Over. And The Course Credit  : " + aCourse.Code
                + " Is " + aCourse.Credit
                + "  ! Still You Want To Assign This Course To This Teacher ?";
            }
            else
            {
                ViewBag.Message = aTeacher.Name
                + " has only " + remainingCredite
                + " Credits Remaining . But, The Credit  : " + aCourse.Code
                + " Is " + aCourse.Credit
                + "  ! Still You Want To Assign This Course To This Teacher ?";
            }

            return View();
        }

        public ActionResult AssignConfirmed()
        {
            Teacher aTeacher = (Teacher)Session["Teacher"];

            AssignCourse assigncourse = (AssignCourse)Session["AssignedCourse"];
            AssignCourse assign = (AssignCourse)Session["AssigneddCourseCheck"];
            Course aCourse = db.Courses.FirstOrDefault(c => c.CourseId == assigncourse.CourseId);


            assigncourse.CreditTaken = aTeacher.CreditTaken;
            if (assign == null)
            {
                assigncourse.RemaingCredit = aTeacher.CreditTaken - aCourse.Credit;
            }
            else
            {
                assigncourse.RemaingCredit = assign.RemaingCredit - aCourse.Credit;
            }

            aCourse.AssignTo = aTeacher.Name;

            db.AssignCourses.Add(assigncourse);
            db.SaveChanges();
            TempData["success"] = "Course Is Assigned Successfully!!!";
            return View();
        }





       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        public ActionResult LoadTeacher(int? departmentId)
        {
            var teacherList = db.Teachers.Where(u => u.DepartmentId == departmentId).ToList();
            ViewBag.TeacherId = new SelectList(teacherList, "TeacherId", "Name");
            return PartialView("~/Views/Shared/_FillteredTeacher.cshtml");
        }

        public ActionResult LoadCourse(int? departmentId)
        {
            var courseList = db.Courses.Where(u => u.DepartmentId == departmentId).ToList();
            ViewBag.CourseId = new SelectList(courseList, "CourseId", "Name");
            return PartialView("~/Views/shared/_FilteredCourse.cshtml");

        }

        public PartialViewResult TeacherInfoLoad(int? teacherId)
        {
            if (teacherId != null)
            {
                Teacher aTeacher = db.Teachers.FirstOrDefault(s => s.TeacherId == teacherId);
                ViewBag.Credit = aTeacher.CreditTaken;
                List<AssignCourse> assignTeachers =
                        db.AssignCourses.Where(t => t.TeacherId == teacherId).ToList();
                AssignCourse assign = null;
                if (assignTeachers.Count != 0)
                {
                    assign = assignTeachers.Last();
                }
                if (assign == null)
                {
                    ViewBag.RemainingCredit = aTeacher.CreditTaken;
                }
                else
                {
                    ViewBag.RemainingCredit = assign.RemaingCredit;
                }

                return PartialView("~/Views/Shared/_TeachersCreditInfo.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_TeachersCreditInfo.cshtml");
            }

        }


        public PartialViewResult CourseInfoLoad(int? courseId)
        {
            if (courseId != null)
            {
                Course aCourse = db.Courses.FirstOrDefault(s => s.CourseId == courseId);
                ViewBag.Code = aCourse.Code;
                ViewBag.Credit = aCourse.Credit;
                return PartialView("~/Views/Shared/_CourseInfo.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/_CourseInfo.cshtml");
            }

        }

        public ActionResult ViewCourseStatus()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
            return View();
        }

        public PartialViewResult CourseStatusLoad(int? departmentId)
        {
            List<Course> courseList = new List<Course>();
            if (departmentId != null)
            {
                courseList = db.Courses.Where(r => r.DepartmentId == departmentId).ToList();
                foreach (var course in courseList)
                {
                    if (course.Status.Equals(false))
                    {
                        course.AssignTo = null;
                    }
                }
                if (courseList.Count == 0)
                {
                    ViewBag.NotAssigned = "Department Empty";
                }
            }


            return PartialView("~/Views/shared/_CourseStatus.cshtml", courseList);
        }

    }
}
