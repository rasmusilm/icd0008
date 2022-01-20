using System;
using System.Collections.Generic;

namespace Domain
{
    public class Student
    {
        
        public int StudentId { get; set; }
        
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public ICollection<Grade> Grades { get; set; } = null!;
        
        
        public override string ToString()
        {
            return StudentId + " - " + FirstName + " " + LastName;
        }
    }
}