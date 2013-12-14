using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_Correct.Core
{
    public class TreeNode
    {
        private readonly List<TreeNode> _childrens;
        public readonly Attribute Attr;
        public string Value { get; set; }
        public string Name { get { return Attr.Name; } }
        private readonly double _infoGain;

        public string DisplayName
        {
            get
            {
                return string.Format("{0}: {1}", Attr.Name, Value);
            }
        }

        public TreeNode(Attribute attr)
        {
            Attr = attr;
            _infoGain = attr.InfoGain;
            _childrens = new List<TreeNode>();
        }

        public TreeNode(Attribute attr, string value)
            : this(attr)
        {
            Value = value;
        }

        public bool HasChilds()
        {
            return _childrens.Any();
        }

        public List<TreeNode> GetChildrens()
        {
            return _childrens;
        }

        public void AddChildren(TreeNode node)
        {
            _childrens.Add(node);
        }

    }
}
