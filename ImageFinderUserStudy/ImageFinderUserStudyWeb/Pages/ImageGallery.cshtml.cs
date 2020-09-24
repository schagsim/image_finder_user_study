using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageFinderUserStudyWeb.Pages
{
    public class ImageGallery : PageModel
    {
        private readonly GlobalConfig _globalConfigHolder;
        private UserSessionsManager _userSessionsManager;
        
        public Guid UserSessionId { get; private set; }
        
        public int GalleryWidthPixels { get; }

        public int GalleryHeightPixels { get; }
        
        public int GalleryScrollBarPixels { get; }
        
        public int ImageWidthPixels { get; }
        
        public int NumberOfPicturesInRow { get; }
        public int NumberOfRows { get; }
        
        public string PresentedImageId { get; private set; }
        public string[,] ImageGalleryMatrix { get; private set; }
        
        public string SizeInPixels(int size)
        {
            return $"{size}px";
        }
        
        public ImageGallery(
            GlobalConfig globalConfig,
            UserSessionsManager userSessionsManager
        )
        {
            _globalConfigHolder = globalConfig;
            _userSessionsManager = userSessionsManager;
            
            GalleryWidthPixels = globalConfig.GalleryWidthPixels;
            GalleryHeightPixels = globalConfig.GalleryHeightPixels;
            GalleryScrollBarPixels = globalConfig.GalleryScrollBarWidthPixels;
            ImageWidthPixels = globalConfig.ImageDimensionsPixels;
            NumberOfPicturesInRow = globalConfig.NumberOfColumns;
            NumberOfRows = globalConfig.NumberOfRows;
        }
        
        public string ImagePath(string imageId)
        {
            return @"Resources/ImageFiles/" + $"{imageId}.jpg";
        }
        
        public void OnGetGallery(
            Guid userSessionGuid
            )
        {
            var userSession = _userSessionsManager.UserSessions[userSessionGuid];
            UserSessionId = userSessionGuid;
            PresentedImageId = userSession.PresentedImageId;
            ImageGalleryMatrix = userSession.PresentedGallery;
            userSession.GalleryPresentationTimeTicks = DateTime.Now.Ticks;
        }

        public IActionResult OnPostImageClick(
            Guid userSessionGuid,
            string imageId
        )
        {
            _userSessionsManager.UserSessions[userSessionGuid].GalleryAnswerTimeTicks = DateTime.Now.Ticks;
            _userSessionsManager.UserSessions[userSessionGuid].ImageIdFound = imageId;
            // TODO: Here, set the time, set the found image to session, etc.
            return RedirectToPage("SummaryPage", "Summary", new { userSessionGuid = userSessionGuid, imageId = imageId });
        }
    }
}