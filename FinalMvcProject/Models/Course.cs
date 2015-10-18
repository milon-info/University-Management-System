using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalMvcProject.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Please Enter the Code")]
        [Display(Name = "Code")]
        [StringLength(20, MinimumLength = 5)]
        [Remote("IsUserCodeAvailable", "Course")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Please Enter the Name")]
        [Display(Name = "Name")]
        [Remote("IsUserNameAvailable", "Course")]
        public string Name { get; set; }

        [Display(Name = "Credit")]
        [Range(0.5, 5.0)]
        public double Credit { get; set; }
         [Required, DisplayName("Description"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public int SemesterId { get; set; }
        public virtual Semester Semester { get; set; }
        public List<RoomAllocation> RoomAllocationList { get; set; }
        public string AssignTo { get; set; }
        public virtual Semester CourseSemister { get; set; }
        public virtual ICollection<Teacher> CourseTeacher { get; set; }
        public bool Status { get; set; }
        public string SemName { get; set; }
    }
}