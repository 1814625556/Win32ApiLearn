using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class DicToEntity
    {
        public static Dictionary<string, object> StudentToDic(Student stu)
        {
            var str = JsonConvert.SerializeObject(new Student()
            {
                StudentHead = new Head()
                {
                    Name = "chenchang",
                    Age = "26"
                },
                Details = new List<Detail>()
                {
                    new Detail() {Address = "shanghai", FriendName = "chenyezhou", QQ = "1234567", WeiXin = "1234567"},
                    new Detail() {Address = "changzhou", FriendName = "xiaochen", QQ = "7654321", WeiXin = "7654321"}
                }
            });
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
        }

        public static Student DicToStudent(Dictionary<string, object> dic)
        {
            var student = new Student();

            //JsonConvert.DeserializeObject<>()

            foreach (var d in dic)
            {
                student.StudentHead.GetType().GetProperty(d.Key).SetValue(student.StudentHead,d.Value,null);
            }

            return student;
        }
    }

    public class Student
    {
        public Head StudentHead { get; set; }
        public IList<Detail> Details { get; set; }
    }

    public class Head
    {
        public string Name { get; set; }
        public string Age { get; set; }
    }

    public class Detail
    {
        public string QQ { get; set; }
        public string WeiXin { get; set; }
        public string FriendName { get; set; }
        public string Address { get; set; }
    }


}
