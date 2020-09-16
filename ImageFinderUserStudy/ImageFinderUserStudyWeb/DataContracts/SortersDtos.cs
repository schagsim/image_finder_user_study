using System.Collections.Generic;

namespace ImageFinderUserStudyWeb.DataContracts
{
    public class SortersDtos
    {
        public class SorterOutput
        {
            public string PresentedImageId { get; }
            public List<string> PresentedImageGallerySorted { get; }

            public SorterOutput(
                string presentedImageId,
                List<string> presentedImageGallerySorted
            )
            {
                PresentedImageId = presentedImageId;
                PresentedImageGallerySorted = presentedImageGallerySorted;
            }
        }
    }
}