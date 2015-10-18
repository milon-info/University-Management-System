using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalMvcProject.Models
{
    public class EnrollCourse
    {
        public int EnrollCourseId { set; get; }

        [Required(ErrorMessage = "Please Enter the Student")]
        public int StudentId { set; get; }
        public virtual Student Student { set; get; }

        [Required(ErrorMessage = "Pleae Enter The course")]
        public int CourseId { set; get; }
        public virtual Course Course { set; get; }

        [Required(ErrorMessage = "Please Enter the date")]
        [DataType(DataType.Date)]
        public DateTime EnrollDate { set; get; }

        public virtual string GradeName { set; get; }
    }
}