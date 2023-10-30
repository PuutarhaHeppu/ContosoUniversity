using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public static class Dbinitializer
    {

        public static void initialize(SchoolContext context)
        {
            //otsib õpilasi
            if (context.Students.Any())
            {
                return; //väljub meetodist kui andmebaas sisaldab juba andmeid ning meetodis kirjeldatud näidisõpilasi, kursuseid ja aineoalusi ei lisata
            }

            var students = new Student[]
            {
                new Student {FirstMidName="Marken", LastName="Lemming",EnrollmentDate=DateTime.Parse("2022-09-01")},
                new Student {FirstMidName="Maw-Of", LastName="malmortius",EnrollmentDate=DateTime.Parse("2022-09-01")},
                new Student {FirstMidName="Marekk", LastName="Lemmingson",EnrollmentDate=DateTime.Parse("2022-09-01")},
                new Student {FirstMidName="Maarek", LastName="Lemminkäinen",EnrollmentDate=DateTime.Parse("2022-09-01")},
                new Student {FirstMidName="Mairo", LastName="Lemminen",EnrollmentDate=DateTime.Parse("2022-09-01")},
                new Student {FirstMidName="Martturi", LastName="Lerissaar",EnrollmentDate=DateTime.Now}
            };
            foreach (Student s in students)
            {
                context.Students.Add(s);
            }
            var Instructors = new Instructor[]
            {
                new Instructor {FirstMidName="Kim", LastName="Kardashian", HireDate=DateTime.Parse("2005-09-01") },
                new Instructor {FirstMidName="Mr.", LastName="Bean", HireDate=DateTime.Parse("2005-09-01") },
                new Instructor {FirstMidName="Kiur", LastName="Arbeiter", HireDate=DateTime.Parse("2005-09-01") },
                new Instructor {FirstMidName="Artur", LastName="Erissaar", HireDate=DateTime.Parse("2005-09-01") },
                new Instructor {FirstMidName="Kohalik", LastName="Parm", HireDate=DateTime.Parse("2005-09-01") },
            };
            foreach (Instructor i in Instructors)
            {
                context.Instructors.Add(i);
            }
            context.SaveChanges();

            var departments = new Department[]
            {
                new Department {DepartmentID = 1, Name = "IT", Budget = 100, StartDate = DateTime.Parse("2022-09-01"), InstructorID=Instructors.Single(i => i.LastName == "Kardashian").ID },
                new Department {DepartmentID = 2, Name = "IT", Budget = 100, StartDate = DateTime.Parse("2022-09-01"), InstructorID=Instructors.Single(i => i.LastName == "Parm").ID },
                new Department {DepartmentID = 3, Name = "IT", Budget = 100, StartDate = DateTime.Parse("2022-09-01"), InstructorID=Instructors.Single(i => i.LastName == "Erissaar").ID },
                new Department {DepartmentID = 4, Name = "IT", Budget = 100, StartDate = DateTime.Parse("2022-09-01"), InstructorID=Instructors.Single(i => i.LastName == "Arbeiter").ID },
            };
            foreach (Department d in departments)
            {
                context.Departments.Add(d);
            }
            context.SaveChanges();
            var Courses = new Course[]
{
                new Course {CourseID = 1001, Title="Programming", Credits=3, DepartmentID=departments.Single(s => s.Name == "Infotechnology").DepartmentID },
                new Course {CourseID = 2221, Title="Databases 101", Credits=3, DepartmentID=departments.Single(s => s.Name == "Infotechnology").DepartmentID },
                new Course {CourseID = 1001, Title="html stuff", Credits=3, DepartmentID=departments.Single(s => s.Name == "Infotechnology").DepartmentID },
                new Course {CourseID = 6543, Title="Cupcakes", Credits=3, DepartmentID=departments.Single(s => s.Name == "home Economics").DepartmentID },
                new Course {CourseID = 4298, Title="chocolate tempering", Credits=3, DepartmentID=departments.Single(s => s.Name == "Home Economics").DepartmentID },
};
            foreach (Course c in Courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            var officeAssignment = new OfficeAssignment[]
            {
                new OfficeAssignment
                {
                    InstructorID = Instructors.Single(i=>i.LastName == "Kardashian").ID, Location="Classroom D428"
                },
                new OfficeAssignment
                {
                    InstructorID = Instructors.Single(i=>i.LastName == "Arbeiter").ID, Location="Classroom A236"
                },
                new OfficeAssignment
                {
                    InstructorID = Instructors.Single(i=>i.LastName == "Parm").ID, Location="Classroom Baltijaam"
                },
                new OfficeAssignment
                {
                    InstructorID = Instructors.Single(i=>i.LastName == "Erissaar").ID, Location="Classroom C122"
                },
            };
            foreach (OfficeAssignment o in OfficeAssignments)
            {
                context.OfficeAssignments.Add(o);
            }
            context.SaveChanges();

            var courseInstructors = new CourseAssignment[]
            {
                new CourseAssignment
                {
                    CourseID = Courses.Single(c=>c.Title == "Programming").CourseID, InstructorID = Instructors.Single(i => i.LastName == "Kardashian").ID,
                },
                new CourseAssignment
                {
                    CourseID = Courses.Single(c=>c.Title == "Databases 101").CourseID, InstructorID = Instructors.Single(i => i.LastName == "Kardashian").ID,
                },
                new CourseAssignment
                {
                    CourseID = Courses.Single(c=>c.Title == "html stuff").CourseID, InstructorID = Instructors.Single(i => i.LastName == "Kardashian").ID,
                },
                new CourseAssignment
                {
                    CourseID = Courses.Single(c=>c.Title == "Cupcakes").CourseID, InstructorID = Instructors.Single(i => i.LastName == "Kardashian").ID,
                },
                new CourseAssignment
                {
                    CourseID = Courses.Single(c=>c.Title == "Chocolate tempering").CourseID, InstructorID = Instructors.Single(i => i.LastName == "Kardashian").ID,
                }
            };
            foreach (CourseAssignment ci in courseInstructors)
            {
                context.CourseAssignment.Add(ci);
            }
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Lemming").ID,
                    CourseID = Courses.Single(c => c.Title == "Programming").CourseID,
                    Grade = Grade.A
                },
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Malmortius").ID,
                    CourseID = Courses.Single(c => c.Title == "Programming").CourseID,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Lemmingson").ID,
                    CourseID = Courses.Single(c => c.Title == "Programming").CourseID,
                    Grade = Grade.B
                },
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Lemminkäinen").ID,
                    CourseID = Courses.Single(c => c.Title == "Programming").CourseID,
                    Grade = Grade.F
                },
                new Enrollment
                {
                    StudentID = students.Single(s => s.LastName == "Lemminen").ID,
                    CourseID = Courses.Single(c => c.Title == "Programming").CourseID,
                    Grade = Grade.F
                }
            };
            foreach (Enrollment e in enrollments)
            {
                var enrollmentDataBase = context.Enrollments.Where(
                    s =>
                    s.Student.ID == s.StudentID &&
                    s.Course.CourseID == e.CourseID)
                    .SingleOrDefault();
                if (enrollmentDataBase == null)
                {
                    context.Enrollments.Add(e);
                }
            };
            context.SaveChanges();
        }
    }
}