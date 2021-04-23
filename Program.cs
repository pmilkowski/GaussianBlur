using System;
using System.Collections.Generic;
using System.Linq;

namespace GaussianBlur
{
    public static class Program
    {
        public static void Main(string[] args)
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
                keyValuePairs["-o"] = $"{outputName[0]}_greyscale.{outputName[1]}";
            }

            // var blur = new ImageBlur(5);
            // blur.BlurImage(keyValuePairs["-i"], keyValuePairs["-o"]);

            var edgeFinder = new EdgeFinder();
            edgeFinder.DetectEdges(keyValuePairs["-i"], keyValuePairs["-o"]);
        }
    }
}
