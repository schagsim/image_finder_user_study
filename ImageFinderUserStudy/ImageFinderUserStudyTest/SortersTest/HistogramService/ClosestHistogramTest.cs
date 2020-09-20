using System.Collections.Generic;
using ImageFinderUserStudyWeb.DataContracts;
using ImageFinderUserStudyWeb.Services.SorterServices;
using Xunit;

namespace ImageFinderUserStudyTest.SortersTest.HistogramService
{
    public class ClosestHistogramTest
    {
        [Fact]
        public void CorrectClosestHistogramTest()
        {
            var histogramService = new HistogramSorterService();
            var histogram1 = new ColorHistogram(
                "randomId1",
                new List<double> { 0.1 },
                new List<double> { 0.1, 0.2 },
                new List<double> { 0.1, 0.2, 0.3 }
            );
            var histogram2 = new ColorHistogram(
                "randomId2",
                new List<double> { 0.1 }, 
                new List<double> { 0.1, 0.2 },
                new List<double> { 0.1, 0.2, 0.2 }
            );
            var histogram3 = new ColorHistogram(
                "randomId3",
                new List<double> { 0.1 },
                new List<double> { 0.1, 0.1 },
                new List<double> { 0.1, 0.2, 0.4 }
            );
            var selectedHistograms = new List<ColorHistogram> {histogram1, histogram2, histogram3};
            var comparisonMap = histogramService.CreateHistogramComparisonMap(selectedHistograms);
            selectedHistograms.RemoveAt(0);

            var closestIndex = histogramService.FindTheClosestHistogramIndex(
                "randomId1",
                selectedHistograms,
                comparisonMap
            );
            Assert.True(closestIndex == 0);
        }
    }
}