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

            SayHello<Student>(new Student()
            {
                Name = "zhangsan",
                book = new Book() { Name = "haiyan"}
            });
        }

        public static void SayHello<T>(T param)
        {
            var stu = param as Student;

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
