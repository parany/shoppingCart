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
        public XmlNode _node { get; set; }

        private string _FilePath { get; set; }

        // Load the file
        public XmlHandler(string filePath)
        {
            _XmlData = new XmlDocument();
            _XmlData.Load(filePath);
            _FilePath = filePath;
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

        public XmlNode Create(string name)
        {
            return _XmlData.CreateElement(name);
        }

        public void ChangeNodeName(string path, string newName)
        {
            XmlNode currentNode = DirectPathNode(path);
            XmlNode newNode = Create(newName);
            currentNode.ParentNode.ReplaceChild(newNode, currentNode);
            foreach (XmlNode node in currentNode.ChildNodes)
            {
                newNode.AppendChild(node);
            }
            foreach (XmlAttribute att in currentNode.Attributes)
            {
                newNode.Attributes.Append(att);
            }

            Save();
        }

        public void DeleteBranch(string path)
        {
            XmlNode currentNode = DirectPathNode(path);
            currentNode.ParentNode.RemoveChild(currentNode);

            Save();
        }

        public void Save()
        {
            _XmlData.Save(_FilePath);
        }
    }
}