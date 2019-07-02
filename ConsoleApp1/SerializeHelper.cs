using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace ConsoleApp1
{
    public class SerializeHelper
    {
        private static readonly JavaScriptSerializer JavaScriptSerializer;
        static SerializeHelper()
        {
            JavaScriptSerializer = new JavaScriptSerializer {MaxJsonLength = int.MaxValue};
        }
        private SerializeHelper()
        {
        }

        public static string SerializeObject(object obj)
        {
            return JavaScriptSerializer.Serialize(obj);
        }

        public static T DeserializeObject<T>(string json)
        {
            return JavaScriptSerializer.Deserialize<T>(json);
        }
    }
}
