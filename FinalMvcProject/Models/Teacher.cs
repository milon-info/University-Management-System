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
    public class Teacher
    {
        //public Teacher()
        //{
        //    this.TeachersCourses = new Collection<Course>();
        //}

        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Please Enter The Name.")]
        public string Name { get; set; }
        [Required, DisplayName("Address"), DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email address")]
        [Remote("IsUserEmailUnique", "Teacher")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your Contact No"), DisplayName("Contact No")]
        [RegularExpression(@"^\(?[+]([8]{2})([0]{1})([1]{1})([1-9]{1})([0-9]{8})$",
           ErrorMessage = "Invalid Phone number. Try this format (+88017XXXXXXXX)")]
        public string ContactNo { get; set; }

        public int DesignationId { get; set; }
        public virtual Designation Designation { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [Required, DisplayName("Cradit To Be Taken")]
        [Range(5, 25, ErrorMessage = "Invalid Cradit")]
        public double CreditTaken { get; set; }

        public virtual ICollection<Course> TeachersCourses { get; set; }
       
    }
}