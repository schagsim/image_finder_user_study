using System;
using ImageFinderUserStudyWeb.Services.SorterServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageFinderUserStudyWeb.Pages
{
    public class ImagePresentation : PageModel
    {
        private readonly GlobalConfig _globalConfig;
        private UserSessionsManager _userSessionsManager;
        private readonly LoadedImageLabelsWrapper _imageLabelsWrapper;
        private readonly LoadedHistogramsWrapper _histogramsWrapper;
        private readonly LabelSorterService _labelSorterService;
        private readonly HistogramSorterService _histogramSorterService;
        
        public string PresentedImageId { get; private set; }
        
        public Guid UserSessionId { get; private set; }
        
        public ImagePresentation(
            GlobalConfig globalConfig,
            UserSessionsManager userSessionsManager,
            LoadedImageLabelsWrapper imageLabelsWrapper,
            LoadedHistogramsWrapper histogramsWrapper,
            LabelSorterService labelSorterService,
            HistogramSorterService histogramSorterService
        )
        {
            _globalConfig = globalConfig;
            _userSessionsManager = userSessionsManager;
            _imageLabelsWrapper = imageLabelsWrapper;
            _histogramsWrapper = histogramsWrapper;
            _labelSorterService = labelSorterService;
            _histogramSorterService = histogramSorterService;
        }
        
        public string ImagePath(string imageId)
        {
            return @"Resources/ImageFiles/" + $"{imageId}.jpg";
        }
        
        public void OnGet()
        {
            UserSessionId = Guid.NewGuid();
            var sortedGallery = _globalConfig.GalleryType switch
            {
                GalleryType.Unknown => throw new ApplicationException("Type of gallery is not specified!"),
                GalleryType.ImageLabels => _labelSorterService.SortLabels(_globalConfig.NumberOfColumns,
                    _globalConfig.NumberOfRows, _globalConfig.ProbabilityOfPresentedImageInGallery,
                    _imageLabelsWrapper.LoadedImageLabels),
                GalleryType.ColorHistograms => _histogramSorterService.SortHistograms(_globalConfig.NumberOfColumns,
                    _globalConfig.NumberOfRows, _globalConfig.ProbabilityOfPresentedImageInGallery,
                    _histogramsWrapper.LoadedColorHistograms),
                GalleryType.SemanticVectors => throw new NotImplementedException(
                    "Semantic vectors are not yet implemented!"),
                _ => throw new ApplicationException("Type of gallery is not specified!")
            };

            PresentedImageId = sortedGallery.PresentedImageId;
            
            var newUserSession = new UserSessionInfo(
                UserSessionId,
                sortedGallery.PresentedImageId,
                sortedGallery.PresentedImageGallerySorted
                );
            
            _userSessionsManager.UserSessions.Add(
                UserSessionId,
                newUserSession
            );
        }
        
        public IActionResult OnPostGallery(Guid userSessionId)
        {
            return RedirectToPage("ImageGallery", "Gallery", new { userSessionGuid = userSessionId });
        }
    }
}