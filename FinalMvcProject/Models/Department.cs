using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Text;

namespace FinalMvcProject.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        [Required(ErrorMessage = "Please Enter the Code")]
        [Display(Name = "Department Code")]
        [StringLength(7, MinimumLength = 2)]
        [Remote("IsUserCodeAvailable", "Department")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Please Enter the Name")]
        [Display(Name = "Department Name")]
        [Remote("IsUserNameAvailable", "Department")]
        public string Name { get; set; }

        public virtual ICollection<Course> Courses { get; set; } 
    }
}