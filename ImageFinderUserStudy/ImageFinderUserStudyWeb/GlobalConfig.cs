namespace ImageFinderUserStudyWeb
{
    public class GlobalConfig
    {
        public int GalleryWidthPixels { get; }

        public int GalleryHeightPixels { get; }
        
        public int GalleryScrollBarWidthPixels { get; }

        public GlobalConfig(
            int galleryWidthPixels,
            int galleryHeightPixels,
            int scrollBarWidthPixels
        )
        {
            GalleryWidthPixels = galleryWidthPixels;
            GalleryHeightPixels = galleryHeightPixels;
            GalleryScrollBarWidthPixels = scrollBarWidthPixels;
        }
    }
}