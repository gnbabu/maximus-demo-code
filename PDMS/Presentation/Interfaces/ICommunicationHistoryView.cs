using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface ICommunicationHistoryView : IView<CommunicationEventData>
    {
        void SetCommunicationHistory(DataSet ds);
        void SetEmailAttachments(DataSet ds);
        void SetContactEmailInfo(DataSet ds);
        void SetCredentialingEmailInfo(DataSet ds);
        void SetErrorMessages();

    }
}