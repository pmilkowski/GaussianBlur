using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GaussianBlur
{
    public class ImageBlur
    {
        public ImageBlur(int blur)
        {
            this.Blur = blur;
            Pascal = PascalRow(Blur * 2 + 1);
            Det = Pascal.Sum();
        }

        public int Blur { get; private set; }
        public int Det { get; }
        public List<int> Pascal { get; }
        public void BlurImage(string input, string output)
        {
            var image = new Bitmap(input);

            var blurred = BlurVertically(
                BlurHorizontally(image)
            );

            blurred.Save(output);
        }

        private Bitmap BlurHorizontally(Bitmap bitmap)
        {
            var blurred = new Bitmap(bitmap.Width, bitmap.Height);
            for (int x = 0; x < bitmap.Width; x += 1)
            {
                for (int y = 0; y < bitmap.Height; y += 1)
                {
                    var (r, g, b) = (0, 0, 0);

                    for (int i = -Blur; i <= Blur; i += 1)
                    {
                        int index = x + i;
                        if (index < 0) index = -index;
                        if (index >= bitmap.Width) index = x - i;

                        r += Pascal[i + Blur] * bitmap.GetPixel(index, y).R / Det;
                        g += Pascal[i + Blur] * bitmap.GetPixel(index, y).G / Det;
                        b += Pascal[i + Blur] * bitmap.GetPixel(index, y).B / Det;
                    }

                    blurred.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return blurred;
        }

        private Bitmap BlurVertically(Bitmap bitmap)
        {
            var blurred = new Bitmap(bitmap.Width, bitmap.Height);
            for (int x = 0; x < bitmap.Width; x += 1)
            {
                for (int y = 0; y < bitmap.Height; y += 1)
                {
                    var (r, g, b) = (0, 0, 0);

                    for (int i = -Blur; i < Blur; i += 1)
                    {
                        int index = y + i;
                        if (index < 0) index = -index;
                        if (index >= bitmap.Height) index = y - i;

                        r += Pascal[i + Blur] * bitmap.GetPixel(x, index).R / Det;
                        g += Pascal[i + Blur] * bitmap.GetPixel(x, index).G / Det;
                        b += Pascal[i + Blur] * bitmap.GetPixel(x, index).B / Det;
                    }

                    blurred.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return blurred;
        }

        private List<int> PascalRow(int row_number)
        {
            int n = row_number;

            // Make the results as a list of long.
            List<int> results = new List<int>();
            int value = 1;
            results.Add(value);

            // Calculate the values.
            for (int k = 1; k <= n; k++)
            {
                value = (value * (n + 1 - k)) / k;
                results.Add(value);
            }

            return results;
        }
    }
}
