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

            var stu = new Student();
            Console.WriteLine(stu.book?.Name);


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

    class Student
    {
        public string Name { get; set; }
        public Book book { get; set; }
        public List<string> list { get; set; }
    }

    class Book
    {
        public string Name { get; set; }
    }
}
