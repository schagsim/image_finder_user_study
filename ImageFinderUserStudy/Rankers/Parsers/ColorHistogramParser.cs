using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Rankers.Parsers
{
    public class ColorHistogramParser
    {
        public static List<ColorHistogram> ReadAndParseColorHistograms(string pathToHistogramsFolder)
        {
            if (pathToHistogramsFolder == null)
            {
                throw new ArgumentNullException(nameof(pathToHistogramsFolder));
            }

            var colorHistogramFiles = Directory.GetFiles(pathToHistogramsFolder);

            return colorHistogramFiles
                .Select(colorHistogramFile => JsonConvert.DeserializeObject<ColorHistogram>(File.ReadAllText(colorHistogramFile)))
                .ToList();
        }
    }
}