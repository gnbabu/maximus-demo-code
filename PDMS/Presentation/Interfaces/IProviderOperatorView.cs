using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IProviderOperatorView : IView<PaperRequestQueueData>
    {
        void SetNextInQueue(int paperRequestQueueID);
        void SetNextInQueueNotFound();
        void SetMyProviders(DataSet ds, int totalResultCount);
        void SetMyDashboard(DataSet ds);
        void SetErrorMessages();

    }
}