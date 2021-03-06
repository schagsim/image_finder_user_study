using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ImageFinderUserStudyWeb.Pages
{
    public class SummaryPage : PageModel
    {
        private readonly UserSessionsManager _userSessionsManager;
        
        private UserSessionInfo UserSession { get; set; }

        public SessionSummary UserSessionSummary { get; private set; }

        public SummaryPage(
            UserSessionsManager userSessionsManager
        )
        {
            _userSessionsManager = userSessionsManager;
        }
        
        public void OnGetSummary(
            Guid userSessionGuid,
            string imageId
        )
        {
            try
            {
                System.IO.Directory.CreateDirectory("userSessions");
                UserSession = _userSessionsManager.UserSessions[userSessionGuid];
                UserSessionSummary = new SessionSummary(UserSession);
                var filePath = "userSessions/" + UserSession.UserSessionId + ".json";
                // TODO: We will later move this to DB instead of file output so it can be queried.
                System.IO.File.WriteAllText(
                    filePath,
                    JsonConvert.SerializeObject(UserSessionSummary)
                );
            }
            catch (KeyNotFoundException e)
            {
                // TODO: Add a logger, not a console logging.
                Console.WriteLine(e);
            }
            // TODO: Add a better exception catching? The "WriteAllText" can throw like 10 exceptions.
            catch (Exception eDefault)
            {
                // TODO: Add a logger, not a console logging.
                Console.WriteLine(eDefault);
            }
        }
    }
}