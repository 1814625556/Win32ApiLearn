using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace UploadNuget
{
    public class XmlHelper
    {
        static XmlDocument xmldoc;
        static XmlNode xmlnode;
        static XmlElement xmlelem;
        public static bool CreateXml()
        {
            xmldoc = new XmlDocument();
            //加入XML的声明段落,<?xml version="1.0" encoding="gb2312"?>
            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmldoc.AppendChild(xmldecl);

            //加入一个根元素
            xmlelem = xmldoc.CreateElement("", "Employees", "");
            xmldoc.AppendChild(xmlelem);
            //加入另外一个元素
            for (int i = 1; i < 3; i++)
            {

                XmlNode root = xmldoc.SelectSingleNode("Employees");//查找<Employees> 
                XmlElement xe1 = xmldoc.CreateElement("Node");//创建一个<Node>节点 
                xe1.SetAttribute("genre", "DouCube");//设置该节点genre属性 
                xe1.SetAttribute("ISBN", "2-3631-4");//设置该节点ISBN属性 

                XmlElement xesub1 = xmldoc.CreateElement("title");
                xesub1.InnerText = "CS从入门到精通";//设置文本节点 
                xe1.AppendChild(xesub1);//添加到<Node>节点中 
                XmlElement xesub2 = xmldoc.CreateElement("author");
                xesub2.InnerText = "候捷";
                xe1.AppendChild(xesub2);
                XmlElement xesub3 = xmldoc.CreateElement("price");
                xesub3.InnerText = "58.3";
                xe1.AppendChild(xesub3);

                root.AppendChild(xe1);//添加到<Employees>节点中 
            }
            //保存创建好的XML文档
            xmldoc.Save("data.xml");
            return true;
        }

        public static bool UpdateXml(string xmlPath, string nodeName, string nodeValue)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);

            XmlNodeList nodeList = xmlDoc.SelectSingleNode("package").FirstChild.ChildNodes;//获取Employees节点的所有子节点 

            foreach (XmlNode xn in nodeList)//遍历所有子节点 
            {
                XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型 
                if (xe.Name == nodeName)
                    xe.InnerText = nodeValue;
            }
            xmlDoc.Save(xmlPath);//保存。
            return true;
        }
    }
}
