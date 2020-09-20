using System;
using System.IO;
using System.Linq;
using ImageFinderUserStudyWeb.Services.SorterServices;
using Xunit;

namespace ImageFinderUserStudyTest.SortersTest.LabelService
{
    public class ParseLabelsTest
    {
        private const string TestFilesFolder = @"TestResources/TestImageLabels";
        private const int NumberOfHistogramsInTestFolder = 24;
        private const string TestImageId = "v00006_s00033(f004957-f005173)_g00078_f005125";
        private const int NumberOfTestLabels = 15;
        
        [Fact]
        public void CorrectParseLabelsTest()
        {
            var labelService = new LabelSorterService();
            
            // Get the directory in which the test files are contained.
            var pathToHistogramFiles = $"{Directory.GetCurrentDirectory()}/../../../{TestFilesFolder}";

            var parseOutput = labelService.ParseLabels(pathToHistogramFiles);
            
            var testFileNames =
                Directory
                    .GetFiles(pathToHistogramFiles)
                    .Select(Path.GetFileNameWithoutExtension)
                    .ToList();
            
            var imageIds =
                parseOutput
                    .Select(imageLabels => imageLabels.ImageId)
                    .ToList();
            
            Assert.True(testFileNames.Count == NumberOfHistogramsInTestFolder);
            Assert.True(testFileNames.Count == parseOutput.Count);
            foreach (var testFileName in testFileNames)
            {
                Assert.Contains(testFileName, imageIds);
            }

            try
            {
                var testImageLabels = parseOutput.Find(imageLabels => imageLabels.ImageId == TestImageId);
                Assert.True(testImageLabels?.LabelValues.Count == NumberOfTestLabels);
            }
            catch (NullReferenceException)
            {
                Assert.True(false);
            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }
    }
}