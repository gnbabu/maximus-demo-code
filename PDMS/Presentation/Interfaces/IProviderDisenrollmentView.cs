using MAXIMUS.Models.Data.PDMS;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IProviderDisenrollmentView : IView<ProviderManagementData>
    {
        void SetProviderInformation(ProviderManagementData data);
        void SetUpdateResults();
        void SetValidationSuccess();
        void SetErrorMessages();

    }
}