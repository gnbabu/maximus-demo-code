using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IGroupAndFacilityAffiliationsView : IView<GroupAndFacilityAffiliations>
    {
        void SetRegPendingAffiliation(DataSet ds);
        void SetRegConfirmedAffiliation(DataSet ds);
        void SetRegHealthCareFacilityAffiliation(DataSet ds);
        void SetRegAssignedDelegates(DataSet ds);

        //void SetErrorMessages();

    }
}