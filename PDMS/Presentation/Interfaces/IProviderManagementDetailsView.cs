using MAXIMUS.Models.Data.PDMS;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IProviderManagementDetailsView : IView<ProviderManagementData>
    {
        void SetProviderDetails(ProviderManagementData details);
        void BeginNewWorkflow(ProviderManagementData keyData);
        void CompleteCancelWorkflow();
        void SetErrorMessages();
        
    }
}