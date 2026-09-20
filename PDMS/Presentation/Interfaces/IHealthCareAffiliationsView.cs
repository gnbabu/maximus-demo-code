using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IHealthCareAffiliationsView : IView<HealthCareAffiliations>
    {

        void GetRegHealthCareFacilityAffiliation(DataSet ds);
        //void SetErrorMessages();

    }
}