using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IOwnerConvictionOnBehalfView : IView<OwnerConvictionOnBehalf>
    {

        void SetOwnerConvictionOnBehalf(DataSet ds);
        //void SetErrorMessages();

    }
}