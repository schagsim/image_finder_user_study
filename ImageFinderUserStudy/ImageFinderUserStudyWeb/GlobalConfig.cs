namespace ImageFinderUserStudyWeb
{
    public enum GalleryType
    {
        Unknown,
        ImageLabels,
        ColorHistograms,
        SemanticVectors
    }
    
    public class GlobalConfig
    {
        public int GalleryWidthPixels { get; }

        public int GalleryHeightPixels { get; }
        
        // Will always be a square. Dimensions are gallery width / number of columns.
        public int ImageDimensionsPixels { get; }
        
        public int GalleryScrollBarWidthPixels { get; }
        
        public int NumberOfRows { get; }
        
        public int NumberOfColumns { get; }
        
        // Value between <0, 1> determining what's the probability of presented image also being in the gallery.
        public double ProbabilityOfPresentedImageInGallery { get; }
        
        public GalleryType GalleryType { get; }

        public GlobalConfig(
            int galleryWidthPixels,
            int galleryHeightPixels,
            int scrollBarWidthPixels,
            int numberOfRows,
            int numberOfColumns,
            double probabilityOfPresentedImageInGallery,
            GalleryType galleryType
        )
        {
            GalleryWidthPixels = galleryWidthPixels;
            GalleryHeightPixels = galleryHeightPixels;
            ImageDimensionsPixels = galleryWidthPixels / numberOfColumns;
            GalleryScrollBarWidthPixels = scrollBarWidthPixels;
            NumberOfRows = numberOfRows;
            NumberOfColumns = numberOfColumns;
            ProbabilityOfPresentedImageInGallery = probabilityOfPresentedImageInGallery;
            GalleryType = galleryType;
        }
    }
}