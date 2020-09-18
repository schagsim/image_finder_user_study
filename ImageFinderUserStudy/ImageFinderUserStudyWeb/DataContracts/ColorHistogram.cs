using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ImageFinderUserStudyWeb.DataContracts
{
    [DataContract]
    public class ColorHistogram
    {
        [DataMember]
        public string ImageId { get; }
    
        [DataMember]
        public List<double> BlueHistogram { get; }
    
        [DataMember]
        public List<double> GreenHistogram { get; }
    
        [DataMember]
        public List<double> RedHistogram { get; }

        public ColorHistogram
        (
            string imageId,
            List<double> blueHistogram,
            List<double> greenHistogram,
            List<double> redHistogram
        )
        {
            ImageId = imageId;
            BlueHistogram = blueHistogram;
            GreenHistogram = greenHistogram;
            RedHistogram = redHistogram;
        }
    }
}