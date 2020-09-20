using System.Collections.Generic;
using ImageFinderUserStudyWeb.DataContracts;

namespace ImageFinderUserStudyWeb
{
    public class LoadedHistogramsWrapper
    {
        public LoadedHistogramsWrapper(List<ColorHistogram> loadedHistograms)
        {
            LoadedColorHistograms = loadedHistograms;
        }
        public List<ColorHistogram> LoadedColorHistograms { get; }
    }
}