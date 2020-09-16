using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ImageFinderUserStudyWeb.DataContracts;

namespace ImageFinderUserStudyWeb.Services.SorterServices
{
    public class LabelSorterService
    {
        public List<ImageLabels> ParseLabels(string pathToLabelsFolder)
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

        private List<ImageLabels> selectRandomImages(
            int numberOfImagesToPresent,
            List<ImageLabels> imageLabels
        )
        {
            throw new NotImplementedException();
        }
        
        public SortersDtos.SorterOutput SortLabels(
            int numberOfImagesToPresent,
            double probabilityOfPresentedImageInGallery,
            List<ImageLabels> imageLabels
        )
        {
            if (numberOfImagesToPresent < 0 || numberOfImagesToPresent > imageLabels.Count)
            {
                throw new ArgumentException(
                    $"Number of presented images has to be between 0 and number of labels loaded ({imageLabels.Count})."
                );
            }
            
            if (probabilityOfPresentedImageInGallery < 0 || probabilityOfPresentedImageInGallery > 1)
            {
                throw new ArgumentException(
                    "Probability of image being in the presented gallery has to be between 0 and 1."
                );
            }
            
            throw new NotImplementedException();
        }
    }
}