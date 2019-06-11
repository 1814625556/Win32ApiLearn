using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace NlogLearn
{
    public static class MyClass
    {
        private static readonly Logger _logger = LogManager.GetLogger("CC");
        private static readonly Logger _clog = LogManager.GetLogger("Console");

        public static void Foo()
        {
            _logger.Info("Hello World");
        }

        public static void Debug()
        {
            _logger.Debug("this is debug demo");
        }

        public static void ConsoleColor()
        {
            _clog.Info("info控制台信息");
            _clog.Warn("warn控制台信息");
            _clog.Debug("debug控制台信息");
            _clog.Error("error控制台信息");
            _clog.Fatal("fatal控制台信息");
        }
    }
}
