using System;
using Microsoft.Extensions.Configuration;

namespace ImageFinderUserStudyWeb.Services
{
    public class GlobalConfigService
    {
        public GlobalConfig LoadConfig(IConfiguration configuration)
        {
            int.TryParse(
                configuration["GallerySettings:GalleryWidthPixels"], out var galleryWidth
                );
            int.TryParse(
                configuration["GallerySettings:GalleryHeightPixels"], out var galleryHeight
                );
            
            // TODO: The scroll bars in the gallery can be different. Is there a good way how to solve it?
            //    The problem with the added pixel width is that if we don't add the additional pixels, the table
            //    is not going to fit the table wrapper and a side scrollbar in the bottom appears.
            int.TryParse(
                configuration["GallerySettings:GalleryScrollBarPixels"], out var scrollBarPixels
                );
            double.TryParse(
                configuration["GallerySettings:PresentedImageInGalleryProbability"],
                out var probabilityOfPresentedImageInGallery
                );
            int.TryParse(
                configuration["GallerySettings:NumberOfRows"], out var numberOfRows
                );
            int.TryParse(
                configuration["GallerySettings:NumberOfRows"], out var numberOfColumns
                );
            int.TryParse(
                configuration["GallerySettings:GalleryType"], out var galleryTypeValue
                );


            if (probabilityOfPresentedImageInGallery < 0 || probabilityOfPresentedImageInGallery > 1)
            {
                throw new ArgumentException("Probability of image presented being in the gallery in the config has to be in the interval <0, 1>.");
            }
            if (numberOfRows < 1)
            {
                throw new ArgumentException("Number of rows in the gallery in the config cannot be less than 1.");
            }
            if (numberOfColumns < 1)
            {
                throw new ArgumentException("Number of columns in the gallery in the config cannot be less than 1.");
            }
            
            var galleryType = galleryTypeValue switch
            {
                1 => GalleryType.ImageLabels,
                2 => GalleryType.ColorHistograms,
                3 => throw new NotImplementedException(
                    "Gallery type Semantic vectors in config is not yet implemented."),
                _ => GalleryType.Unknown
            };

            return new GlobalConfig(
                galleryWidth,
                galleryHeight,
                scrollBarPixels,
                numberOfRows,
                numberOfColumns,
                probabilityOfPresentedImageInGallery,
                galleryType
            );
        }
    }
}