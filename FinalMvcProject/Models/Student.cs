using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalMvcProject.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [DisplayName("Student Name"), Required]
        public string Name { get; set; }

        [Required, EmailAddress(ErrorMessage = "Invalid email Address")]
        [Remote("IsEmailUnique", "Student")]
        public string Email { get; set; }

        [Required, DisplayName("Contact No")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?[+]([8]{2})([0]{1})([1]{1})([1-9]{1})([0-9]{8})$", ErrorMessage = "Invalid Phone number. Try this format (+88017XXXXXXXX)")]
        public string ContactNo { get; set; }

        [Required, DisplayName("Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required, DisplayName("Address"), DataType(DataType.MultilineText)]
        public string Address { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public virtual string RegistrationId { get; set; }
    }
}