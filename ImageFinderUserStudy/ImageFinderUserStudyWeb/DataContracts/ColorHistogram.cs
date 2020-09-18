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

    public class HistogramComparisonKeys
    {
        public string Key1 { get; }
        public string Key2 { get; }

        public override bool Equals(object? obj)
        {
            if (!(obj is HistogramComparisonKeys secondHistogram)) return false;
            return Key1 == secondHistogram.Key1 && Key2 == secondHistogram.Key2;
        }

        public override int GetHashCode() => new {Key1, Key2}.GetHashCode();

        public HistogramComparisonKeys(
            string key1,
            string key2
        )
        {
            Key1 = key1;
            Key2 = key2;
        }
    }
}