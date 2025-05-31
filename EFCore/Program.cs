using System;
using System.Collections.Generic;
using System.Linq;
using EFCore;
using Microsoft.EntityFrameworkCore;

namespace EFCoreApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using var context = new AppDbContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Seed data
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
                // No courses
            };

            context.Students.AddRange(student1, student2, student3);
            context.SaveChanges();

            Console.WriteLine("\nStudents with Courses:");
            var students = context.Students.Include(s => s.Courses).ToList();
            foreach (var s in students)
            {
                Console.WriteLine($"Student: {s.Name}");
                foreach (var c in s.Courses)
                {
                    Console.WriteLine($"Course: {c.Title}");
                }
            }

            Console.WriteLine("\nSearch student by name: Ali");
            var found = context.Students
                .Where(s => s.Name.Contains("Ali"))
                .Include(s => s.Courses)
                .ToList();

            foreach (var s in found)
            {
                Console.WriteLine($"Found: {s.Name}");
            }

            Console.WriteLine($"\nTotal Students: {context.Students.Count()}");
            Console.WriteLine($"Total Courses: {context.Courses.Count()}");

            Console.WriteLine("\nCourses containing 'Pro':");
            var filteredCourses = context.Courses
                .Where(c => c.Title.Contains("Pro"))
                .ToList();

            foreach (var c in filteredCourses)
            {
                Console.WriteLine($"Course: {c.Title}");
            }

            Console.WriteLine("\nStudents with NO courses:");
            var noCourses = context.Students
                .Include(s => s.Courses)
                .Where(s => s.Courses.Count == 0)
                .ToList();

            foreach (var s in noCourses)
            {
                Console.WriteLine($" {s.Name}");
            }

            var courseToUpdate = context.Courses.FirstOrDefault(c => c.Title == "Biology");
            if (courseToUpdate != null)
            {
                courseToUpdate.Title = "Advanced Biology";
                context.SaveChanges();
                Console.WriteLine("\n Course updated to Advanced Biology");
            }

            Console.WriteLine("\nFinal student list with updated data:");
            var finalList = context.Students.Include(s => s.Courses).ToList();
            foreach (var s in finalList)
            {
                Console.WriteLine($"Student: {s.Name}");
                foreach (var c in s.Courses)
                {
                    Console.WriteLine($"   Course: {c.Title}");
                }
            }

            Console.WriteLine("\n Application Finished!");
        }
    }
}
