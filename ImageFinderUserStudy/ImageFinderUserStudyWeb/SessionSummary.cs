using System;

namespace ImageFinderUserStudyWeb
{
    /// <summary>
    /// This session summary is a different class from UserSession because not all the data in the user session
    /// 1) Are in the correct format
    /// 2) Are going to be relevant in the summary.
    /// </summary>
    public class SessionSummary
    {
        public Guid UserSessionId { get; }
        public int GalleryWidthPixels { get; }
        public int GalleryHeightPixels { get; }
        public int NumberOfImagesPerRow { get; }
        public int NumberOfRows { get; }
        public int NumberOfImagesPresented { get; }
        public string PresentedImageId { get; }
        public string[,] GalleryPresented { get; }
        public string GalleryType { get; }
        
        public double SecondsToRespond { get; }
        
        public SessionSummary(
            UserSessionInfo userSession
        )
        {
            if (userSession.GalleryPresentationTimeTicks == null || userSession.GalleryAnswerTimeTicks == null)
            {
                throw new ArgumentException("Answer times have to be filled.");
            }
            
            UserSessionId = userSession.UserSessionId;
            
            GalleryWidthPixels = userSession.GalleryWidth;
            GalleryHeightPixels = userSession.GalleryHeight;

            NumberOfImagesPerRow = userSession.NumberOfImagesPerRow;
            NumberOfRows = userSession.NumberOfRows;
            NumberOfImagesPresented = NumberOfImagesPerRow * NumberOfRows;

            PresentedImageId = userSession.PresentedImageId;
            
            GalleryPresented = userSession.PresentedGallery;
            
            GalleryType = userSession.GalleryType switch
            {
                ImageFinderUserStudyWeb.GalleryType.Unknown => "Unknown",
                ImageFinderUserStudyWeb.GalleryType.ImageLabels => "Google Image Labels",
                ImageFinderUserStudyWeb.GalleryType.ColorHistograms => "Color Histograms",
                ImageFinderUserStudyWeb.GalleryType.SemanticVectors => "Semantic Vectors",
                _ => "Unknown"
            };

            SecondsToRespond = (new DateTime((long)userSession.GalleryAnswerTimeTicks) - new DateTime((long)userSession.GalleryPresentationTimeTicks)).TotalSeconds;
        }
    }
}