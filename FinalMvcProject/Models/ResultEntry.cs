using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalMvcProject.Models
{
    public class ResultEntry
    {
        public int ResultEntryId { set; get; }

        [Required(ErrorMessage = "Please Enter a student")]
        public int StudentId { set; get; }
        public virtual Student Student { set; get; }

        [Required(ErrorMessage = "Please Enter a cousre")]
        public int CourseId { set; get; }
        public virtual Course Course { set; get; }

        [Required(ErrorMessage = "Please Enter a grade")]
        public int GradeId { set; get; }
        public virtual Grade Grade { set; get; }
    }
}