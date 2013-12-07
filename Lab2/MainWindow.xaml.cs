using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Lab2.Attributes;
using Attribute = Lab2.Attributes.Attribute;

namespace Lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataTable _samples;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _samples = GetDataTable();
        }

        private void Work()
        {
            var attributes = GetInputData();

            //var age = new Attribute("Age", new[] { "18" }, typeof(int));
            //var salary = new Attribute("Salary", new[] { "25000" }, typeof(int));
            ////var pledge = new Attribute("Pledge", new[] { "No" }, typeof(string));
            //var history = new Attribute("Credithistory", new[] { "No" }, typeof(string));

            //var attributes = new[] { age, salary, history };
            
            var id3 = new DecisionTreeId3();
            Logger.Init(id3);
            var root = id3.MountTree(_samples, "Credit", attributes.ToArray());

            RenameRoot(root);

            var parentItem = new TreeViewItem() { Header = "RESULT", IsExpanded = true };
            PrintNode(root, parentItem);
            DecisionTree.Items.Clear();
            DecisionTree.Items.Add(parentItem);
            LogTextBlock.Text = Logger.LogText;
        }

        private IEnumerable<Attribute> GetInputData()
        {
            foreach (var textBox in EditorWrap.Children.OfType<TextBox>())
            {
                var name = textBox.Name;
                var values = textBox.Text.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                if (!values.Any()) continue;
                var type = (Type) textBox.Tag;
                yield return new Attribute(name, values, type);
            }
        }

        private void RenameRoot(TreeNode root)
        {
            var isSuccess = root.UpdateNulls();
            if (!isSuccess) return;

            if (root.Attribute.Type == typeof(int))
            {
                var newVals = new List<string>();
                var correctValues = root.Attribute.Values.Where(x => !Regex.IsMatch(x, "<=|>")).ToList();
                
                // уже нормальные названия
                if (!correctValues.Any() && root.Attribute.Values.Any())
                {
                    newVals = root.Attribute.Values.ToList();
                }
                else
                {
                    foreach (var val in correctValues)
                    {
                        newVals.Add("<=" + val);
                        newVals.Add(">" + val);
                    }
                }
                root.Attribute.Values = newVals.ToArray();
            }

            for (int i = 0; i < root.TotalChilds; i++)
            {
                var cur = root.GetChild(i);
                if (cur == null) continue;
                RenameRoot(cur);
            }
        }

        public static void PrintNode(TreeNode root, TreeViewItem parent)
        {
            if (root == null || root.Attribute == null) return;
            var attr = new TreeViewItem() { Header = root.Attribute, IsExpanded = true };
            parent.Items.Add(attr);

            if (root.Attribute.Values == null) return;

            foreach (var nodeValue in root.Attribute.Values)
            {
                var val = new TreeViewItem() {Header = nodeValue, IsExpanded = true};
                attr.Items.Add(val);

                var childNode = root.GetChildByBranchName(nodeValue);
                PrintNode(childNode, val);
            }
        }

        //public static void PrintNode(TreeNode root, string tabs)
        //{
        //    if (root == null || root.Attribute == null) return;
        //    Console.WriteLine(tabs + '|' + root.Attribute + '|');

        //    if (root.Attribute.Values == null) return;
            
        //    foreach (var nodeValue in root.Attribute.Values)
        //    {
        //        Console.WriteLine(tabs + "\t" + "<" + nodeValue + ">");
        //        var childNode = root.GetChildByBranchName(nodeValue);
        //        PrintNode(childNode, "\t" + tabs);
        //    }
        //}
        
        private DataTable GetDataTable()
        {
            EditorWrap.Children.Clear();
            var result = new DataTable("work");

            var inputTextLines = File.ReadAllLines("data.txt");
            var headers = inputTextLines[0].Split('\t');
            foreach (var header in headers)
            {
                var matches = Regex.Matches(header, @"(\w|\s)+");
                var column = result.Columns.Add(matches[0].Value.Replace(" ", ""));
                var type = matches[1].Value;
                column.DataType = type == "q" ? typeof (int) : typeof (string);


                if (type == "g")
                {
                    TargetAttrTextBlock.Text = "Целевой атрибут: " + matches[0].Value;
                }
                else
                {
                    EditorWrap.Children.Add(new TextBlock
                    {
                        Text = matches[0].Value, 
                        Width = EditorWrap.ActualWidth / 3, 
                        Margin = new Thickness(0, 0, 0, 5)
                    });
                    EditorWrap.Children.Add(new TextBox
                    {
                        Name = matches[0].Value,
                        Tag = column.DataType,
                        Width = EditorWrap.ActualWidth * 2 / 3, 
                        Margin = new Thickness(0, 0, 0, 5)
                    });
                }

            }

            for (int row = 1; row < inputTextLines.Count(); row++)
            {
                var strValues = inputTextLines[row].Split('\t');
                result.Rows.Add(strValues.Cast<object>().ToArray());
            }

            return result;

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Work();
        }
    }
}
