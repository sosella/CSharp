using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace CSharp.TestSuites
{
    public class Course
    {
        [Key]
        public int ID { get; set; }

        public string Title { get; set; }

        [Required]
        [Range(0, 5)]
        public int Credits { get; set; }

        [Required]
        [ForeignKey("DepartmentID")]
        public int DepartmentID { get; set; }
        public Department Department { get; set; }
    }

    public class CourseAssignment
    {
        [ForeignKey("InstructorID")]
        public int InstructorID { get; set; }
        public Instructor Instructor { get; set; }

        [ForeignKey("CourseID")]
        public int CourseID { get; set; }
        public Course Course { get; set; }
    }

    public enum Grade
    {
        A, B, C, D, F
    }

    public class Department
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        [Required]
        public decimal Budget { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [ForeignKey("InstructorID")]
        public int InstructorID { get; set; }
        public Instructor Administrator { get; set; }

        public byte[] RowVersion { get; set; }
    }

    public class Enrollment
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("CourseID")]
        public int CourseID { get; set; }
        public Course Course { get; set; }

        [ForeignKey("StudentID")]
        public int StudentID { get; set; }
        public Student Student { get; set; }

        public Grade? Grade { get; set; }
    }

    public class Instructor
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        public string FullName
        {
            get { return LastName + ", " + FirstName; }
        }
    }

    public class OfficeAssignment
    {
        [Key]
        [ForeignKey("InstructorID")]
        public int InstructorID { get; set; }
        public Instructor Instructor { get; set; }

        public string Location { get; set; }
    }

    public class Student
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }
    }

    public class UniversityDbContext : DbContext
    {
        private static readonly string kUniversityConnectionString = @"Server=.\SQLEXPRESS;Database=ContosoUniversity;Trusted_Connection=True;";

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<CourseAssignment> CourseAssignments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(kUniversityConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment");
            modelBuilder.Entity<CourseAssignment>()
                .HasKey(ca => new { ca.InstructorID, ca.CourseID });
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Instructor>().ToTable("Instructor");
            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment");
            modelBuilder.Entity<Student>().ToTable("Student");
        }

        public Student FindOrAddStudent(string firstName, string lastName)
        {
            Student student =
                (from s
                 in Students
                 where s.FirstName.Equals(firstName) && s.LastName.Equals(lastName) 
                 select s).SingleOrDefault();
            if (student == null)
            {
                student = new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EnrollmentDate = DateTime.Now
                };
                Students.Add(student);
                SaveChanges();
            }
            return student;
        }

        public void RemoveStudent(Student nicholas)
        {
            Students.Remove(nicholas);
            SaveChanges();
        }

        public Course FindOrAddCourse(string title, int credits)
        {
            Course course =
                (from c
                 in Courses
                 where c.Title.Equals(title)
                 select c).SingleOrDefault();
            if (course == null)
            {
                Department department = RandomlySelectDepartment();
                course = new Course
                {
                    Title = title,
                    Credits = credits,
                    DepartmentID = department.ID, 
                    Department = department
                };
                Courses.Add(course);
                SaveChanges();
            }
            return course;
        }

        public Enrollment FindOrAddEnrollment(Student student, Course course)
        {
            Enrollment enrollment =
                (from e
                 in Enrollments
                 where e.StudentID.Equals(student.ID) && e.CourseID.Equals(course.ID)
                 select e).SingleOrDefault();
            if (enrollment == null)
            {
                enrollment = new Enrollment
                {
                    StudentID = student.ID,
                    CourseID = course.ID
                };
                Enrollments.Add(enrollment);
                SaveChanges();
            }
            return enrollment;
        }

        private Department RandomlySelectDepartment()
        {
            List<Department> departments = Departments.ToList();
            Random rnd = new Random();
            return departments[rnd.Next(departments.Count)];
        }
    }

    public class EntityFrameworkTestSuite : TestSuite, ITestSuite
    {
        public void Run()
        {
            WriteTestSuiteName();

            EntityFramework1();
            EntityFramework2();
            EntityFramework3();
            EntityFramework4();
            EntityFramework5();

            WriteTestSuiteName();
        }

        private void EntityFramework1()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            using (UniversityDbContext ctx = new UniversityDbContext())
            {
                Student stephen = ctx.FindOrAddStudent("Stephen", "Osella");
                Student alexander = ctx.FindOrAddStudent("Alexander", "Osella");
                Student nicholas = ctx.FindOrAddStudent("Nicholas", "Osella");
                WriteStudents(ctx.Students.ToList());

                ctx.RemoveStudent(nicholas);
                WriteStudents(ctx.Students);
            }
        }

        private void EntityFramework2()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            using (UniversityDbContext ctx = new UniversityDbContext())
            {
                WriteStudents(ctx.Students);
            }
        }

        private void EntityFramework3()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            using (UniversityDbContext ctx = new UniversityDbContext())
            {
                Course calculus = ctx.FindOrAddCourse("Calculus", 3);
                Course trigonometry = ctx.FindOrAddCourse("Trigonometry", 3);
                Course englishI = ctx.FindOrAddCourse("English I", 3);
                Course englishII = ctx.FindOrAddCourse("English II", 3);
                WriteCourses(ctx.Courses);
            }
        }

        private void EntityFramework4()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);

            using (UniversityDbContext ctx = new UniversityDbContext())
            {
                Student stephen = ctx.Students.FirstOrDefault(s => s.FirstName.Equals("Stephen") && s.LastName.Equals("Osella"));
                if (stephen != null)
                {
                    Course calculus = ctx.Courses.FirstOrDefault(c => c.Title.Equals("Calculus"));
                    if (calculus != null)
                    {
                        ctx.FindOrAddEnrollment(stephen, calculus);
                    }
                    Course englishI = ctx.Courses.FirstOrDefault(c => c.Title.Equals("English I"));
                    if (englishI != null)
                    {
                        ctx.FindOrAddEnrollment(stephen, englishI);
                    }
                }
                var enrollments = ctx.Enrollments
                    .Where(e => e.StudentID == stephen.ID)
                    .Include(e => e.Course)
                    .Include(e => e.Student);
                WriteEnrollments(enrollments);
            }
        }

        private void EntityFramework5()
        {
            WriteMethodName(MethodBase.GetCurrentMethod().Name);
            using (UniversityDbContext ctx = new UniversityDbContext())
            {
                var enrollments = ctx.Enrollments
                    .Include(e => e.Course)
                    .Include(e => e.Student);
                WriteEnrollments(enrollments);
            }
        }

        public static void WriteStudents(IEnumerable<Student> students)
        {
            Console.WriteLine("Students");
            foreach (Student student in students)
            {
                WriteStudent(student);
            }
        }

        private static void WriteStudent(Student student)
        {
            Console.WriteLine($"{student.FirstName} {student.LastName}");
        }

        private static void WriteCourses(IEnumerable<Course> courses)
        {
            Console.WriteLine("Courses");
            foreach (Course course in courses)
            {
                WriteCourse(course);
            }
        }

        private static void WriteCourse(Course course)
        {
            Console.WriteLine($"Title={course.Title} Credits={course.Credits}");
        }

        private static void WriteEnrollments(IEnumerable<Enrollment> enrollments)
        {
            Console.WriteLine("Enrollments");
            foreach (Enrollment enrollment in enrollments)
            {
                WriteEnrollment(enrollment);
            }
        }

        private static void WriteEnrollment(Enrollment enrollment)
        {
            Console.WriteLine($"{enrollment.Student.FirstName} {enrollment.Student.LastName} - {enrollment.Course.Title}");
        }
    }
}
