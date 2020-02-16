using System.Collections.Generic;

namespace Rankers.QueryGenerators
{
    public interface IQueryGenerator
    {
        List<string> GenerateQueryFromLabels(ImageLabels imageLabels);
    }
}