using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace ShoppingCart.CommonController.Tools
{
    public class XmlHandler
    {
        private XmlDocument _XmlData { get; set; }
        private XmlNode _node { get; set; }

        // Load the file
        public XmlHandler(string filePath)
        {
            _XmlData = new XmlDocument();
            _XmlData.Load(filePath);
            _node = _XmlData.ChildNodes[1]; // this will get the head of the tree "//workflows"
        }

        //get the head of the branch
        public XmlNode PrincipleNode(string s)
        {
            return _node.SelectSingleNode(".//" + s);
        }

        //find a sub node of a branch
        // xPath => .// + level1/level2/level3 ...

        public XmlNode GetUnderLevelNode(XmlNode start, string s)
        {
            return start.SelectSingleNode(".//" + s);
        }

        public XmlNode DirectPathNode(string s)
        {
            _node = _XmlData.ChildNodes[1];
            return _node.SelectSingleNode(".//" + s);
        }

        public XmlNodeList Heads()
        {
            return _node.ChildNodes;
        }

        public XmlNode StartNode()
        {
            return _node;
        }
    }
}