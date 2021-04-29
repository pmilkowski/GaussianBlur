using System.Drawing;

namespace GaussianBlur
{
    public class LaplacianOfGaussian
    {
        private EdgeFinder edgeFinder;
        private const int kernelSize = 4;
        public LaplacianOfGaussian()
        {
            edgeFinder = new EdgeFinder();
        }

        private int[][] kernel =
        {
new int[] { 0, 1, 1,  2,  2,  2,  1,  1,  0 },
new int[] { 1, 2, 4,  5,  5,  5,  4,  2,  1 },
new int[] { 1, 4, 5,  3,  0,  3,  5,  4,  1 },
new int[] { 2, 5, 3,-12,-24,-12,  3,  5,  2 },
new int[] { 2, 5, 0,-24,-40,-24,  0,  5,  2 },
new int[] { 2, 5, 3,-12,-24,-12,  3,  5,  2 },
new int[] { 1, 4, 5,  3,  0,  3,  5,  4,  1 },
new int[] { 1, 2, 4,  5,  5,  5,  4,  2,  1 },
new int[] { 0, 1, 1,  2,  2,  2,  1,  1,  0 },
        };

        public Bitmap Process(Bitmap image)
        {
            var greyscale = edgeFinder.ConvertToGreyscale(image);
            var processed = new Bitmap(image.Width, image.Height);

            for (int y = 0; y < image.Height; y += 1)
            {
                for (int x = 0; x < image.Width; x += 1)
                {
                    int pixelValue = 0;
                    for (int yy = -kernelSize; yy < kernelSize; yy += 1)
                    {
                        int indexY = y + yy;
                        if (indexY < 0 || indexY >= processed.Height) continue;
                        // if (indexY < 0) indexY = -indexY;
                        // if (indexY >= processed.Height) indexY = y - yy;

                        for (int xx = -kernelSize; xx < kernelSize; xx += 1)
                        {
                            int indexX = x + xx;
                            if (indexX < 0 || indexX >= processed.Width) continue;
                            // if (indexX < 0) indexX = -indexX;
                            // if (indexX >= processed.Width) indexX = x - xx;

                            pixelValue += greyscale.GetPixel(indexX, indexY).R * -kernel[xx + kernelSize][yy + kernelSize];
                        }
                    }
                    pixelValue = pixelValue > 255
                        ? 255
                        : pixelValue < 0
                            ? 0
                            : pixelValue;
                    processed.SetPixel(x, y, Color.FromArgb(pixelValue, pixelValue, pixelValue));
                }
            }

            return processed;
        }

        private int NormalizeToColorRange(int number) =>
            number > 255
                ? 255
                : number < 0
                    ? 0
                    : number;
    }
}
