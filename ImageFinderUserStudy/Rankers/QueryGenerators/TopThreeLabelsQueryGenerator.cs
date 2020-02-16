using System.Collections.Generic;
using System.Linq;

namespace Rankers.QueryGenerators
{
    public class TopThreeLabelsQueryGenerator : IQueryGenerator
    {
        public List<string> GenerateQueryFromLabels(ImageLabels imageLabels)
        {
            var sortedLabels = 
                imageLabels.LabelValues.OrderByDescending(x => x.Value).ToList();

            var queriesWithLabels = sortedLabels.Take(3).ToList();

            return queriesWithLabels.Select(t => t.Key).ToList();
        }
    }
}