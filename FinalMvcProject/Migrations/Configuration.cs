using System.Collections.Generic;
using FinalMvcProject.Models;

namespace FinalMvcProject.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FinalMvcProject.Models.UniversityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FinalMvcProject.Models.UniversityDbContext context)
        {
            var semester = new List<Semester>
            {
                new Semester() {Name = "Spring13"},
                new Semester() {Name = "Summer13"},
                new Semester() {Name = "Fall13"},
                new Semester() {Name = "Spring14"},
                new Semester() {Name = "Summer14"},
                new Semester() {Name = "Fall14"},
                new Semester() {Name = "Spring15"},
                new Semester() {Name = "Summer15"},
                new Semester() {Name = "Fall15"},
            };
            semester.ForEach(a => context.Semesters.AddOrUpdate(b => b.Name, a));
            context.SaveChanges();

            var designation = new List<Designation>
            {
                new Designation() {Name = "Professor"},
                new Designation() {Name = "Assiestent Professor"},
                new Designation(){Name = "Associate Professor"},
                new Designation(){Name = "Lecturer"}

            };
            designation.ForEach(b => context.Designations.AddOrUpdate(a => a.Name, b));
            context.SaveChanges();

            var grade = new List<Grade>
            {
                new Grade() {Name = "A+"},
                new Grade() {Name = "A"},
                new Grade() {Name = "A-"},
                new Grade() {Name = "B+"},
                new Grade() {Name = "B"},
                new Grade() {Name = "B-"},
                new Grade() {Name = "C+"},
                new Grade() {Name = "C"},
                new Grade() {Name = "C-"},
                new Grade() {Name = "D+"},
                new Grade() {Name = "D"},
                new Grade() {Name = "D-"},
                new Grade() {Name = "F"}
            };
            grade.ForEach(c => context.Grades.AddOrUpdate(a => a.Name, c));
            context.SaveChanges();

            var day = new List<Day>
            {

                new Day() {Name = "Sunday"},
                new Day() {Name = "Monday"},
                new Day() {Name = "Tuesday"},
                new Day() {Name = "Wednesday"},
                new Day() {Name = "Thursday"},
                new Day() {Name = "Friday"},
                new Day() {Name = "Saturday"}

            };
            day.ForEach(d => context.Days.AddOrUpdate(a => a.Name, d));
            context.SaveChanges();

            var room = new List<Room>
            {
                new Room() {Name = "BITM-101"},
                new Room() {Name = "BITM-202"},
                new Room() {Name = "BITM-303"},
                new Room() {Name = "BITM-404"},
                new Room() {Name = "BITM-505"}
            };
            room.ForEach(c => context.Rooms.AddOrUpdate(a => a.Name, c));
            context.SaveChanges();
        }
    }
}
