using System.Collections.Generic;
using ImageFinderUserStudyWeb.DataContracts;

namespace ImageFinderUserStudyWeb
{
    public class ImageLabelsWrapper
    {
        public ImageLabelsWrapper(List<ImageLabels> loadedLabels)
        {
            LoadedImageLabels = loadedLabels;
        }
        public List<ImageLabels> LoadedImageLabels { get; }
    }
}