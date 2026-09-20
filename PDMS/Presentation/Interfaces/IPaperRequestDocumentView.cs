using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IPaperRequestDocumentView : IView<PaperRequestDocumentData>
    {
        void SetMyDocuments(DataSet data);
        void SetInsertSuccess();
        void SetErrorMessages();

    }
}