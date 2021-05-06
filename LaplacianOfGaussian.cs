using System;
using System.Drawing;

namespace GaussianBlur
{
    public class LaplacianOfGaussian
    {
        private const double Sigma = 1;
        private EdgeFinder edgeFinder;
        private double[,] kernel;
        private int kernelSize;
        public LaplacianOfGaussian(int kernelSize = 3)
        {
            edgeFinder = new EdgeFinder();
            this.kernelSize = kernelSize;
            CalculateKernel();
        }

        private void CalculateKernel()
        {
            kernel = new double[2 * kernelSize + 1, 2 * kernelSize + 1];
            for (int x = -kernelSize; x < kernelSize; x += 1)
            {
                for (int y = -kernelSize; y < kernelSize; y += 1)
                {
                    double sumOfSquares = Math.Pow(x, 2) + Math.Pow(y, 2);
                    double numerator = 1 - sumOfSquares / (2 * Math.Pow(Sigma, 2));
                    double denominator = Math.PI * Math.Pow(Sigma, 4);
                    double exponent = -sumOfSquares / (2 * Math.Pow(Sigma, 2));

                    kernel[x + kernelSize, y + kernelSize] = -numerator / denominator * Math.Pow(Math.E, exponent);
                }
            }
        }


        public Bitmap Process(Bitmap image)
        {
            var greyscale = edgeFinder.ConvertToGreyscale(image);
            var processed = new Bitmap(image.Width, image.Height);

            for (int y = 0; y < image.Height; y += 1)
            {
                for (int x = 0; x < image.Width; x += 1)
                {
                    double pixelValueAsDouble = 0;
                    for (int yy = -kernelSize; yy < kernelSize; yy += 1)
                    {
                        int indexY = y + yy;
                        if (indexY < 0 || indexY >= processed.Height) continue;

                        for (int xx = -kernelSize; xx < kernelSize; xx += 1)
                        {
                            int indexX = x + xx;
                            if (indexX < 0 || indexX >= processed.Width) continue;

                            pixelValueAsDouble += (double)greyscale.GetPixel(indexX, indexY).R * -kernel[xx + kernelSize, yy + kernelSize];
                        }
                    }
                    int pixelValue = NormalizeToColorRange(pixelValueAsDouble);
                    processed.SetPixel(x, y, Color.FromArgb(pixelValue, pixelValue, pixelValue));
                }
            }

            return processed;
        }

        private int NormalizeToColorRange(double number) =>
            number > 255
                ? 255
                : number < 0
                    ? 0
                    : (int)number;
    }
}
