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
    public class RoomAllocationController : Controller
    {
        private UniversityDbContext db = new UniversityDbContext();


        public string Unallocate()
        {
            var rooms = from p in db.RoomAllocations where p.Status.Equals(true) select p;
            foreach (var roomAllocation in rooms)
            {
                roomAllocation.Status = false;
                db.RoomAllocations.AddOrUpdate(roomAllocation);
            }
            db.SaveChanges();
            return "All Rooms Are Unallocated";
        }


        public ActionResult Index()
        {
            var roomallocations = db.RoomAllocations.Include(r => r.Department).Include(r => r.Course).Include(r => r.Room).Include(r => r.Day);
            return View(roomallocations.ToList());
        }

       
        // GET: /RoomAllocation/Create

        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
            ViewBag.CourseId = new SelectList("", "CourseId", "Code");
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name");
            ViewBag.DayId = new SelectList(db.Days, "DayId", "Name");
            return View();
        }

        //
        // POST: /RoomAllocation/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoomAllocation roomallocation)
        {

            Room room = db.Rooms.Find(roomallocation.RoomId);
            Course course = db.Courses.Find(roomallocation.CourseId);
            Day day = db.Days.Find(roomallocation.DayId);

            roomallocation.Status = true;


            if (ModelState.IsValid)
            {

                int givenfrom, givenEnd;

                try
                {
                    givenfrom = ConvertTimetoInt(roomallocation.From);
                }
                catch (Exception anException)
                {
                    if (TempData["ErrorMessage3"] == null)
                    {
                        TempData["ErrorMessage1"] = anException.Message;
                    }
                    TempData["ErrorMessage4"] = TempData["ErrorMessage3"];
                    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", course.DepartmentId);
                    ViewBag.CourseId = new SelectList(db.Courses.Where(c => c.DepartmentId == course.DepartmentId), "CourseId", "Code", roomallocation.CourseId);
                    ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name", roomallocation.RoomId);
                    ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", roomallocation.DayId);
                    return View(roomallocation);

                }

                try
                {
                    givenEnd = ConvertTimetoInt(roomallocation.To);
                }
                catch (Exception anException)
                {
                    if (TempData["ErrorMessage3"] == null)
                    {
                        TempData["ErrorMessage2"] = anException.Message;
                    }
                    TempData["ErrorMessage5"] = TempData["ErrorMessage3"];
                    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", course.DepartmentId);
                    ViewBag.CourseId = new SelectList(db.Courses.Where(c => c.DepartmentId == course.DepartmentId), "CourseId", "Code", roomallocation.CourseId);
                    ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name", roomallocation.RoomId);
                    ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", roomallocation.DayId);
                    return View(roomallocation);
                }

                if (givenEnd < givenfrom)
                {
                    TempData["Error"] = "Class Must Should be Started at 24 hours";
                    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", course.DepartmentId);
                    ViewBag.CourseId = new SelectList(db.Courses.Where(c => c.DepartmentId == course.DepartmentId), "CourseId", "Code", roomallocation.CourseId);
                    ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name", roomallocation.RoomId);
                    ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", roomallocation.DayId);
                    return View(roomallocation);
                }
                if (givenEnd == givenfrom)
                {
                    TempData["Error"] = "Class Must Should be Started at 24 hours";
                    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", course.DepartmentId);
                    ViewBag.CourseId = new SelectList(db.Courses.Where(c => c.DepartmentId == course.DepartmentId), "CourseId", "Code", roomallocation.CourseId);
                    ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name", roomallocation.RoomId);
                    ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", roomallocation.DayId);
                    return View(roomallocation);
                }

                if ((givenfrom < 0) || (givenEnd >= (24 * 60)))
                {
                    TempData["Message"] = " 24 hours--format HH:MM";
                    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", course.DepartmentId);
                    ViewBag.CourseId = new SelectList(db.Courses.Where(c => c.DepartmentId == course.DepartmentId), "CourseId", "Code", roomallocation.CourseId);
                    ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name", roomallocation.RoomId);
                    ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", roomallocation.DayId);
                    return View(roomallocation);
                }

                List<RoomAllocation> overLapList = new List<RoomAllocation>();

                var DayRoomAllocationList =
                db.RoomAllocations.Include(c => c.Course).Include(d => d.Day).Include(r => r.Room).Where(r => r.RoomId == roomallocation.RoomId && r.DayId == roomallocation.DayId)
                    .ToList();

                foreach (var aDayroomAllocation in DayRoomAllocationList)
                {
                    //int OverlapFrom = 0;
                    //int OverlapEnd = 0;
                    //RoomAllocation overlap =new RoomAllocation();
                    if (aDayroomAllocation.Status.Equals(true))
                    {
                        int DbFrom = ConvertTimetoInt(aDayroomAllocation.From);
                        int DbEnd = ConvertTimetoInt(aDayroomAllocation.To);

                        if (
                                ((DbFrom <= givenfrom) && (givenfrom < DbEnd))
                                || ((DbFrom < givenEnd) && (givenEnd <= DbEnd))
                                || ((givenfrom <= DbFrom) && (DbEnd <= givenEnd))
                            )
                        {
                            overLapList.Add(aDayroomAllocation);
                        }
                    }

                }
                if (overLapList.Count == 0)
                {

                    db.RoomAllocations.Add(roomallocation);
                    db.SaveChanges();
                    TempData["Message"] = "Room : " + room.Name + " " + "Allocated "
                    + " for course : " + course.Code + " From " + roomallocation.From
                    + " to " + roomallocation.To + " on " + day.Name;
                }
                else
                {
                    string message = "Room : " + room.Name + " Overlapped With : ";
                    foreach (var anOverlappedRoom in overLapList)
                    {
                        int dbFrom = ConvertTimetoInt(anOverlappedRoom.From);
                        int dbEnd = ConvertTimetoInt(anOverlappedRoom.To);

                        string overlapStart, overlapEnd;

                        if ((dbFrom <= givenfrom) && (givenfrom < dbEnd))
                            overlapStart = roomallocation.From;
                        else
                            overlapStart = anOverlappedRoom.From;

                        if ((dbFrom < givenEnd) && (givenEnd <= dbEnd))
                            overlapEnd = roomallocation.To;
                        else
                            overlapEnd = anOverlappedRoom.To;
                        message += " Course : " + anOverlappedRoom.Course.Code + " Start Time : "
                            + anOverlappedRoom.From + " End Time : "
                            + anOverlappedRoom.To + " Overlapped from : ";
                        message += overlapStart + " To " + overlapEnd;
                    }

                    TempData["Overlapping"] = message + " on " + day.Name;

                    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
                    ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", course.DepartmentId);
                    ViewBag.CourseId = new SelectList(db.Courses.Where(c => c.DepartmentId == course.DepartmentId), "CourseId", "Code", roomallocation.CourseId);
                    ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name", roomallocation.RoomId);
                    ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", roomallocation.DayId);
                    return View(roomallocation);
                }
                //}

                return RedirectToAction("Create");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code", roomallocation.DepartmentId);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", roomallocation.CourseId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Name", roomallocation.RoomId);
            ViewBag.DayId = new SelectList(db.Days, "DayId", "Name", roomallocation.DayId);
            return View(roomallocation);
        }

        private int ConvertTimetoInt(string timeStr)
        {
            try
            {
                if (TimeFormateCheck(timeStr))
                {
                    if (timeStr.Length == 4)
                    {
                        timeStr = "0" + timeStr;
                    }
                    string hour = timeStr[0].ToString() + timeStr[1];
                    string minute = timeStr[3].ToString() + timeStr[4];

                    if (Convert.ToInt32(minute) > 59 || Convert.ToInt32(minute) < 0)
                    {
                        TempData["ErrorMessage3"] = "Minites Must Be Equal Or Less Then 59";
                        throw new Exception();
                    }

                    int time = (Convert.ToInt32(hour) * 60);
                    time += Convert.ToInt32(minute);
                    return time;
                }
                else
                {
                    throw new Exception("24 hours--format HH:MM");
                }
            }

            catch (FormatException aFormatException)
            {
                throw new FormatException(
                    "24 hours--format HH:MM", aFormatException);
            }

            catch (IndexOutOfRangeException aRangException)
            {
                throw new IndexOutOfRangeException(
                    "24 hours--format HH:MM", aRangException);
            }

            catch (Exception anException)
            {
                throw new Exception("24 hours--format HH:MM", anException);
            }
        }

        private bool TimeFormateCheck(string timeStr)
        {
            char[] list = timeStr.ToCharArray();
            foreach (var aChar in list)
            {
                if (aChar == ':')
                {
                    return true;
                }
            }
            return false;
        }

        public ActionResult RoomAllocationView()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "Code");
            List<Course> CourseList = db.Courses.ToList();

            foreach (var aCourse in CourseList)
            {
                aCourse.RoomAllocationList
                    = db.RoomAllocations.Include(a => a.Room).Include(a => a.Day)
                    .Where(a => a.CourseId == aCourse.CourseId  && a.Status.Equals(true)).ToList();
            }

            return View(CourseList);
        }
        
      

        public ActionResult LoadCourse(int? departmentId)
        {
            var courseList = db.Courses.Where(u => u.DepartmentId == departmentId).ToList();
            ViewBag.CourseId = new SelectList(courseList, "CourseId", "Name");
            return PartialView("~/Views/shared/_FilteredCourse.cshtml");

        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public PartialViewResult AllocatedRoomLoad(int? departmentId)
        {
            List<Course> courseList = null;
            if (departmentId != null)
            {
                courseList = db.Courses.Where(c => c.DepartmentId == departmentId).ToList();
            }
            //else
            //{
            //    CourseList = db.Courses.ToList();
            //}

            foreach (var aCourse in courseList)
            {
                aCourse.RoomAllocationList
                    = db.RoomAllocations.Include(a => a.Room).Include(a => a.Day)
                    .Where(a => a.CourseId == aCourse.CourseId  && a.Status.Equals(true)).ToList();
            }
            if (courseList.Count == 0)
            {
                ViewBag.NoCourse = "Department Empty";
            }
            return PartialView("~/Views/shared/_RoomAllocationView.cshtml", courseList);
        }

    }
}
