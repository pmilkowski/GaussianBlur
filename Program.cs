using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GaussianBlur
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var keyValuePairs = ParseArgs(args);
            string input = keyValuePairs["-i"];
            var image = new Bitmap(input);

            ProcessLoG(image, input);
            ProcessConvolution(image, input);
        }

        private static void ProcessConvolution(Bitmap image, string input)
        {
            var blur = new ImageBlur(1);
            var edges = new EdgeFinder();
            var convolvedImage = edges.DetectEdges(
                blur.BlurImage(image)
            );
            var output = input.Split('.');
            convolvedImage.Save($"{output[0]}_convolved.{output[1]}");
        }

        private static void ProcessLoG(Bitmap image, string input)
        {
            var log = new LaplacianOfGaussian();
            var logImage = log.Process(image);
            var output = input.Split('.');

            logImage.Save($"{output[0]}_log.{output[1]}");
        }

        private static Dictionary<string, string> ParseArgs(string[] args)
        {
            var keyValuePairs = new Dictionary<string, string>();
            while (args.Any())
            {
                keyValuePairs[args[0]] = args[1];
                args = args.Skip(2).ToArray();
            }
            if (!keyValuePairs.ContainsKey("-i"))
            {
                keyValuePairs["-i"] = "tygrus.jpg";
            }
            if (!keyValuePairs.ContainsKey("-o"))
            {
                var outputName = keyValuePairs["-i"].Split('.');
                keyValuePairs["-o"] = $"{outputName[0]}_LoG.{outputName[1]}";
            }

            return keyValuePairs;
        }
    }
}
