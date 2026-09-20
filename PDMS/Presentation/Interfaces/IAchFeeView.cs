using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IAchFeeView : IView<AchFeeInformation>
    {
        void SetProviderInfo(Provider provider);
        void SetFeeHistory(DataSet ds);
        void SetErrorMessages();

    }
}