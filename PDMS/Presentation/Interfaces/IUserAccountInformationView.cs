using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IUserAccountInformationView : IView<UserAccountInformation>
    {
        void SetUserInformation(DataSet ds);
        void SetErrorMessages();

    }
}