using System.Drawing;

namespace GaussianBlur
{
    public class EdgeFinder
    {
        int laplacianRadius = 1;
        public Bitmap DetectEdges(Bitmap image)
        {
            var greyscale = ConvertToGreyscale(image);
            var edges = new Bitmap(image.Width, image.Height);
            for (int y = 0; y < image.Height; y += 1)
            {
                for (int x = 0; x < image.Width; x += 1)
                {
                    int pixelValue = 0;
                    for (int yy = -laplacianRadius; yy < laplacianRadius; yy += 1)
                    {
                        int indexY = y + yy;
                        if (indexY < 0) indexY = -indexY;
                        if (indexY >= edges.Height) indexY = y - yy;

                        for (int xx = -laplacianRadius; xx < laplacianRadius; xx += 1)
                        {
                            int indexX = x + xx;
                            if (indexX < 0) indexX = -indexX;
                            if (indexX >= edges.Width) indexX = x - xx;

                            var greyPixelValue = greyscale.GetPixel(indexX, indexY).R;
                            pixelValue += (xx, yy) == (0, 0)
                                ? greyPixelValue * 8
                                : greyPixelValue * -1;

                        }
                    }

                    pixelValue = pixelValue > 255
                        ? 255
                        : pixelValue < 0
                            ? 0
                            : pixelValue;

                    edges.SetPixel(x, y, Color.FromArgb(pixelValue, pixelValue, pixelValue));
                }
            }

            return edges;
        }

        public Bitmap ConvertToGreyscale(Bitmap image)
        {
            var greyscale = new Bitmap(image.Width, image.Height);
            for (int y = 0; y < image.Height; y += 1)
            {
                for (int x = 0; x < image.Width; x += 1)
                {
                    var currentPixel = image.GetPixel(x, y);
                    var colorValue = (currentPixel.R + currentPixel.G + currentPixel.B) / 3;
                    greyscale.SetPixel(x, y, Color.FromArgb(colorValue, colorValue, colorValue));
                }
            }

            return greyscale;
        }
    }
}
