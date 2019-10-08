using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NugetClass1
{
    public class AnimaFactory
    {
        public static void DogEat()
        {
            Console.WriteLine($"dog love eat meat...");
        }

        public static void CatEat()
        {
            Console.WriteLine($"cat love eat fish...");
        }

        public static void MonkeyEat()
        {
            Console.WriteLine($"Monkey love eat banana...");
        }
    }
}
