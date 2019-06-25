using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class DynamicTest
    {
        public static void MainInvoke()
        {

            var aa = GetEntity();
            var name = aa.name;
            var age = aa.age;

            Console.WriteLine($"Name:{name},Age:{age}");
            

            ExampleClass ec = new ExampleClass();
            // The following call to exampleMethod1 causes a compiler error 
            // if exampleMethod1 has only one parameter. Uncomment the line
            // to see the error.
            //ec.exampleMethod1(10, 4);

            dynamic dynamic_ec = new ExampleClass();
            // The following line is not identified as an error by the
            // compiler, but it causes a run-time exception.
            dynamic_ec.exampleMethod1(10, 4);

            // The following calls also do not cause compiler errors, whether 
            // appropriate methods exist or not.
            dynamic_ec.someMethod("some argument", 7, null);
            dynamic_ec.nonexistentMethod();
        }


        public static dynamic GetEntity()
        {
            return GetEntity1();
        }

        public static dynamic GetEntity1()
        {
            return new { name = "chenchang", age = 11 };
        }


    }

    class ExampleClass
    {
        public ExampleClass() { }
        public ExampleClass(int v) { }

        public void exampleMethod1(int i) { }

        public void exampleMethod2(string str) { }
    }
}
