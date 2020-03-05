using System.Collections.Generic;

namespace Rankers
{
    /// <summary>
    /// This class holds information about the labels of one image.
    /// Labels are in a format "label": [double value].
    /// </summary>
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