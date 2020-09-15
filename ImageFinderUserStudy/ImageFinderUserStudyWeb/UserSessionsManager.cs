using System;
using System.Collections.Generic;

namespace ImageFinderUserStudyWeb
{
    public class UserSessionsManager
    {
        public Dictionary<Guid, UserSessionInfo> UserSessions { get; private set; }

        public UserSessionsManager()
        {
            UserSessions = new Dictionary<Guid, UserSessionInfo>();
        }
    }
}