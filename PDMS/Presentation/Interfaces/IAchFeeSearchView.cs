using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IAchFeeSearchView : IView<AchFeeSearch>
    {
        void SetACHFeeSearchResults(DataSet fees);
        void SetValidationSuccess();
        void SetErrorMessages();

    }
}
