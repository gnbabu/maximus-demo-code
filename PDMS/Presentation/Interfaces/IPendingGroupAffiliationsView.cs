using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IPendingGroupAffiliationsView : IView<PendingGroupAffiliations>
    {
        void InsertRegPendingAffiliation(DataSet ds);
        void UpdateRegPendingAffiliation(DataSet ds);
        void GetRegHealthCareFacilityAffiliation(DataSet ds);
        //void SetErrorMessages();

    }
}