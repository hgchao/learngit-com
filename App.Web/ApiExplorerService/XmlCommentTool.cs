using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace App.Web.ApiExplorerService
{
    public class XmlCommentTool
    {
        private XmlDocument _document { get; }
        public XmlCommentTool(string path)
        {
            _document = new XmlDocument();
            _document.Load(path);
        }

        public string GetActionSummary(string name)
        {
            string[] actionSegment = name.Split('(');
            string xpath = $"//member[starts-with(@name,'M:{actionSegment[0]}')]/summary";
            XmlNode node = _document.SelectSingleNode(xpath);
            if (node == null)
                return null;
            Regex reg = new Regex(@"\s+");
            Regex reg2 = new Regex(@"\r\n");
            var output = reg.Replace(node.InnerText, String.Empty);
            return reg2.Replace(output, String.Empty);
        }

        public Dictionary<string, string> GetPropertySummaries()
        {
            var res = new Dictionary<string, string>();
            string xpath = $"//member[starts-with(@name,'P:')]";
            var list = _document.SelectNodes(xpath);
            foreach (XmlNode node in list)
            {
                var name = node.Attributes["name"].Value.Replace("P:", "");
                if (res.ContainsKey(name))
                    continue;
                var value = node.SelectSingleNode("summary").InnerText;
                Regex reg = new Regex(@"\s+");
                Regex reg2 = new Regex(@"\r\n");
                var output = reg.Replace(node.InnerText, String.Empty);
                output = reg2.Replace(output, String.Empty);
                res.Add(name, output);
            };
            return res;
        }


        public string GetControllerSummary(string name)
        {
            string xpath = $"//member[@name='T:{name}']/summary";
            XmlNode node = _document.SelectSingleNode(xpath);
            if (node == null)
                return null;
            Regex reg = new Regex(@"\s+");
            Regex reg2 = new Regex(@"\r\n");
            var output = reg.Replace(node.InnerText, String.Empty);
            return reg2.Replace(output, String.Empty);
        }
    }
} 