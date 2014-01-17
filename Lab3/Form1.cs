using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab3.Core;

namespace Lab3
{
    public partial class Form1 : Form
    {
        private Bitmap _image, _saveImage;
        private NeuralWorker _neuralWorker;
        const int SizeX = 20, SizeY = 30;
        
        public Form1()
        {
            InitializeComponent();

            _saveImage = new Bitmap(SizeX, SizeY);
            _image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                using (var g = Graphics.FromImage(_image))
                {
                    using (var g2 = Graphics.FromImage(_saveImage))
                    {
                        var stepX = pictureBox1.Width / SizeX;
                        var stepY = pictureBox1.Height / SizeY;

                        var x = e.X / stepX * stepX + 1;
                        var y = e.Y / stepY * stepY + 1;
                        g.FillRectangle(Brushes.Black, x, y, stepX, stepY);
                        g2.FillRectangle(Brushes.Black, e.X/stepX + 1, e.Y/stepY + 1, 1, 1);
                    }
                }
                pictureBox1.Image = _image;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var folder = "Learn/" + numericUpDown1.Value;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            _saveImage.Save(string.Format("{0}/{1}.bmp", folder, DateTime.Now.Ticks));
            ResetImgs();
        }

        private void ResetImgs()
        {
            _image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _saveImage = new Bitmap(SizeX, SizeY);
            pictureBox1.Image = _image;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResetImgs();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _neuralWorker = new NeuralWorker("Learn", label2);
            _neuralWorker.Learn();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var imgVector = Helper.GetBinaryVectorFromImage(_saveImage);
            var vals = _neuralWorker.Recognize(imgVector.Select(x => (double)x).ToList()).ToList();
            MessageBox.Show(string.Join("\n", vals));
            MessageBox.Show("Это: " + vals.IndexOf(vals.Max()));
        }
    }
}
