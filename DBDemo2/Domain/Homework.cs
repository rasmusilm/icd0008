using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Homework
    {
        public int HomeworkId { get; set; }
        
        [MaxLength(255)]
        public string HomeworkDescription { get; set; } = default!;
        
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public List<Grade>? Grades { get; set; }
    }
}