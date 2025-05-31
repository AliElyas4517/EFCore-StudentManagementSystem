using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore
{
    internal class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }

        // Foreign key
        public int StudentId { get; set; }

        // Navigation property
        public Student Student { get; set; }
    }
}
