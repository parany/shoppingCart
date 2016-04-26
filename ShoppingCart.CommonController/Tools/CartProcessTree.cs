using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Xml;

namespace ShoppingCart.CommonController.Tools
{
    public class CartProcessTree : XmlHandler
    {
        public CartProcessTree(string filePath): base(filePath)
        {

        }

        /*
         * ACTIONS WITH NODES / VIEW NODES
         *
         *
         */
        public class NodeObject
        {
            public string label { get; set; }
            public string type { get; set; }
        }
        // Get all informations for the stage

        public IList<NodeObject> Descriptions(string path)
        {
            XmlNodeList nodes = DirectPathNode(path).ChildNodes;
            IList<NodeObject> des = new List<NodeObject>();
            foreach (XmlNode node in nodes)
            {
                if (!node.Name.Equals("Options") && !node.Name.Equals("Action"))
                {
                    des.Add(new NodeObject()
                    {
                        label = node.Name,
                        type = node.Value
                    });
                }
            }

            return des;
        }

        public IList<NodeObject> ForwardOptions(string path)
        {
            
            IList<NodeObject> des = new List<NodeObject>();

            if (DirectPathNode(path + "/Options") != null)
            {
                XmlNodeList nodes = DirectPathNode(path + "/Options").ChildNodes;
                foreach (XmlNode node in nodes)
                {
                    des.Add(new NodeObject()
                    {
                        label = node.Value,
                        type = node.Name
                    });
                }
            }

            return des;
        }

        public bool TreeState(string path)
        {
            /* RESULT
             * true => state on going
             * false => final state
             */
            return DirectPathNode(path).SelectSingleNode("Options").HasChildNodes;
        }

        public IList<NodeObject> RowllingBack(string path)
        {
            XmlNodeList nodes = DirectPathNode(path).ParentNode.ParentNode.ChildNodes;
            IList<NodeObject> des = new List<NodeObject>();
            foreach (XmlNode node in nodes)
            {
                des.Add(new NodeObject()
                {
                    label = node.Name,
                    type = node.Value
                });
            }

            return des;
        }

        public string CurrentPotitionOnTree(string path)
        {
            return DirectPathNode(path).Name;
        }

        
        /*
         * CREATE NODES
         *
         *
         */

    }
}