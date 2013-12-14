using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lab2_Correct.Core;
using Lab2_Correct.Helper;
using Attribute = Lab2_Correct.Core.Attribute;

namespace Lab2_Correct
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private TreeNode _res;
        private string _goalAttr;

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var fileReader = new FileTreeReader();
            var inputAttrs = fileReader.ReadTable("data.txt");
            _goalAttr = fileReader.GoalAttrName;
            LoadInputs(inputAttrs.Where(x => x.Name != _goalAttr).ToList());

            var dTree = new DecisionTree(inputAttrs, _goalAttr);
            Logger.Init(dTree);

            var parent = new TreeViewItem() {Header = "RESULT", IsExpanded = true};
            _res = dTree.MountTree();
            FillTree(_res, parent);
            DecisionTree.Items.Add(parent);
            Log.Text = Logger.LogText;
        }

        private void LoadInputs(IEnumerable<Attribute> inputAttrs)
        {
            foreach (var attribute in inputAttrs)
            {
                EditorWrap.Children.Add(new TextBlock
                {
                    Text = attribute.Name,
                    Width = EditorWrap.ActualWidth / 3,
                    Margin = new Thickness(0, 0, 0, 5)
                });
                EditorWrap.Children.Add(new TextBox
                {
                    Name = attribute.Name.Replace(" ", ""),
                    Tag = attribute.IsQuantity,
                    Width = EditorWrap.ActualWidth * 2 / 3,
                    Margin = new Thickness(0, 0, 0, 5)
                });
            }
            var btn = new Button()
            {
                Content = "Run query",
                Width = EditorWrap.ActualWidth,
                Height = 30
            };
            btn.Click += (sender, args) => RunQuery();
            EditorWrap.Children.Add(btn);
        }
        
        private void FillTree(TreeNode node, TreeViewItem parent)
        {
            var child = new TreeViewItem() {Header = node.DisplayName, IsExpanded = true};
            parent.Items.Add(child);

            if (node.HasChilds())
            {
                foreach (var children in node.GetChildrens())
                {
                    FillTree(children, child);
                }
            }
        }


        private void RunQuery()
        {
            var res = FindAttr(_res);
            MessageBox.Show("Решение: " + res);
        }

        private string FindAttr(TreeNode curNode)
        {
            if (curNode.Name == _goalAttr)
            {
                return curNode.Value;
            }

            if (curNode.Value == "")
            {
                foreach (var children in curNode.GetChildrens())
                {
                    var res = FindAttr(children);
                    if (res != "WTF") return res;
                }
            }
            else
            {
                // curnode = age: <18
                var tb = EditorWrap.Children.OfType<TextBox>().Single(x => x.Name == curNode.Name.Replace(" ", ""));
                var tbVal = tb.Text;
                if ((bool) tb.Tag)
                {
                    if (curNode.Value.Contains("<"))
                    {
                        if (int.Parse(tbVal) < int.Parse(curNode.Value.Replace("<", "")))
                        {
                            foreach (var children in curNode.GetChildrens())
                            {
                                var res = FindAttr(children);
                                if (res != "WTF") return res;
                            }
                        }
                    }
                    else
                    {
                        if (int.Parse(tbVal) >= int.Parse(curNode.Value.Replace(">=", "")))
                        {
                            foreach (var children in curNode.GetChildrens())
                            {
                                var res = FindAttr(children);
                                if (res != "WTF") return res;
                            }
                        }
                    }
                    
                }
                else
                {
                    if (curNode.Value == tbVal)
                    {
                        foreach (var children in curNode.GetChildrens())
                        {
                            var res = FindAttr(children);
                            if (res != "WTF") return res;
                        }
                    }
                }
            }
            return "WTF";
        }
    }
}
