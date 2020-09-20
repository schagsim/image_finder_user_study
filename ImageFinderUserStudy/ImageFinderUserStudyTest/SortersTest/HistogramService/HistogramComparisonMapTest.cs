using System;
using System.Collections.Generic;
using ImageFinderUserStudyWeb.DataContracts;
using ImageFinderUserStudyWeb.Services.SorterServices;
using Xunit;

namespace ImageFinderUserStudyTest.SortersTest.HistogramService
{
    public class HistogramComparisonMapTest
    {
        [Fact]
        public void CorrectComparisonMapTest()
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
            
            Assert.True(comparisonMap.Count == 6);
            Assert.True(
                comparisonMap[new HistogramComparisonKeys("randomId1", "randomId2")] - Math.Pow(0.1, 2) < 0.001
                );
            Assert.True(
                comparisonMap[new HistogramComparisonKeys("randomId2", "randomId1")] - Math.Pow(0.1, 2) < 0.001
            );
            Assert.True(
                comparisonMap[new HistogramComparisonKeys("randomId1", "randomId3")] - 2 * Math.Pow(0.1, 2) < 0.001
            );
            Assert.True(
                comparisonMap[new HistogramComparisonKeys("randomId3", "randomId1")] - 2 * Math.Pow(0.1, 2) < 0.001
            );
            Assert.True(
                comparisonMap[new HistogramComparisonKeys("randomId2", "randomId3")] - (Math.Pow(0.1, 2) + Math.Pow(0.2, 2)) < 0.001
            );
            Assert.True(
                comparisonMap[new HistogramComparisonKeys("randomId3", "randomId2")] - (Math.Pow(0.1, 2) + Math.Pow(0.2, 2)) < 0.001
            );
        }
    }
}