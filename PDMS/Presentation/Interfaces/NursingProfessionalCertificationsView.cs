using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface INursingProfessionalCertificationsView : IView<NursingProfessionalCertifications>
    {

        void GetRegNursingProfessionalCertification(DataSet ds);
        //void SetErrorMessages();

    }
}