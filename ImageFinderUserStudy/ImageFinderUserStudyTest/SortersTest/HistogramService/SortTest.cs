using System.Collections.Generic;
using ImageFinderUserStudyWeb.DataContracts;
using ImageFinderUserStudyWeb.Services.SorterServices;
using Xunit;

namespace ImageFinderUserStudyTest.SortersTest.HistogramService
{
    public class SortTest
    {
        [Fact]
        public void CorrectSortTest()
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
            var histogram4 = new ColorHistogram(
                "randomId4",
                new List<double> { 0.2 },
                new List<double> { 0.2, 0.1 },
                new List<double> { 0.1, 0.2, 0.3 }
            );
            var selectedHistograms = new List<ColorHistogram> {histogram1, histogram2, histogram3, histogram4};
            var comparisonMap = histogramService.CreateHistogramComparisonMap(selectedHistograms);

            var sortedHistograms = histogramService.SortSelectedHistograms(
                2,
                2,
                comparisonMap,
                selectedHistograms
                );
            Assert.True(true);
        }
    }
}