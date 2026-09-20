using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IProviderAddView : IView<ProviderManagementData>
    {
        void SetRecreateSuccess();
        void SetDeleteOnlySuccess();
        void SetKeyFieldUpdateResults();
        void SetConvertedProviderData(ProviderManagementData data);
        void SetExistingProviderData(ProviderManagementData data);
        void SetPaperRequestData(PaperRequestQueueData data);
        void SetMultipleMedicaidsFound(DataTable dt);
        void SetProviderCategories(DataView dv);
        void SetProviderTypes(DataView dv);
        void SetSpecialtyTypes(DataSet ds);
        void SetTaxonomyTypes(DataView dv);
        void SetPracticeTypes(DataView dv);
        void SetValidationSuccess();
        void SetErrorMessages();
        void SetExistingReferralData(ReferralData data);
        void SetApplicationTypes(DataView dv, DataView dv1);
        void SetTaxonomyTypesFromNPPES(Result rs, bool showTaxonomyDropDown);
    }
}