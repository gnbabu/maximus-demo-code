using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IProviderTerminateView : IView<ProviderManagementData>
    {
        void SetWorkflowCreatedResults();
        void SetErrorMessages();
        void SetProviderInformation(ProviderManagementData data);
        void SetUpdateResults();
        void SetValidationSuccess();
        void SetEnrollmentStatuses(DataView dv);
    }
}