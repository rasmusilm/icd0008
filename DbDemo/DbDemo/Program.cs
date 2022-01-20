using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DbDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (var db = new AppDbContext())
            {
                db.Database.Migrate();
                
                var grade = new Grade()
                {
                    GradeValue = 5,
                    Student = new Student()
                    {
                        FirstName = "Rasmus",
                        LastName = "Ilmjärv",
                    },
                    Homework = new Homework()
                    {
                        Description = "Set up db2"
                    }
                };
                Console.WriteLine(grade);

                db.Grades?.Add(grade);
                Console.WriteLine(grade);
                db.SaveChanges();
                foreach (var exGrade in db.Grades
                    .Include(g => g.Student)
                    .Include(g => g.Homework)
                    .Where(g => g.GradeId > 2))
                {
                    Console.WriteLine(exGrade);
                }
            }
            
        }
    }
}