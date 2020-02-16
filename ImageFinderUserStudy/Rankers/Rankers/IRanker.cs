using System.Collections.Generic;

namespace Rankers.Rankers
{
    /// <summary>
    /// Interface for different rankers. Every ranker will select top N items from input.
    /// The method how the top N are chosen will differ for every ranker.
    /// </summary>
    interface IRanker
    {
        /// <summary>
        /// Filters label vectors. The output will be a dictionary of IDs with the found top relevancy.
        /// </summary>
        /// <param name="numberOfItemsInOutput">
        /// Number of items to the output.
        /// </param>
        /// <param name="queryParams">
        /// List of label IDs from which the query consists.
        /// </param>
        /// <returns></returns>
        Dictionary<string, double> RankWithRelevancyOutput(uint numberOfItemsInOutput, List<string> queryParams);

        /// <summary>
        /// Filters label vectors. The output will be just top N relevant items without the relevancy data included.
        /// </summary>
        /// <param name="numberOfItemsInOutput">
        /// Number of items to the output.
        /// </param>
        /// /// <param name="queryParams">
        /// List of label IDs from which the query consists.
        /// </param>
        /// <returns></returns>
        List<string> RankWithoutRelevancyOutput(uint numberOfItemsInOutput, List<string> queryParams);
    }
}