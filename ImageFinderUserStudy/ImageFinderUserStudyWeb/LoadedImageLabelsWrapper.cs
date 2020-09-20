using System.Collections.Generic;
using ImageFinderUserStudyWeb.DataContracts;

namespace ImageFinderUserStudyWeb
{
    public class LoadedImageLabelsWrapper
    {
        public LoadedImageLabelsWrapper(List<ImageLabels> loadedLabels)
        {
            LoadedImageLabels = loadedLabels;
        }
        public List<ImageLabels> LoadedImageLabels { get; }
    }
}