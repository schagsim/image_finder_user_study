using System;
using System.Collections.Generic;
using System.Linq;

namespace Rankers.Rankers
{
    public class SumOfRelevantLabelsRanker : IRanker
    {
        private readonly List<ImageLabels> _imageLabels;

        public SumOfRelevantLabelsRanker(List<ImageLabels> imageLabels)
        {
            _imageLabels = imageLabels;
        }

        /// <summary>
        /// Outputs sum of all relevant query parameters.
        /// </summary>
        private static double SumQueries(ICollection<string> queryParams, ImageLabels imageLabels)
        {
            var labels = imageLabels.LabelValues.ToList();

            return labels.Where(t => queryParams.Contains(t.Key)).Sum(t => t.Value);
        }

        public List<string> RankWithoutRelevancyOutput(uint numberOfItemsInOutput, List<string> queryParams)
        {
            return new List<string>(RankWithRelevancyOutput(numberOfItemsInOutput, queryParams).Keys);
        }

        public Dictionary<string, double> RankWithRelevancyOutput(uint numberOfItemsInOutput, List<string> queryParams)
        {
            var outputList = _imageLabels.Select(imageLabel => new Tuple<string, double>(imageLabel.ImageId, SumQueries(queryParams, imageLabel))).ToList();

            return outputList.OrderByDescending(x => x.Item2).Take((int)numberOfItemsInOutput).ToDictionary(x => x.Item1, x => x.Item2);
        }
    }
}