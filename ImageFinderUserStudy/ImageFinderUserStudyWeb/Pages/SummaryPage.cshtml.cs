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

        public SummaryPage(
            UserSessionsManager userSessionsManager
        )
        {
            _userSessionsManager = userSessionsManager;
        }
        
        public void OnGetSummaryPageAsync(
            Guid userSessionId
        )
        {
            try
            {
                UserSession = _userSessionsManager.UserSessions[userSessionId];
                var filePath = "userSessions/" + UserSession.UserSessionId + ".json";
                // TODO: We will later move this to DB instead of file output so it can be queried.
                System.IO.File.WriteAllText(
                    filePath,
                    JsonConvert.SerializeObject(new SessionSummary(UserSession))
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