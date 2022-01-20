using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            newProgramm.test();
        }
    }

    public class Person
    {
        private int _height;

        public Person(int height)
        {
            Height = height;
        }

        public Person(): this(175)
        {
            
        }

        public int Height
        {
            get { return _height;}
            set { _height = value; }
        }

        public void SetHeight()
        {
            Height = 5;
        }

        public int X(int i)
        {
            return 1;
        }

        public int X(string s)
        {
            return int.Parse(s);
        }
    }


    public class AnotherPerson
    {
        public string Name;
        protected int SocialCode;

        public AnotherPerson()
        {
            Console.WriteLine("Person");
        }
    }

    public class Student : Person
    {
        public Student()
        {
            Console.Write("Student");
        }
    }
    
    abstract class BaseClass
    {
        public abstract double Area(double side);
    }

    class BaseClassRoot : BaseClass
    {
        public override double Area(double side)
        {
            return side*side;
        }
    }

    class DerivedClassCube: BaseClassRoot
    {
        public override double Area(double side)
        {
            return base.Area(side) * 6;
        }
    }
    
    interface ISampleInterface
    {
        void DoSomething();
    }

    class SampleClassWithInterface : ISampleInterface
    {
        public void DoSomething()
        {
            throw new NotImplementedException(); 
        }
    }
}