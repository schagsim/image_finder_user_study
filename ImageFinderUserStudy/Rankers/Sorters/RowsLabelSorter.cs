using System;
using System.Collections.Generic;
using System.Linq;

namespace Rankers.Sorters
{
    /// <summary>
    /// This class sorts the input of pairs (itemId, relevancy) into a matrix.
    /// The matrix has the most relevant object at [0,0] and the second most relevant at [0,1], etc.
    /// </summary>
    public class RowsLabelSorter : ISorter
    {
        private readonly List<string> _sortedIds;
        private readonly uint _numberOfColumns;

        public RowsLabelSorter(Dictionary<string, double> mapIdsToRelevancy, uint numberOfColumns)
        {
            if (numberOfColumns <= 0) throw new ArgumentException("Number of columns needs to be > 0.");

            var mapIdsToRelevancyOrdered = 
                mapIdsToRelevancy.OrderByDescending(x => x.Value).ToArray();
            _sortedIds = new List<string>();

            foreach (var idValuePair in mapIdsToRelevancyOrdered)
            {
                _sortedIds.Add(idValuePair.Key);
            }

            _numberOfColumns = numberOfColumns;
        }

        public string[,] Sort()
        {
            var numberOfRows = _sortedIds.Count % _numberOfColumns == 0
                ? (uint)_sortedIds.Count / _numberOfColumns
                : ((uint)_sortedIds.Count / _numberOfColumns) + 1;

            var sortedIds = new string[numberOfRows, _numberOfColumns];
            var idCount = 0;
            for (var i = 0; i < numberOfRows; i++)
            {
                for (var j = 0; j < _numberOfColumns; j++)
                {
                    sortedIds[i, j] = idCount < _sortedIds.Count ? _sortedIds[idCount] : "";
                    idCount++;
                }
            }
            return sortedIds;
        }
    }
}