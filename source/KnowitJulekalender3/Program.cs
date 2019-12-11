using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace KnowitJulekalender3
{
    class Program
    {
        static void Main(string[] args)
        {
            var txt = File.ReadAllText("./img.txt");
            for(int width = 20; width < txt.Length / 20; width++)
            {
                if ((txt.Length % width) != 0) continue;

                int height = txt.Length / width;
                Bitmap bmp = new Bitmap(width, height);
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int pos = y * width + x;
                        if (txt[pos] == '0') bmp.SetPixel(x, y, Color.Red);
                    }
                }
                bmp.Save($"c:/temp/IKEA_{width}.png", ImageFormat.Png);

            }
        }
    }
}
