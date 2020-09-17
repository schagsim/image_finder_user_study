using System.Collections.Generic;

namespace ImageFinderUserStudyWeb.DataContracts
{
    public class SortersDtos
    {
        public class SorterOutput
        {
            public string PresentedImageId { get; }
            public string[,] PresentedImageGallerySorted { get; }

            public SorterOutput(
                string presentedImageId,
                string[,] presentedImageGallerySorted
            )
            {
                PresentedImageId = presentedImageId;
                PresentedImageGallerySorted = presentedImageGallerySorted;
            }
        }
    }
}