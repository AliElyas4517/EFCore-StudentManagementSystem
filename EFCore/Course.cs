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

            public int StudentId { get; set; } // Foreign key
            public Student Student { get; set; } // Navigation property
        
    }
}
