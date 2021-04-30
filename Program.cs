using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GaussianBlur
{
    public static class Program
    {
        private static LaplacianOfGaussian LoG = new LaplacianOfGaussian();
        public static void Main(string[] args)
        {
            var keyValuePairs = ParseArgs(args);
            var image = new Bitmap(keyValuePairs["-i"]);
            var processed = LoG.Process(image);

            processed.Save(keyValuePairs["-o"]);
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
                keyValuePairs["-o"] = $"{outputName[0]}_colors.{outputName[1]}";
            }

            return keyValuePairs;
        }
    }
}
