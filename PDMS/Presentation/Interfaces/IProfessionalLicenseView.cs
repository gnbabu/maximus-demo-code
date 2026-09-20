using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IProfessionalLicenseView: IView<ProfessionalLicense>
    {


        void GetRegProfessionalLicenses(DataSet ds);
    }
}
