using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IDCSAdminSearchView : IView<DCSProviderSearch>
    {
        void SetProviderCategories(DataSet categories);
        void SetProviderTypes(DataSet providerTypes);
        void SetProviderSearchResults(DataSet providers);
        void SetValidationSuccess();
        void SetErrorMessages();

    }
}
