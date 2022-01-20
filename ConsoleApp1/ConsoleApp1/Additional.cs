using System;

namespace ConsoleApp1
{
    public class Additional
    {
        bool? SwitchIsOn = null;
        private int? x = null;

        public void run()
        {
            if (x.HasValue)
            {
                Console.WriteLine(true);
            }
            if (x != null)
            {
                    Console.WriteLine(true);
            }
                
            int y = x ?? 0;
            Console.WriteLine(y);
        }
        
        static void ex()
        {
            // Initialize:
            CoOrds coords1 = new CoOrds();
            CoOrds coords2 = new CoOrds(10, 10);
            // Declare an object:
            CoOrds coords3;
            // Initialize:
            coords3.x = 10;
            coords3.y = 20;
        }
    }
    
    
    public struct CoOrds{
        public int x, y;
        public CoOrds(int p1, int p2)
        {
            x = p1;
            y = p2;
        }
    }
    
    
    public record Person3(string FirstName, string LastName); // immutable record
    
    public record Person4 // immutable record
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
    };
    
    public record Person5 // mutable record
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    };

    
    // new type
    public delegate int AddNumbers(int a, int b);


    class newProgramm
    {
        public static void test()
        {
            AddNumbers add = Plus;
            AddNumbers sub = Minus;
            Console.WriteLine(DoMath(add, 2, 3) + DoMath(sub, 8, 3));
        }

        static int DoMath(AddNumbers f, int a, int b)
        {
            return f(a, b);
        }

        static int Plus(int a, int b)
        {
            return a + b;
        }

        static int Minus(int a, int b)
        {
            return a - b;
        }
    }
    
    
    }