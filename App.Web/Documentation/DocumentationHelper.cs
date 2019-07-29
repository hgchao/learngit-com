using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace App.Web.Documentation
{
    public class DocumentationHelper
    {
        public static void Merge()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var files = Directory.GetFiles(directory, "App.*.xml").ToList();
            var mainFile = files.Where(u => u.Contains("App.Web.xml")).FirstOrDefault();
            if (string.IsNullOrEmpty(mainFile))
                return;
            files.Remove(mainFile);
            if (files.Count == 0)
                return;
            var mainDoc = new XmlDocument();
            mainDoc.Load(mainFile);
            var membersNode = mainDoc.SelectSingleNode("//members");
            files.ForEach(file => {
                var doc = new XmlDocument();
                doc.Load(file);
                var nodes = doc.SelectNodes("//member");
                foreach(XmlNode node in nodes)
                {
                    var importNode = mainDoc.ImportNode(node, true);
                    membersNode.AppendChild(importNode);
                }
            });
            files.ForEach(u =>
            {
                File.Delete(u);
            });
            mainDoc.Save(mainFile);
        }
    }
}
