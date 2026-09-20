using MAXIMUS.Models.Data.PDMS;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IProviderReactivationView : IView<ProviderManagementData>
    {
        void SetWorkflowCreatedResults();
        void SetErrorMessages();
        void SetProviderInformation(ProviderManagementData data);
        void SetUpdateResults();
        void SetValidationSuccess();
    }
}