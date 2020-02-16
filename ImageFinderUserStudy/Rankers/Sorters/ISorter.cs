namespace Rankers.Sorters
{
    /// <summary>
    /// Interface for different sorters for matrix output.
    /// </summary>
    interface ISorter
    {
        /// <summary>
        /// Sorts the internal data to a matrix of IDs according to the specific class used to sort them.
        /// </summary>
        /// <returns>
        /// List of item IDs.
        /// </returns>
        string[,] Sort();
    }
}