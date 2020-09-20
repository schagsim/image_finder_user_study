using System.IO;
using System.Linq;
using ImageFinderUserStudyWeb.Services.SorterServices;
using Xunit;

namespace ImageFinderUserStudyTest.SortersTest.HistogramService
{
    public class ParseHistograms
    {
        private const string TestFilesFolder = @"TestResources/TestColorHistograms";
        private const int NumberOfHistogramsInTestFolder = 24;
        
        [Fact]
        public void CorrectParse()
        {
            var histogramService = new HistogramSorterService();
            
            // Get the directory in which the test files are contained.
            var pathToHistogramFiles = $"{Directory.GetCurrentDirectory()}/../../../{TestFilesFolder}";
            
            // Parse the files in the test directory.
            var parseOutput = histogramService.ParseHistograms(pathToHistogramFiles);
            
            var testFileNames =
                Directory
                    .GetFiles(pathToHistogramFiles)
                    .Select(Path.GetFileNameWithoutExtension)
                    .ToList();
            
            var imageIds =
                parseOutput
                    .Select(histogram => histogram.ImageId)
                    .ToList();

            Assert.True(testFileNames.Count == NumberOfHistogramsInTestFolder);
            Assert.True(testFileNames.Count == parseOutput.Count);
            foreach (var testFileName in testFileNames)
            {
                Assert.Contains(testFileName, imageIds);
            }

            foreach (var colorHistogram in parseOutput)
            {
                Assert.True(colorHistogram.RedHistogram.Count == 16);
                Assert.True(colorHistogram.GreenHistogram.Count == 16);
                Assert.True(colorHistogram.BlueHistogram.Count == 16);
            }
        }
    }
}