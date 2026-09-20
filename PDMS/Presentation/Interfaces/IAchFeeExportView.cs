using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IAchFeeExportView : IView<AchFeeSearch>
    {
        void SetACHFeeInformation(DataSet fees);
        void SetErrorMessages();
    }

}
