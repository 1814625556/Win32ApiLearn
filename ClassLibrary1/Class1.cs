using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ClassLibrary1
{
    public static class Class1
    {
        public static string GetHello()
        {
            return JsonConvert.SerializeObject(new PERSON()
            {
                Name = "ZHANGSAN",
                Address = "SHANGHAI"
            });
        }

    }
}
