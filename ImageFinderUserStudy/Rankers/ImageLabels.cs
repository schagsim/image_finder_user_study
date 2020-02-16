using System.Collections.Generic;

namespace Rankers
{
    /// <summary>
    /// This class holds information about the labels of one image.
    /// Labels are in a format "label": [double value].
    /// </summary>
    public class ImageLabels
    {
        public string ImageId { get; private set; }
        public Dictionary<string, double> LabelValues { get; private set; }

        public ImageLabels(string imageId, Dictionary<string, double> labelvalues)
        {
            ImageId = imageId;
            LabelValues = labelvalues;
        }
    }
}