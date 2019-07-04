using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;

namespace ConsoleApp1
{
    public class Module : NinjectModule
    {
        public override void Load()
        {
            if (Kernel != null)
            {
                Kernel.Bind<Person>()
                    .ToMethod(context => new Person() {Name = "chenchang", Age = 26, Address = "shanghai"})
                    .InSingletonScope();
                Kernel.Bind<Student2>().ToMethod(context => new Student2("lisi", "LongXiao"));
            }
        }
    }
}
