using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IPartyEligibilityView : IView<PartyEligibility>
    {
        void SetProviderDetails(DCSProvider provider);
        void SetProviderEligibilities(DataSet partyEligibilities);
        void SetErrorMessages();
        void SetDeleteSuccess();
        void SetSubmitAffiliationSuccess();

    }
}
