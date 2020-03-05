using System;
using System.IO;
using System.Linq;
using Rankers.Parsers;
using Xunit;

namespace RankersTest
{
    public class ColorHistogramParserTest
    {
        private const string TestFilesFolder = @"HistogramTestFiles";
        
        [Fact]
        public void CorrectInputParse()
        {
            // Get the directory in which the tests are contained.
            var pathToHistogramFiles = $"{Directory.GetCurrentDirectory()}/../../../{TestFilesFolder}";
            
            // Parse the files in the gotten directory.
            var parseOutput = ColorHistogramParser.ReadAndParseColorHistograms(pathToHistogramFiles);
            
            var testFiles = Directory.GetFiles(pathToHistogramFiles);

            Assert.True(testFiles.Length == parseOutput.Count);
            var imageIds = parseOutput.Select(histogram => histogram.ImageId).ToList();
            foreach (var testFile in testFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(testFile);
                Assert.Contains(fileName, imageIds);
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