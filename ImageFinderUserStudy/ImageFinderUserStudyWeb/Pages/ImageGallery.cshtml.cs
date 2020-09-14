using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageFinderUserStudyWeb.Pages
{
    public class ImageGallery : PageModel
    {
        private readonly GlobalConfig _globalConfigHolder;
        
        public int GalleryWidthPixels { get; private set; }

        public int GalleryHeightPixels { get; private set; }
        
        public int GalleryScrollBarPixels { get; private set; }
        
        public static string SizeInPixels(int size)
        {
            return $"{size}px";
        }
        
        public ImageGallery(
            GlobalConfig globalConfig
        )
        {
            _globalConfigHolder = globalConfig;
        }
        
        public void OnGet()
        {
            GalleryWidthPixels = _globalConfigHolder.GalleryWidthPixels;
            GalleryHeightPixels = _globalConfigHolder.GalleryHeightPixels;
            GalleryScrollBarPixels = _globalConfigHolder.GalleryScrollBarWidthPixels;
        }
    }
}