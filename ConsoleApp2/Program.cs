using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<bool> list = new List<bool>();
            for (var i = 0; i < 10; i++)
            {
                list.Add(false);
            }
            list.Add(true);

            var flag = list.Any();

            var str = ClassLibrary1.Class2.GetHello();
            var message = ClassLibrary1.Class1.GetHello();
        }
    }
}
