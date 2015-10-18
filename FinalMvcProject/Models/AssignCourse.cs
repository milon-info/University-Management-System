using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalMvcProject.Models
{
    public class AssignCourse
    {
        public int AssignCourseId { set; get; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public int TeacherId { set; get; }
        public virtual Teacher Teacher { set; get; }

        public virtual double CreditTaken { set; get; }

        public virtual double RemaingCredit { set; get; }

        public int CourseId { set; get; }
        public virtual Course Course { set; get; }
        public bool Status { get; set; }
    }
}