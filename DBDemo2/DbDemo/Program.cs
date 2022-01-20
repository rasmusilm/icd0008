using System;
using System.Linq;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore.Design;

namespace DbDemo
{
    class Program
    {
        static void Main(string[] args)
        {
           Console.WriteLine("Hello World!");

            using var db = new ApplicationDbContext();
            Console.WriteLine("Person count " + db.Persons.Count());

            /*var person = new Person
            {
                FirstName = "Jaan",
                LastName = "Kibus"
            };

            db.Persons.Add(person);

            // send data to db engine
            db.SaveChanges();*/


            Console.WriteLine("Person count " + db.Persons.Count());

            foreach (var dbPerson in db.Persons)
            {
                Console.WriteLine(dbPerson);
            }

            var personFromDb = db.Persons.FirstOrDefault(p => p.PersonId == 1);
            if (personFromDb != null)
            {
                personFromDb.FirstName = personFromDb.FirstName + " " + Guid.NewGuid();
                personFromDb.LastName = personFromDb.LastName + " " + Guid.NewGuid();
            }

            db.SaveChanges();

            foreach (var dbPerson in db.Persons)
            {
                Console.WriteLine(dbPerson);

            }
        }
    }
}