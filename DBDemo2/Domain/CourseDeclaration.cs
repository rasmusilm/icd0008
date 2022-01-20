using System.Collections.Generic;

namespace Domain
{
    public class CourseDeclaration
    {
        public int CourseDeclarationId { get; set; }
        
        public bool IsStudent { get; set; }
        public int FinalGrade { get; set; }
        
        public int PersonId { get; set; }
        public Person? Person { get; set; }
        
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public List<Grade>? Grades { get; set; }
    }
}