using System;
using System.Linq;
using EFCore;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        using var context = new AppDbContext();

        // Reset DB (for demo purposes)
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // ✅ Add student with courses
        var student = new Student
        {
            Name = "Ali Elyas",
            Courses = new List<Course>
            {
                new Course { Title = "Math" },
                new Course { Title = "Programming" }
            }
        };

        context.Students.Add(student);
        context.SaveChanges();

        // ✅ Read all students with courses
        Console.WriteLine("Students and their courses:");
        var students = context.Students.Include(s => s.Courses).ToList();
        foreach (var s in students)
        {
            Console.WriteLine($"Student: {s.Name}");
            foreach (var c in s.Courses)
            {
                Console.WriteLine($"  Course: {c.Title}");
            }
        }

        // ✅ Update student name
        var ali = context.Students.FirstOrDefault(s => s.Name == "Ali Elyas");
        if (ali != null)
        {
            ali.Name = "Ali Updated";
            context.SaveChanges();
        }

        // ✅ Delete one course
        var math = context.Courses.FirstOrDefault(c => c.Title == "Math");
        if (math != null)
        {
            context.Courses.Remove(math);
            context.SaveChanges();
        }

        // ✅ Final Output
        Console.WriteLine("\nAfter Update and Delete:");
        foreach (var s in context.Students.Include(s => s.Courses))
        {
            Console.WriteLine($"Student: {s.Name}");
            foreach (var c in s.Courses)
            {
                Console.WriteLine($"  Course: {c.Title}");
            }
        }
    }
}
