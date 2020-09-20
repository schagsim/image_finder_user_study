using System;

namespace ImageFinderUserStudyWeb
{
    public class UserSessionInfo
    {
        public Guid UserSessionId { get; }
        
        public string PresentedImageId { get; }
        
        public string[,] PresentedGallery { get; } 

        public UserSessionInfo(
            Guid sessionGuid,
            string presentedImageId,
            string[,] presentedGallery
            )
        {
            UserSessionId = sessionGuid;
            PresentedImageId = presentedImageId;
            PresentedGallery = presentedGallery;
        }
    }
}