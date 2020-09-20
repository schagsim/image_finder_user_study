using System;
using System.Collections.Generic;

namespace ImageFinderUserStudyWeb
{
    public class UserSessionsManager
    {
        public Dictionary<Guid, UserSessionInfo> UserSessions { get; }

        public UserSessionsManager()
        {
            UserSessions = new Dictionary<Guid, UserSessionInfo>();
        }

        public void AddNewSessionInfo(
            Guid userSessionId
        )
        {
            var newUserSession =
                new UserSessionInfo(
                    userSessionId
                );
        }
    }
}