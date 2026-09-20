using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IMalpracticeClaimView : IView<MalpracticeClaimInfo>
    {

        void GetMalpracticeClaimInfo(DataSet ds);
        //void SetErrorMessages();

    }
}