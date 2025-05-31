using EFCore;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        using var context = new AppDbContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        SeedData(context);
        RunCrudMenu(context);
    }

    static void SeedData(AppDbContext context)
    {
        var student1 = new Student
        {
            Name = "Ali Elyas",
            Courses = new List<Course>
            {
                new Course { Title = "Math" },
                new Course { Title = "Programming" }
            }
        };

        var student2 = new Student
        {
            Name = "Sara",
            Courses = new List<Course>
            {
                new Course { Title = "Biology" }
            }
        };

        var student3 = new Student
        {
            Name = "Zainab"
        };

        context.Students.AddRange(student1, student2, student3);
        context.SaveChanges();
    }

    static void RunCrudMenu(AppDbContext context)
    {
        while (true)
        {
            Console.WriteLine("\n=== EF Core CRUD Menu ===");
            Console.WriteLine("1. Show All Students");
            Console.WriteLine("2. Add New Student");
            Console.WriteLine("3. Update Student Name");
            Console.WriteLine("4. Delete Student");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowAllStudents(context);
                    break;
                case "2":
                    AddStudent(context);
                    break;
                case "3":
                    UpdateStudent(context);
                    break;
                case "4":
                    DeleteStudent(context);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine(" Invalid choice.");
                    break;
            }
        }
    }

    static void ShowAllStudents(AppDbContext context)
    {
        var students = context.Students.Include(s => s.Courses).ToList();
        Console.WriteLine("\n Student List:");
        foreach (var student in students)
        {
            Console.WriteLine($" {student.Name}");
            foreach (var course in student.Courses)
                Console.WriteLine($"    {course.Title}");
        }
    }

    static void AddStudent(AppDbContext context)
    {
        Console.Write("\nEnter student name: ");
        var name = Console.ReadLine();

        Console.Write("Enter number of courses: ");
        var count = int.Parse(Console.ReadLine());

        var courses = new List<Course>();
        for (int i = 0; i < count; i++)
        {
            Console.Write($"Course {i + 1} title: ");
            var title = Console.ReadLine();
            courses.Add(new Course { Title = title });
        }

        var newStudent = new Student
        {
            Name = name,
            Courses = courses
        };

        context.Students.Add(newStudent);
        context.SaveChanges();
        Console.WriteLine(" Student added.");
    }

    static void UpdateStudent(AppDbContext context)
    {
        Console.Write("\nEnter student name to update: ");
        var name = Console.ReadLine();

        var student = context.Students.FirstOrDefault(s => s.Name == name);
        if (student == null)
        {
            Console.WriteLine(" Student not found.");
            return;
        }

        Console.Write("Enter new name: ");
        var newName = Console.ReadLine();

        student.Name = newName;
        context.SaveChanges();
        Console.WriteLine("Student name updated.");
    }

    static void DeleteStudent(AppDbContext context)
    {
        Console.Write("\nEnter student name to delete: ");
        var name = Console.ReadLine();

        var student = context.Students.Include(s => s.Courses).FirstOrDefault(s => s.Name == name);
        if (student == null)
        {
            Console.WriteLine(" Student not found.");
            return;
        }

        context.Courses.RemoveRange(student.Courses); // Remove related courses first
        context.Students.Remove(student);
        context.SaveChanges();
        Console.WriteLine("Student deleted.");
    }
}
