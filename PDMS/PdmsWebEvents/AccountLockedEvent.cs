using System.Web.Management;

namespace PdmsWebEvents
{
    public class AccountLockedEvent : WebAuthenticationFailureAuditEvent
    {
        public AccountLockedEvent(string msg, object eventSource, string nameToAuthenticate)
            : base(msg, eventSource, 100003, nameToAuthenticate)
        {
        }

        public AccountLockedEvent(string msg, object eventSource, int eventCode, string nameToAuthenticate)
            : base(msg, eventSource, eventCode, nameToAuthenticate)
        {
        }

        public AccountLockedEvent(string username, string msg, object eventSource, int eventDetailCode, string nameToAuthenticate)
            : base(msg, eventSource, 100003, eventDetailCode,
                  nameToAuthenticate)
        {
        }

        public AccountLockedEvent(string username, string msg, object eventSource, int eventCode, int eventDetailCode, string nameToAuthenticate)
            : base(msg, eventSource, eventCode, eventDetailCode,
                  nameToAuthenticate)
        {
        }
    }
}
