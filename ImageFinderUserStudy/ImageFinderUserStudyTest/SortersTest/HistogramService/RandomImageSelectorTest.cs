using System;
using System.IO;
using ImageFinderUserStudyWeb.Services.SorterServices;
using Xunit;

namespace ImageFinderUserStudyTest.SortersTest.HistogramService
{
    public class RandomImageSelectorTest
    {
        private const string TestFilesFolder = @"TestResources/TestColorHistograms";
        private const int NumberOfImagesToPresent = 5;
        private const int NumberOfTestImagesForArgumentExc = 25;
        
        [Fact]
        public void CorrectImageSelection()
        {
            var histogramService = new HistogramSorterService();
            var pathToHistogramFiles = $"{Directory.GetCurrentDirectory()}/../../../{TestFilesFolder}";
            var colorHistograms = histogramService.ParseHistograms(pathToHistogramFiles);

            var presentedImages =
                histogramService.SelectRandomImages(NumberOfImagesToPresent, colorHistograms);
            
            Assert.True(presentedImages.Count == NumberOfImagesToPresent);
            for (var i = presentedImages.Count - 1; i >= 0; i--)
            {
                for (var j = i - 1; j >= 0; j--)
                {
                    Assert.True(
                        presentedImages[i].ImageId != presentedImages[j].ImageId,
                        $"{presentedImages[i].ImageId} matches {presentedImages[j].ImageId}"
                        );
                }
                presentedImages.RemoveAt(i);
            }
        }

        [Fact]
        public void IncorrectArgumentImageSelection()
        {
            var histogramService = new HistogramSorterService();
            var pathToHistogramFiles = $"{Directory.GetCurrentDirectory()}/../../../{TestFilesFolder}";
            var colorHistograms = histogramService.ParseHistograms(pathToHistogramFiles);

            try
            {
                histogramService.SelectRandomImages(NumberOfTestImagesForArgumentExc, colorHistograms);
            }
            catch (ArgumentException)
            {
                Assert.True(true);
            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }
    }
}