using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rankers.Parsers
{
    public static class LabelParser
    {
        public static List<ImageLabels> ReadAndParseImageLabels(string pathToLabelsFolder)
        {
            if (pathToLabelsFolder == null)
            {
                throw new ArgumentNullException(nameof(pathToLabelsFolder));
            }

            var parsedImageLabels = new List<ImageLabels>();

            var labelFilesNames = Directory.GetFiles(pathToLabelsFolder);
            foreach (var labelFileName in labelFilesNames)
            {
                string[] currentLabelsFile;
                try
                {
                    var labelsFile = File.ReadAllText($"{labelFileName}");
                    currentLabelsFile = Regex.Split(labelsFile, "(, \")|(,\")")
                        .Select(x => x.Trim(new char[] { '"', '{', '}', ',' }))
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .ToArray();
                }
                catch (Exception)
                {
                    Console.WriteLine($"Couldn't read file {labelFileName}.");
                    continue;
                }

                var currentLabels = new Dictionary<string, double>();
                foreach (var t in currentLabelsFile)
                {
                    var splitLabel = Regex.Split(t, "(:)")
                        .Select(x => x.Trim(new char[] { '"', '{', '}', ',', ':' }))
                        .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                    if (splitLabel.Length != 2) goto NEXT;

                    double.TryParse(splitLabel[1], out double labelVal);
                    currentLabels.Add(splitLabel[0], labelVal);
                }
                parsedImageLabels.Add(
                    new ImageLabels(
                            Path.GetFileNameWithoutExtension(labelFileName),
                            currentLabels
                        )
                    );
                NEXT:;
            }

            return parsedImageLabels;
        }
    }
}