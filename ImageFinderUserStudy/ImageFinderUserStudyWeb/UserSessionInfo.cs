using System;

namespace ImageFinderUserStudyWeb
{
    public class UserSessionInfo
    {
        public Guid UserSessionId { get; private set; }

        public UserSessionInfo(Guid sessionGuid)
        {
            UserSessionId = sessionGuid;
        }
    }
}