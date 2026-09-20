using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface IOwnerIdentifyingInfoView : IView<OwnerIdentifyingInfo>
    {

        void GetOwnerIdentifyingInfo(DataSet ds);
        //void SetErrorMessages();

    }
}