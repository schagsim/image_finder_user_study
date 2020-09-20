using System.Collections.Generic;
using System.Linq;
using ImageFinderUserStudyWeb.DataContracts;
using ImageFinderUserStudyWeb.Services.SorterServices;
using Xunit;

namespace ImageFinderUserStudyTest.SortersTest.LabelService
{
    public class GetTopNLabelsTest
    {
        [Fact]
        public void CorrectGetTopNTest()
        {
            var labelSorterService = new LabelSorterService();
            
            var labelValues = new Dictionary<string, double>
            {
                ["car"] = 0.1,
                ["dark"] = 0.2,
                ["wheel"] = 0.3,
                ["sky"] = 0.4
            };

            var imageLabels = new ImageLabels(
                "randomId1",
                labelValues
            );

            var top2LabelValues = labelSorterService.GetTopNLabels(2, imageLabels);
            Assert.True(top2LabelValues.Count == 2);
            Assert.True(top2LabelValues[0] == "sky");
            Assert.True(top2LabelValues[1] == "wheel");
        }
    }
}