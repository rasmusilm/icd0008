using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Person
    {
        public int PersonId { get; set; }
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public List<CourseDeclaration>? CourseDeclarations { get; set; }

        public override string ToString()
        {
            return $"{PersonId}: {FirstName} {LastName} declarations {CourseDeclarations?.Count.ToString() ?? "null"}";
        }
    }
}