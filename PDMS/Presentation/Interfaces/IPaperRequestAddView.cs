using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IPaperRequestAddView : IView<PaperRequestQueueData>
    {
        void SetPaperRequestTypes(DataSet ds);
        void SetPaperDocumentTypes(DataSet ds);
        void SetPaperRequestStatusTypes(DataSet ds);
        void SetProviderCategories(DataSet ds);
        void SetProviderTypes(DataSet ds);
        void SetSpecialtyTypes(DataSet ds);
        void SetTaxonomyTypes(DataView dt);
        void SetPaperRequestDetail(PaperRequestQueueData data);
        void SetWaiverInfo(string name);
        void SetExceptionLog(DataSet ds);
        void SetPaperApplicationTypes(DataSet ds);
       
        void SetInsertResults();
        void SetUpdateResults(PaperRequestQueueData data);
        void SetValidationSuccess();
        void SetErrorMessages();
    }
}