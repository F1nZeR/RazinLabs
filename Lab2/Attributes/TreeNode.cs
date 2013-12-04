using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Attributes
{
    public class TreeNode
    {
        private readonly ArrayList _mChilds;
        private readonly Attribute _mAttribute;

        public TreeNode(Attribute attribute)
        {
            if (attribute != null && attribute.Values != null)
            {
                _mChilds = new ArrayList(attribute.Values.Length);
                for (int i = 0; i < attribute.Values.Length; i++)
                    _mChilds.Add(null);
            }
            else
            {
                _mChilds = new ArrayList(1) {null};
            }
            _mAttribute = attribute;
        }
        
        public void AddTreeNode(TreeNode treeNode, string valueName)
        {
            int index = _mAttribute.IndexValue(valueName);
            _mChilds[index] = treeNode;
        }

        public int TotalChilds
        {
            get { return _mChilds.Count; }
        }

        public TreeNode GetChild(int index)
        {
            return (TreeNode)_mChilds[index];
        }

        public Attribute Attribute
        {
            get { return _mAttribute; }
        }

        public TreeNode GetChildByBranchName(string branchName)
        {
            int index = _mAttribute.IndexValue(branchName);
            return (TreeNode)_mChilds[index];
        }
    }
}
