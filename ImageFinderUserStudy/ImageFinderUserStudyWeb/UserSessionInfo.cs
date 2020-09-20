using System;

namespace ImageFinderUserStudyWeb
{
    public class UserSessionInfo
    {
        public Guid UserSessionId { get; }
        
        public string PresentedImageId { get; }
        
        public string[,] PresentedGallery { get; } 
        
        public GalleryType GalleryType { get; }
        
        public int GalleryWidth { get; }
        public int GalleryHeight { get; }
        public int NumberOfImagesPerRow { get; }
        public int NumberOfRows { get; }

        public UserSessionInfo(
            Guid sessionGuid,
            string presentedImageId,
            string[,] presentedGallery,
            GalleryType galleryType,
            int galleryWidth,
            int galleryHeight,
            int numberOfImagesPerRow,
            int numberOfRows)
        {
            UserSessionId = sessionGuid;
            PresentedImageId = presentedImageId;
            PresentedGallery = presentedGallery;
            GalleryType = galleryType;
            GalleryWidth = galleryWidth;
            GalleryHeight = galleryHeight;
            NumberOfRows = numberOfRows;
            NumberOfImagesPerRow = numberOfImagesPerRow;
        }
    }
}