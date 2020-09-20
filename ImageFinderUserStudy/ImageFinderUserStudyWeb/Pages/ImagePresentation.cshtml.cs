using System;
using ImageFinderUserStudyWeb.DataContracts;
using ImageFinderUserStudyWeb.Services.SorterServices;
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
            return "http://herkules.ms.mff.cuni.cz/lineit/20k_images/images/" + $"{imageId}.jpg";
        }
        
        public void OnGet()
        {
            var newSessionId = Guid.NewGuid();
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
        }
    }
}