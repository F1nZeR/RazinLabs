﻿using System;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Work()
        {
            var age = new Attribute("Age", new[] { "18", "19" }, typeof(int));
            //var salary = new Attribute("Salary", new[] { "60000", "15" }, typeof(string));
            var pledge = new Attribute("Pledge", new[] { "Yes" }, typeof(string));
            var history = new Attribute("Credithistory", new[] { "No" }, typeof(string));

            var attributes = new [] { age, pledge, history };

            var samples = GetDataTable();

            var id3 = new DecisionTreeId3();
            var root = id3.MountTree(samples, "Credit", attributes);

            PrintNode(root, "");
        }

        public static void PrintNode(TreeNode root, string tabs)
        {
            Console.WriteLine(tabs + '|' + root.Attribute + '|');

            if (root.Attribute.Values != null)
            {
                foreach (string nodeValue in root.Attribute.Values)
                {
                    Console.WriteLine(tabs + "\t" + "<" + nodeValue + ">");
                    TreeNode childNode = root.GetChildByBranchName(nodeValue);
                    PrintNode(childNode, "\t" + tabs);
                }
            }
        }
        
        static DataTable GetDataTable()
        {
            var result = new DataTable("work");

            var inputTextLines = File.ReadAllLines("data.txt");
            var headers = inputTextLines[0].Split('\t');
            foreach (var header in headers)
            {
                var matches = Regex.Matches(header, @"(\w|\s)+");
                var column = result.Columns.Add(matches[0].Value.Replace(" ", ""));
                var type = matches[1].Value;
                column.DataType = type == "q" ? typeof (int) : typeof (string);
            }

            for (int row = 1; row < inputTextLines.Count(); row++)
            {
                var strValues = inputTextLines[row].Split('\t');
                var vals = new List<object>();
                for (int i = 0; i < strValues.Length; i++)
                {
                    var strValue = strValues[i];
                    vals.Add(strValue);
                }
                result.Rows.Add(vals.ToArray());
            }

            return result;

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Work();
        }
    }
}
