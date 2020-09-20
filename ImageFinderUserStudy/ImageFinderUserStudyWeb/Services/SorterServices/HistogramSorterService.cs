using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageFinderUserStudyWeb.DataContracts;
using Newtonsoft.Json;

namespace ImageFinderUserStudyWeb.Services.SorterServices
{
    public class HistogramSorterService
    {
        public List<ColorHistogram> ParseHistograms(string pathToHistogramsFolder)
        {
            return Directory
                .GetFiles(pathToHistogramsFolder)
                .Select(colorHistogramFile => JsonConvert.DeserializeObject<ColorHistogram>(File.ReadAllText(colorHistogramFile)))
                .ToList();
        }
        
        /// <summary>
        /// Selects N+1 random image color histograms.
        /// The "+1" is there in case we want to select a distinct image from the N selected.
        /// </summary>
        /// <returns>New list of ColorHistograms selected.</returns>
        private static List<ColorHistogram> SelectRandomImages(
            int numberOfImagesToPresent,
            IReadOnlyList<ColorHistogram> colorHistograms
        )
        {
            if (numberOfImagesToPresent + 1 > colorHistograms.Count)
            {
                throw new ArgumentException("Cannot select more image labels than the number of loaded labels");
            }
            
            var randomSelector = new Random(DateTime.Now.Ticks.GetHashCode());
            var possibleIndexes = new List<int>();
            for (var i = 0; i < colorHistograms.Count; i++)
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
                .Select(t => new ColorHistogram(
                    new string(colorHistograms[t].ImageId),
                    new List<double>(colorHistograms[t].BlueHistogram),
                    new List<double>(colorHistograms[t].GreenHistogram),
                    new List<double>(colorHistograms[t].RedHistogram)
                ))
                .ToList();
        }

        /// <summary>
        /// Go through the differences in two color histograms, square them a sum all this.
        /// </summary>
        /// <returns>Sum of the histogram differences squared.</returns>
        private static double HistogramDifferenceSum(
            ColorHistogram histogram1,
            ColorHistogram histogram2
        )
        {
            if (histogram1.BlueHistogram.Count != histogram2.BlueHistogram.Count
                || histogram1.GreenHistogram.Count != histogram2.GreenHistogram.Count
                || histogram1.RedHistogram.Count != histogram2.RedHistogram.Count)
            {
                throw new ArgumentException("Histograms do not have the same range of colors.");
            }
            double blueDifferenceSum = 0;
            for (var colorRangeIndex = 0; colorRangeIndex < histogram1.BlueHistogram.Count; colorRangeIndex++)
            {
                blueDifferenceSum += 
                    Math.Sqrt(
                        histogram1.BlueHistogram[colorRangeIndex] - histogram2.BlueHistogram[colorRangeIndex]
                        );
            }
            double greenDifferenceSum = 0;
            for (var colorRangeIndex = 0; colorRangeIndex < histogram1.GreenHistogram.Count; colorRangeIndex++)
            {
                greenDifferenceSum += 
                    Math.Sqrt(
                        histogram1.GreenHistogram[colorRangeIndex] - histogram2.GreenHistogram[colorRangeIndex]
                    );
            }
            double redDifferenceSum = 0;
            for (var colorRangeIndex = 0; colorRangeIndex < histogram1.RedHistogram.Count; colorRangeIndex++)
            {
                redDifferenceSum += 
                    Math.Sqrt(
                        histogram1.RedHistogram[colorRangeIndex] - histogram2.RedHistogram[colorRangeIndex]
                    );
            }

            return blueDifferenceSum + greenDifferenceSum + redDifferenceSum;
        }

        private static int FindTheClosestHistogramIndex(
            string imageId,
            IReadOnlyList<ColorHistogram> possibleHistograms,
            IReadOnlyDictionary<HistogramComparisonKeys, double> comparedHistograms
        )
        {
            if (possibleHistograms.Count == 0)
            {
                throw new ArgumentException("Cannot select from empty possible Histograms list.");
            }
            var currentIndex = 0;
            var currentClosestVal =
                comparedHistograms[new HistogramComparisonKeys(imageId, possibleHistograms[currentIndex].ImageId)];

            for (var i = 1; i < possibleHistograms.Count; i++)
            {
                var currentComparison =
                    comparedHistograms[new HistogramComparisonKeys(imageId, possibleHistograms[i].ImageId)];
                if (!(currentComparison < currentClosestVal)) continue;
                currentClosestVal = currentComparison;
                currentIndex = i;
            }

            return currentIndex;
        }

        private static string[,] SortHistograms(
            int numberOfRows,
            int numberOfColumns,
            IReadOnlyDictionary<HistogramComparisonKeys, double> comparedHistograms,
            IReadOnlyCollection<ColorHistogram> selectedHistograms
        )
        {
            if (numberOfRows < 1 || numberOfColumns < 1)
            {
                throw new ArgumentException("Rows and columns dimensions have to be at least 1x1.");
            }
            if (selectedHistograms.Count != numberOfRows * numberOfColumns)
            {
                throw new ArgumentException("Number of selected histograms has to be the same as number of cells.");
            }

            var histograms = selectedHistograms
                .Select(h => new ColorHistogram(
                    h.ImageId,
                    new List<double>(h.BlueHistogram),
                    new List<double>(h.GreenHistogram),
                    new List<double>(h.RedHistogram)
                ))
                .ToList();
            var randomGen = new Random(DateTime.Now.Ticks.GetHashCode());
            
            // Create a boolean two-dimensional array holding info which cells have already been queued.
            var queuedCells = new bool[numberOfRows, numberOfColumns];
            for (var row = 0; row < numberOfRows; row++)
            {
                for (var column = 0; column < numberOfColumns; column++)
                {
                    queuedCells[row, column] = false;
                }
            }
            
            var filledGallery = new string[numberOfRows, numberOfColumns];
            // Create a queue.
            var queuedCellsQueue = new Queue<QueueData>();
            var randomIndex = randomGen.Next(0, histograms.Count);
            var firstHistogramId = histograms[randomIndex].ImageId;
            histograms.RemoveAt(randomIndex);
            
            // Queue the first cell ([0, 0]) with random image ID and watch the magic happen.
            queuedCellsQueue.Enqueue(
                new QueueData(
                    firstHistogramId,
                    new Tuple<int, int>(0, 0)
                    )
                );
            queuedCells[0, 0] = true;
            filledGallery[0, 0] = firstHistogramId;

            while (queuedCellsQueue.Count > 0)
            {
                var currentCell = queuedCellsQueue.Dequeue();
                
                var nextHistogramIndex =
                    FindTheClosestHistogramIndex(
                        currentCell.SourceImageId,
                        histograms,
                        comparedHistograms
                        );
                var nextImageId = histograms[nextHistogramIndex].ImageId;
                filledGallery[currentCell.CellCoordinates.Item1, currentCell.CellCoordinates.Item2] = nextImageId;
                histograms.RemoveAt(nextHistogramIndex);

                void TryQueueCellsAndMark(
                    int newRow,
                    int newColumn,
                    int maxRows,
                    int maxColumns,
                    Queue<QueueData> queue,
                    bool[,] queuedItems
                    )
                {
                    if (queuedCells[newRow, newColumn] || newRow >= maxRows || newColumn >= maxColumns) return;
                    queue.Enqueue(
                        new QueueData(
                            nextImageId,
                            new Tuple<int, int>(newRow, newColumn)
                        ));
                    queuedItems[newRow, newColumn] = true;
                }
                
                TryQueueCellsAndMark(
                    currentCell.CellCoordinates.Item1,
                    currentCell.CellCoordinates.Item2 + 1,
                    numberOfRows,
                    numberOfColumns,
                    queuedCellsQueue,
                    queuedCells
                    );
                TryQueueCellsAndMark(
                    currentCell.CellCoordinates.Item1 + 1,
                    currentCell.CellCoordinates.Item2 + 1,
                    numberOfRows,
                    numberOfColumns,
                    queuedCellsQueue,
                    queuedCells
                );
                TryQueueCellsAndMark(
                    currentCell.CellCoordinates.Item1 + 1,
                    currentCell.CellCoordinates.Item2,
                    numberOfRows,
                    numberOfColumns,
                    queuedCellsQueue,
                    queuedCells
                );
            }

            return filledGallery;
        }

        public SortersDtos.SorterOutput SortHistograms(
            int numberOfImagesPresentedPerRow,
            int numberOfRowsInGallery,
            double probabilityOfPresentedImageInGallery,
            IReadOnlyList<ColorHistogram> colorHistograms
        )
        {
            var numberOfImagesToPresent = numberOfRowsInGallery * numberOfImagesPresentedPerRow;
            
            if (numberOfImagesToPresent < 0 || numberOfImagesToPresent > colorHistograms.Count)
            {
                throw new ArgumentException(
                    $"Number of presented images has to be between 0 and number of labels loaded ({colorHistograms.Count})."
                );
            }
            
            if (probabilityOfPresentedImageInGallery < 0 || probabilityOfPresentedImageInGallery > 1)
            {
                throw new ArgumentException(
                    "Probability of image being in the presented gallery has to be between 0 and 1."
                );
            }

            var selectedColorHistograms =
                SelectRandomImages(
                    numberOfImagesToPresent,
                    colorHistograms
                );
            
            // TODO: This part could be done in a more general way so all the sorter services can use this.
            var rnd = new Random(DateTime.Now.Ticks.GetHashCode());
            var presentedImage =
                rnd.NextDouble() > probabilityOfPresentedImageInGallery
                    ? selectedColorHistograms[selectedColorHistograms.Count - 1]
                    : selectedColorHistograms[rnd.Next(0, selectedColorHistograms.Count - 1)];
            // Remove the last color histogram. We no longer need it.
            selectedColorHistograms.RemoveAt(selectedColorHistograms.Count - 1);

            var comparedHistograms = new Dictionary<HistogramComparisonKeys, double>();
            for (
                var colorHistogramIndex = 0;
                colorHistogramIndex < selectedColorHistograms.Count;
                colorHistogramIndex++
                )
            {
                for (
                    var secondColorHistogramIndex = colorHistogramIndex + 1;
                    secondColorHistogramIndex < selectedColorHistograms.Count;
                    secondColorHistogramIndex++
                    )
                {
                    var histogramComparison =
                        HistogramDifferenceSum(
                            selectedColorHistograms[colorHistogramIndex],
                            selectedColorHistograms[secondColorHistogramIndex]
                        );
                    comparedHistograms[new HistogramComparisonKeys(
                        selectedColorHistograms[colorHistogramIndex].ImageId,
                        selectedColorHistograms[secondColorHistogramIndex].ImageId
                        )] = histogramComparison;
                    comparedHistograms[new HistogramComparisonKeys(
                        selectedColorHistograms[secondColorHistogramIndex].ImageId,
                        selectedColorHistograms[colorHistogramIndex].ImageId
                    )] = histogramComparison;
                }
            }
            
            /*
             We will display the gallery in the following way.
             We select a random image and place it to the coordinates [0, 0].
             After that, we put adjacent unfilled images to a FIFO queue.
             We will hold an two-dimensional array of booleans to remember which positions are already in the queue.
             We start popping the unfilled images from the queue.
             For each such popped image, we find the most similar image (from comparedHistograms) and put it there.
             Push all adjacent unfilled images into the queue.
            */ 
            return new SortersDtos.SorterOutput(
                presentedImage.ImageId,
                SortHistograms(
                    numberOfRowsInGallery,
                    numberOfImagesPresentedPerRow,
                    comparedHistograms,
                    selectedColorHistograms
                    )
                );
        }
    }
}