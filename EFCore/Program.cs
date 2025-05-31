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
            Console.WriteLine("\n=== EF Core CRUD + LINQ Menu ===");
            Console.WriteLine("1. Show All Students");
            Console.WriteLine("2. Add New Student");
            Console.WriteLine("3. Update Student Name");
            Console.WriteLine("4. Delete Student");
            Console.WriteLine("5. LINQ: Count Students and Courses");
            Console.WriteLine("6. LINQ: Filter Students by Name");
            Console.WriteLine("7. LINQ: Sort Courses Alphabetically");
            Console.WriteLine("8. LINQ: Check if Any Student Has No Courses");
            Console.WriteLine("9. Exit");
            Console.WriteLine("10. LINQ Query Syntax: Filter Students by Name");
            Console.WriteLine("11. LINQ Query Syntax: Sort Courses Alphabetically");
            Console.WriteLine("12. LINQ Query Syntax: Check if Any Student Has No Courses");
            Console.WriteLine("13. LINQ Method Syntax: Filter Students by Name");
            Console.WriteLine("14. LINQ Method Syntax: Sort Courses Alphabetically");
            Console.WriteLine("15. LINQ Method Syntax: Check if Any Student Has No Courses");
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
                    LinqCountStudentsCourses(context);
                    break;
                case "6":
                    LinqFilterStudentsByName(context);
                    break;
                case "7":
                    LinqSortCourses(context);
                    break;
                case "8":
                    LinqAnyStudentNoCourses(context);
                    break;
                case "9":
                    return;
                case "10":
                    LinqQuerySyntax_FilterStudentsByName(context);
                    break;
                case "11":
                    LinqQuerySyntax_SortCourses(context);
                    break;
                case "12":
                    LinqQuerySyntax_AnyStudentNoCourses(context);
                    break;
                case "13":
                    LinqFilterStudentsByName_Method(context);
                    break;
                case "14":
                    LinqSortCourses_Method(context);
                    break;
                case "15":
                    LinqAnyStudentNoCourses_Method(context);
                    break;
                default:
                    Console.WriteLine(" Invalid choice.");
                    break;
            }
        }
    }

    // LINQ functions:

    static void LinqCountStudentsCourses(AppDbContext context)
    {
        int studentCount = context.Students.Count();
        int courseCount = context.Courses.Count();
        Console.WriteLine($"\n Total Students: {studentCount}");
        Console.WriteLine($" Total Courses: {courseCount}");
    }

    static void LinqFilterStudentsByName(AppDbContext context)
    {
        Console.Write("\nEnter partial name to filter students: ");
        string input = Console.ReadLine();

        var filtered = context.Students
            .Where(s => s.Name.Contains(input))
            .ToList();

        Console.WriteLine($"\n Students with '{input}' in their name:");
        if (filtered.Count == 0)
            Console.WriteLine("No matching students found.");
        else
            foreach (var s in filtered)
                Console.WriteLine($" {s.Name}");
    }

    static void LinqSortCourses(AppDbContext context)
    {
        var sortedCourses = context.Courses
            .OrderBy(c => c.Title)
            .ToList();

        Console.WriteLine("\n Courses sorted alphabetically:");
        foreach (var c in sortedCourses)
            Console.WriteLine($" {c.Title}");
    }

    static void LinqAnyStudentNoCourses(AppDbContext context)
    {
        bool anyNoCourses = context.Students
            .Include(s => s.Courses)
            .Any(s => s.Courses.Count == 0);

        Console.WriteLine($"\n Are there any students with no courses? {(anyNoCourses ? "Yes" : "No")}");
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

    static void LinqQuerySyntax_FilterStudentsByName(AppDbContext context)
    {
        Console.Write("\n[Query Syntax] Enter partial name to filter students: ");
        string input = Console.ReadLine();

        var filtered = (from s in context.Students
                        where s.Name.Contains(input)
                        select s).ToList();

        Console.WriteLine($"\n Students with '{input}' in their name:");
        if (filtered.Count == 0)
            Console.WriteLine("No matching students found.");
        else
            foreach (var s in filtered)
                Console.WriteLine($" {s.Name}");
    }

    static void LinqQuerySyntax_SortCourses(AppDbContext context)
    {
        var sortedCourses = (from c in context.Courses
                             orderby c.Title
                             select c).ToList();

        Console.WriteLine("\n [Query Syntax] Courses sorted alphabetically:");
        foreach (var c in sortedCourses)
            Console.WriteLine($" {c.Title}");
    }

    static void LinqQuerySyntax_AnyStudentNoCourses(AppDbContext context)
    {
        var studentsWithCourses = from s in context.Students.Include(s => s.Courses)
                                  where s.Courses.Count == 0
                                  select s;

        bool anyNoCourses = studentsWithCourses.Any();

        Console.WriteLine($"\n [Query Syntax] Are there any students with no courses? {(anyNoCourses ? "Yes" : "No")}");
    }


    // New Method Syntax LINQ implementations:

    static void LinqFilterStudentsByName_Method(AppDbContext context)
    {
        Console.Write("\n[Method Syntax] Enter partial name to filter students: ");
        string input = Console.ReadLine();

        var filtered = context.Students
                              .Where(s => s.Name.Contains(input))
                              .ToList();

        Console.WriteLine($"\nStudents with '{input}' in their name:");
        if (filtered.Count == 0)
            Console.WriteLine("No matching students found.");
        else
            foreach (var s in filtered)
                Console.WriteLine($" {s.Name}");
    }

    static void LinqSortCourses_Method(AppDbContext context)
    {
        var sortedCourses = context.Courses
                                   .OrderBy(c => c.Title)
                                   .ToList();

        Console.WriteLine("\n[Method Syntax] Courses sorted alphabetically:");
        foreach (var c in sortedCourses)
            Console.WriteLine($" {c.Title}");
    }

    static void LinqAnyStudentNoCourses_Method(AppDbContext context)
    {
        bool anyNoCourses = context.Students
                                   .Include(s => s.Courses)
                                   .Any(s => s.Courses.Count == 0);

        Console.WriteLine($"\n[Method Syntax] Are there any students with no courses? {(anyNoCourses ? "Yes" : "No")}");
    }
}
