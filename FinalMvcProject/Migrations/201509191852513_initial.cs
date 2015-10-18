namespace FinalMvcProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssignCourses",
                c => new
                    {
                        AssignCourseId = c.Int(nullable: false, identity: true),
                        DepartmentId = c.Int(nullable: false),
                        TeacherId = c.Int(nullable: false),
                        CreditTaken = c.Double(nullable: false),
                        RemaingCredit = c.Double(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AssignCourseId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: false)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: false)
                .ForeignKey("dbo.Teachers", t => t.TeacherId, cascadeDelete: false)
                .Index(t => t.DepartmentId)
                .Index(t => t.TeacherId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false),
                        Credit = c.Double(nullable: false),
                        Description = c.String(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        SemesterId = c.Int(nullable: false),
                        AssignTo = c.String(),
                        Semester_SemesterId = c.Int(),
                        CourseSemister_SemesterId = c.Int(),
                        Semester_SemesterId1 = c.Int(),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.Semesters", t => t.Semester_SemesterId)
                .ForeignKey("dbo.Semesters", t => t.CourseSemister_SemesterId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: false)
                .ForeignKey("dbo.Semesters", t => t.Semester_SemesterId1)
                .Index(t => t.DepartmentId)
                .Index(t => t.Semester_SemesterId)
                .Index(t => t.CourseSemister_SemesterId)
                .Index(t => t.Semester_SemesterId1);
            
            CreateTable(
                "dbo.Semesters",
                c => new
                    {
                        SemesterId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.SemesterId);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        TeacherId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Email = c.String(),
                        ContactNo = c.String(nullable: false),
                        DesignationId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        CreditTaken = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TeacherId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: false)
                .ForeignKey("dbo.Designations", t => t.DesignationId, cascadeDelete: false)
                .Index(t => t.DesignationId)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 7),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Designations",
                c => new
                    {
                        DesignationId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.DesignationId);
            
            CreateTable(
                "dbo.RoomAllocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DepartmentId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        RoomId = c.Int(nullable: false),
                        DayId = c.Int(nullable: false),
                        From = c.String(),
                        To = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: false)
                .ForeignKey("dbo.Days", t => t.DayId, cascadeDelete: false)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: false)
                .ForeignKey("dbo.Rooms", t => t.RoomId, cascadeDelete: false)
                .Index(t => t.DepartmentId)
                .Index(t => t.CourseId)
                .Index(t => t.RoomId)
                .Index(t => t.DayId);
            
            CreateTable(
                "dbo.Days",
                c => new
                    {
                        DayId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.DayId);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.RoomId);
            
            CreateTable(
                "dbo.EnrollCourses",
                c => new
                    {
                        EnrollCourseId = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        EnrollDate = c.DateTime(nullable: false),
                        GradeName = c.String(),
                    })
                .PrimaryKey(t => t.EnrollCourseId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: false)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: false)
                .Index(t => t.StudentId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        ContactNo = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Address = c.String(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        RegistrationId = c.String(),
                    })
                .PrimaryKey(t => t.StudentId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: false)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Enrollments",
                c => new
                    {
                        EnrollmentId = c.Int(nullable: false, identity: true),
                        DepartmentId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        Result = c.String(),
                    })
                .PrimaryKey(t => t.EnrollmentId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: false)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: false)
                .Index(t => t.DepartmentId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        GradeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.GradeId);
            
            CreateTable(
                "dbo.ResultEntries",
                c => new
                    {
                        ResultEntryId = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        GradeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResultEntryId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: false)
                .ForeignKey("dbo.Grades", t => t.GradeId, cascadeDelete: false)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: false)
                .Index(t => t.StudentId)
                .Index(t => t.CourseId)
                .Index(t => t.GradeId);
            
            CreateTable(
                "dbo.TeacherCourses",
                c => new
                    {
                        Teacher_TeacherId = c.Int(nullable: false),
                        Course_CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Teacher_TeacherId, t.Course_CourseId })
                .ForeignKey("dbo.Teachers", t => t.Teacher_TeacherId, cascadeDelete: false)
                .ForeignKey("dbo.Courses", t => t.Course_CourseId, cascadeDelete: false)
                .Index(t => t.Teacher_TeacherId)
                .Index(t => t.Course_CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResultEntries", "StudentId", "dbo.Students");
            DropForeignKey("dbo.ResultEntries", "GradeId", "dbo.Grades");
            DropForeignKey("dbo.ResultEntries", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Enrollments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Enrollments", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.EnrollCourses", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Students", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.EnrollCourses", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.AssignCourses", "TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.AssignCourses", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.AssignCourses", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Courses", "Semester_SemesterId1", "dbo.Semesters");
            DropForeignKey("dbo.RoomAllocations", "RoomId", "dbo.Rooms");
            DropForeignKey("dbo.RoomAllocations", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.RoomAllocations", "DayId", "dbo.Days");
            DropForeignKey("dbo.RoomAllocations", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.TeacherCourses", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.TeacherCourses", "Teacher_TeacherId", "dbo.Teachers");
            DropForeignKey("dbo.Teachers", "DesignationId", "dbo.Designations");
            DropForeignKey("dbo.Teachers", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Courses", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Courses", "CourseSemister_SemesterId", "dbo.Semesters");
            DropForeignKey("dbo.Courses", "Semester_SemesterId", "dbo.Semesters");
            DropIndex("dbo.TeacherCourses", new[] { "Course_CourseId" });
            DropIndex("dbo.TeacherCourses", new[] { "Teacher_TeacherId" });
            DropIndex("dbo.ResultEntries", new[] { "GradeId" });
            DropIndex("dbo.ResultEntries", new[] { "CourseId" });
            DropIndex("dbo.ResultEntries", new[] { "StudentId" });
            DropIndex("dbo.Enrollments", new[] { "StudentId" });
            DropIndex("dbo.Enrollments", new[] { "DepartmentId" });
            DropIndex("dbo.Students", new[] { "DepartmentId" });
            DropIndex("dbo.EnrollCourses", new[] { "CourseId" });
            DropIndex("dbo.EnrollCourses", new[] { "StudentId" });
            DropIndex("dbo.RoomAllocations", new[] { "DayId" });
            DropIndex("dbo.RoomAllocations", new[] { "RoomId" });
            DropIndex("dbo.RoomAllocations", new[] { "CourseId" });
            DropIndex("dbo.RoomAllocations", new[] { "DepartmentId" });
            DropIndex("dbo.Teachers", new[] { "DepartmentId" });
            DropIndex("dbo.Teachers", new[] { "DesignationId" });
            DropIndex("dbo.Courses", new[] { "Semester_SemesterId1" });
            DropIndex("dbo.Courses", new[] { "CourseSemister_SemesterId" });
            DropIndex("dbo.Courses", new[] { "Semester_SemesterId" });
            DropIndex("dbo.Courses", new[] { "DepartmentId" });
            DropIndex("dbo.AssignCourses", new[] { "CourseId" });
            DropIndex("dbo.AssignCourses", new[] { "TeacherId" });
            DropIndex("dbo.AssignCourses", new[] { "DepartmentId" });
            DropTable("dbo.TeacherCourses");
            DropTable("dbo.ResultEntries");
            DropTable("dbo.Grades");
            DropTable("dbo.Enrollments");
            DropTable("dbo.Students");
            DropTable("dbo.EnrollCourses");
            DropTable("dbo.Rooms");
            DropTable("dbo.Days");
            DropTable("dbo.RoomAllocations");
            DropTable("dbo.Designations");
            DropTable("dbo.Departments");
            DropTable("dbo.Teachers");
            DropTable("dbo.Semesters");
            DropTable("dbo.Courses");
            DropTable("dbo.AssignCourses");
        }
    }
}
