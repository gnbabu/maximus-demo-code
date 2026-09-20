using MAXIMUS.Models.Data.PDMS;
using System.Collections.Generic;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IPartyEligibilityEditView  : IView<PartyEligibility>
    {
        //This interface is implemented by the web form

        //Presenter calls these methods to return results to View (web form)
        void SetEligibilityGroups(DataSet eligibilityGroups);
        void SetEligibilityDetails();
        void SetValidationSuccess();
        void SetEligibilityInsertResults();
        void SetEligibilityUpdateResults();
        void SetErrorMessages();
        void SetAllEligibilities(List<int> eligibilityIDs);

    }
}
