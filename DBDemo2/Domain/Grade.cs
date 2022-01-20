namespace Domain
{
    public class Grade
    {
        public int GradeId { get; set; }
        
        public int GradeValue { get; set; }
        
        public int HomeworkId { get; set; }
        public Homework? Homework { get; set; }
        
        public int CourseDeclarationId { get; set; }
        public CourseDeclaration? Declaration { get; set; }
    }
}