using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface ICPRCertificationsView : IView<CPRCertifications>
    {

        void GetRegCPRCertification(DataSet ds);
        //void SetErrorMessages();

    }
}