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

        /// <summary>
        /// Selects N+1 random image labels.
        /// The "+1" is there in case we want to select a distinct image from the N selected.
        /// </summary>
        /// <returns>New list of ImageLabels selected.</returns>
        private static List<ImageLabels> SelectRandomImages(
            int numberOfImagesToPresent,
            IReadOnlyList<ImageLabels> imageLabels
        )
        {
            if (numberOfImagesToPresent > imageLabels.Count)
            {
                throw new ArgumentException("Cannot select more image labels than the number of loaded labels");
            }
            
            var randomSelector = new Random(DateTime.Now.Ticks.GetHashCode());
            var possibleIndexes = new List<int>();
            for (var i = 0; i < imageLabels.Count; i++)
            {
                possibleIndexes.Add(i);
            }
            // Now we generate N+1 positions.
            var selectedIndexes = new List<int>();
            while (selectedIndexes.Count < numberOfImagesToPresent + 1)
            {
                var currentIndex = randomSelector.Next(0, possibleIndexes.Count);
                possibleIndexes.RemoveAt(currentIndex);
                selectedIndexes.Add(currentIndex);
            }

            return selectedIndexes
                .Select(t => new ImageLabels(
                    new string(imageLabels[t].ImageId),
                    new Dictionary<string, double>(imageLabels[t].LabelValues)))
                .ToList();
        }

        private static IEnumerable<string> GetTopNLabels(int numberOfTopLabels, ImageLabels imageLabels)
        {
            if (numberOfTopLabels > imageLabels.LabelValues.Count)
            {
                throw new ArgumentException("Cannot select more label values than the number of labels loaded");
            }
            
            return imageLabels.LabelValues
                    .OrderByDescending(x => x.Value)
                    .Take(numberOfTopLabels)
                    .Select(t => t.Key)
                    .ToList();
        }

        /// <summary>
        /// Sum all relevant labels in the @imageLabels container.
        /// </summary>
        /// <returns></returns>
        private static double SumOfRelevantLabels(
            IEnumerable<string> relevantLabels,
            ImageLabels imageLabels
        )
        {
            return imageLabels.LabelValues
                .Where(t => relevantLabels.Contains(t.Key))
                .Sum(t => t.Value);
        }
        
        public SortersDtos.SorterOutput SortLabels(
            int numberOfImagesPresentedPerRow,
            int numberOfRowsInGallery,
            double probabilityOfPresentedImageInGallery,
            IReadOnlyList<ImageLabels> imageLabels
        )
        {
            var numberOfImagesToPresent = numberOfRowsInGallery * numberOfImagesPresentedPerRow;
            
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
            
            // Now we select random N+1 image labels.
            var selectedImageLabels = SelectRandomImages(
                numberOfImagesToPresent,
                imageLabels
            );
            
            // TODO: This part could be done in a more general way so all the sorter services can use this.
            var rnd = new Random(DateTime.Now.Ticks.GetHashCode());
            var presentedImage =
                rnd.NextDouble() > probabilityOfPresentedImageInGallery
                    ? selectedImageLabels[selectedImageLabels.Count - 1]
                    : selectedImageLabels[rnd.Next(0, selectedImageLabels.Count - 1)];
            selectedImageLabels.RemoveAt(selectedImageLabels.Count - 1); // Remove the last image label. We no longer need it.
            
            // Now we get the top three labels of the presented image and sort the rest of the image labels according to that.
            var topThreeLabels = GetTopNLabels(3, presentedImage);
            var mapImageLabelsToRelevancy =
                selectedImageLabels
                    .Select(t => new Tuple<ImageLabels, double>(t, SumOfRelevantLabels(topThreeLabels, t)))
                    .OrderByDescending(t => t.Item2)
                    .Select(t => t.Item1.ImageId)
                    .ToList();
            
            // And now we convert to the final array (image gallery).
            var currentIndex = 0;
            var sortedGallery = new string[numberOfRowsInGallery, numberOfImagesPresentedPerRow];
            for (var row = 0; row < numberOfRowsInGallery; row++)
            {
                for (var column = 0; column < numberOfImagesPresentedPerRow; column++)
                {
                    sortedGallery[row, column] = mapImageLabelsToRelevancy[currentIndex];
                    currentIndex++;
                }
            }

            return new SortersDtos.SorterOutput(
                presentedImage.ImageId,
                sortedGallery
            );
        }
    }
}