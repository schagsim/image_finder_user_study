using System.Collections.Generic;

namespace Rankers
{
    public class ColorHistogram
    {
        public string ImageId { get; }
        
        public Dictionary<string, double> BlueHistogram { get; }
        
        public Dictionary<string, double> GreenHistogram { get; }
        
        public Dictionary<string, double> RedHistogram { get; }

        public ColorHistogram
        (
            string imageId,
            Dictionary<string, double> blueHistogram,
            Dictionary<string, double> greenHistogram,
            Dictionary<string, double> redHistogram
        )
        {
            ImageId = imageId;
            BlueHistogram = blueHistogram;
            GreenHistogram = greenHistogram;
            RedHistogram = redHistogram;
        }
    }
}