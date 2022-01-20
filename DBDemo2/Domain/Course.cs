using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Course
    {
        public int CourseId { get; set; }
        
        [MaxLength(64)]
        public string? CourseName { get; set; }

        public List<CourseDeclaration> CourseDeclarations { get; set; } = default!;
        public List<Homework> Homeworks { get; set; } = default!;
    }
}