using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IProviderManagerView : IView<ProviderManagerData>
    {
        void SetMyProviders(DataSet ds, int totalResultCount);
        void SetMyGroupMemberProfiles(DataSet ds, int totalResultCount);
        void SetPendingProviders(DataSet ds, int totalResultCount); //converted providers
        void SetMyPendingReferrals(DataSet ds, int totalResultCount);
        void SetErrorMessages();
        void SetApplicationTypes(DataSet ds);
        void SetAllProviders(DataSet ds, int totalResultCount);
    }
}