using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string result = "";
            string taxStr = "1070303990000000000";
            for (var i = taxStr.Length - 1; i > 1; i--)
            {
                if (taxStr[i] == '0')
                {
                    result = taxStr.Substring(0, i+1);
                    continue;
                }
                else
                {
                    result = taxStr.Substring(0, i+1);
                }
                break;
            }
        }
    }
}
