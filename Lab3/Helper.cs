using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class Helper
    {
        public static int[] GetBinaryVectorFromImage(string imgPath)
        {
            var img = Image.FromFile(imgPath);
            return GetBinaryVectorFromImage(img);
        }

        public static int[] GetBinaryVectorFromImage(Image img)
        {
            var curImg = (Bitmap) img;
            var result = new List<int>();
            var binaryImg = new int[20, 30];
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    var color = curImg.GetPixel(i, j);
                    var val = color.A == 0 ? 0 : 1;
                    binaryImg[i, j] = val;
                }
            }

            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    var val = binaryImg[j, i];
                    result.Add(val);
                    //Console.Write(val);
                }
                //Console.Out.WriteLine();
            }
            return result.ToArray();
        }
    }
}
