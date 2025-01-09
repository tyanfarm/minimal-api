using SimpleMinimalAPI.Models;

namespace SimpleMinimalAPI.Config
{
    public static class StudentCollection
    {
        public static List<Student> Students = new List<Student>();

        public static void Init()
        {
            Students.Add(new Student
            {
                Id = 1,
                Name = "Alice",
                Course = "Math",
                Points = 10,
                DateOfBirth = DateTime.Parse("2003-01-21")
            });

            Students.Add(new Student
            {
                Id = 2,
                Name = "Bob",
                Course = "Physics",
                Points = 9,
                DateOfBirth = DateTime.Parse("2002-12-11")
            });

            Students.Add(new Student
            {
                Id = 3,
                Name = "Charlie",
                Course = "Chemistry",
                Points = 8,
                DateOfBirth = DateTime.Parse("2004-03-15")
            });

            Students.Add(new Student
            {
                Id = 4,
                Name = "David",
                Course = "Biology",
                Points = 7,
                DateOfBirth = DateTime.Parse("2001-05-25")
            });

            Students.Add(new Student
            {
                Id = 5,
                Name = "Ella",
                Course = "History",
                Points = 6,
                DateOfBirth = DateTime.Parse("2003-08-02")
            });

            Students.Add(new Student
            {
                Id = 6,
                Name = "Frank",
                Course = "English",
                Points = 9,
                DateOfBirth = DateTime.Parse("2002-07-20")
            });

            Students.Add(new Student
            {
                Id = 7,
                Name = "Grace",
                Course = "Geography",
                Points = 10,
                DateOfBirth = DateTime.Parse("2004-04-18")
            });

            Students.Add(new Student
            {
                Id = 8,
                Name = "Henry",
                Course = "Computer Science",
                Points = 8,
                DateOfBirth = DateTime.Parse("2001-10-09")
            });

            Students.Add(new Student
            {
                Id = 9,
                Name = "Ivy",
                Course = "Art",
                Points = 7,
                DateOfBirth = DateTime.Parse("2003-11-12")
            });

            Students.Add(new Student
            {
                Id = 10,
                Name = "Jack",
                Course = "Economics",
                Points = 9,
                DateOfBirth = DateTime.Parse("2002-09-30")
            });
        }

        public static void AddNewStudent(Student student)
        {
            if (Students.Count > 0)
            {
                var latestId = Students.Max(x => x.Id);

                Students.Add(new Student
                {
                    Id = latestId + 1,
                    Name = student.Name,
                    Course = student.Course,
                    Points = student.Points,
                    DateOfBirth = student.DateOfBirth
                });
            }
            else
            {
                Students.Add(new Student
                {
                    Id = 1,
                    Name = student.Name,
                    Course = student.Course,
                    Points = student.Points,
                    DateOfBirth = student.DateOfBirth
                });
            }
        }
    }
}
