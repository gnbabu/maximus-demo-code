using MAXIMUS.Models.Data.PDMS;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IAchFeeEditView : IView<AchFeeInformation>
    {
        void SetInsertResults();
        void SetValidationSuccess();
        void SetErrorMessages();
    }
}