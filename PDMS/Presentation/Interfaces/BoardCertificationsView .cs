using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IBoardCertificationsView : IView<BoardCertifications>
    {

        void GetRegBoardCertification(DataSet ds);
        //void SetErrorMessages();

    }
}