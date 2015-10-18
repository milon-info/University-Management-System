using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalMvcProject.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department ADepartment { get; set; }

        public int StudentId { get; set; }
        public virtual Student AStudent { get; set; }

        public DateTime DateTime { get; set; }
        public String Result { get; set; }
    }
}