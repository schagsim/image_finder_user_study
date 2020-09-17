using System.Collections.Generic;

namespace ImageFinderUserStudyWeb.DataContracts
{
    public class ImageLabels
    {
        public string ImageId { get; }
        public Dictionary<string, double> LabelValues { get; }

        public ImageLabels(string imageId, Dictionary<string, double> labelValues)
        {
            ImageId = imageId;
            LabelValues = labelValues;
        }
    }
}