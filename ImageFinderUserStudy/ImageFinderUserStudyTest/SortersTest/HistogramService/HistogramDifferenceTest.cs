using System;
using System.Collections.Generic;
using ImageFinderUserStudyWeb.DataContracts;
using ImageFinderUserStudyWeb.Services.SorterServices;
using Xunit;

namespace ImageFinderUserStudyTest.SortersTest.HistogramService
{
    public class HistogramDifferenceTest
    {
        [Fact]
        public void CorrectDifferenceTest()
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
                new List<double> { 0.1, 0.2, 0.3 }
                );
            var difference = histogramService.HistogramDifferenceSum(histogram1, histogram2);
            Assert.True(difference == 0);
            
            var histogram3 = new ColorHistogram(
                "randomId3",
                new List<double> { 0.1 }, 
                new List<double> { 0.1, 0.1 }, 
                new List<double> { 0.1, 0.2, 0.2 }
            );
            difference = histogramService.HistogramDifferenceSum(histogram1, histogram3);
            Assert.True(difference - 2 * Math.Pow(0.1, 2) < 0.001);
        }

        [Fact]
        public void IncorrectRangeTest()
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
                new List<double> { 0.1, 0.2 }, 
                new List<double> { 0.1, 0.2 }, 
                new List<double> { 0.1, 0.2, 0.3 }
            );

            try
            {
                histogramService.HistogramDifferenceSum(histogram1, histogram2);
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