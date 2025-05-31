using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore
{
    internal class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }

        // Navigation property
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
