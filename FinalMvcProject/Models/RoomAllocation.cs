using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalMvcProject.Models
{
    public class RoomAllocation
    {
        public int Id { set; get; }

        [Required(ErrorMessage = "Please Enter a Department")]
        public int DepartmentId { set; get; }
        public virtual Department Department { set; get; }

        [Required(ErrorMessage = "Please Enter a Couse")]
        public int CourseId { set; get; }
        public virtual Course Course { set; get; }

        [Required(ErrorMessage = "Please Enter a Room")]
        public int RoomId { set; get; }
        public virtual Room Room { set; get; }

        [Required(ErrorMessage = "Please Enter a Day")]
        public int DayId { set; get; }
        public virtual Day Day { set; get; }

        [Display(Name = "From")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string From { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string To { get; set; }

        public bool Status { get; set; }
    }
}