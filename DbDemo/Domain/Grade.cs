namespace Domain
{
    public class Grade
    {
        public int GradeId { get; set; }
        
        public int GradeValue { get; set; }
        
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        
        public int HomeworkId { get; set; }
        public Homework? Homework { get; set; }

        public override string ToString()
        {
            return
                $"{GradeId} - {GradeValue}. StudentID: {StudentId}, HomeworkID: {HomeworkId}, Student: {Student}, Homework: {Homework}";
        }
    }
}