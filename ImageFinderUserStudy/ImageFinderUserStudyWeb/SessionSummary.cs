using System;
using System.Collections.Generic;

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
        public int NumberOfPicturesPresented { get; }
        public int NumberOfPicturesPerRow { get; }
        public List<string> PictureIdsPresented { get; }
        public SessionSummary(
            UserSessionInfo userSession
        )
        {
            UserSessionId = userSession.UserSessionId;
            // TODO: Add this to user session.
            GalleryWidthPixels = 700;
            // TODO: Add this to user session.
            GalleryHeightPixels = 500;
            // TODO: Add this to user session.
            NumberOfPicturesPresented = 20;
            // TODO: Add this to user session.
            NumberOfPicturesPerRow = 4;
            // TODO: Add this to user session.
            PictureIdsPresented = new List<string>
            {
                "v00095_s00015(f006905-f007357)_g00030_f007132",
                "v00271_s00058(f012158-f012291)_g00124_f012257",
                "v00403_s00032(f005112-f005661)_g00047_f00530",
                "v00546_s00038(f003552-f003696)_g00078_f003625",
                "v00691_s00017(f001526-f001639)_g00032_f001588",
                "v00774_s00032(f002653-f002682)_g00072_f002675",
                "v00848_s00289(f055432-f055482)_g00458_f055474",
                "v00997_s00032(f004901-f005500)_g00045_f005034",
                "v01018_s00110(f008237-f008332)_g00160_f008295",
                "v01106_s00024(f010062-f010628)_g00086_f010453",
                "v01160_s00035(f002382-f002453)_g00036_f002425",
                "v01181_s00092(f010661-f010709)_g00116_f010680",
                "v01246_s00006(f000634-f000813)_g00007_f000725",
                "v01310_s00089(f004859-f004902)_g00118_f004860",
                "v01422_s00030(f002288-f002465)_g00045_f002325",
                "v01476_s00057(f003664-f003724)_g00090_f003700",
                "v01512_s00129(f022172-f022201)_g00363_f022177",
                "v01595_s00014(f001693-f001744)_g00031_f001728",
                "v01691_s00019(f012111-f012532)_g00070_f012420",
                "v01762_s00005(f001046-f001358)_g00011_f001108",
                "v01841_s00043(f002002-f002261)_g00056_f002007",
                "v00002_s00062(f004514-f004638)_g00096_f004585",
                "v01891_s00048(f043550-f043771)_g00126_f043666",
                "v01939_s00057(f006915-f007001)_g00138_f006929"
            };
        }
    }
}