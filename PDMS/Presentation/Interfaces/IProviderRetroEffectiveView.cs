using MAXIMUS.Models.Data.PDMS;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IProviderRetroEffectiveView : IView<ProviderManagementData>
    {
        void SetProviderInformation(ProviderManagementData data);
        void SetUpdateResults();
        void SetValidationSuccess();
        void SetErrorMessages();

    }
}