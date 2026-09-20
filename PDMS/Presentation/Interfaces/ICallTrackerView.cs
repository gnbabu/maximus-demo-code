using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface ICallTrackerView : IView<CallTrackingData>
    {
        void SetCallReasons(DataSet callReasons);
        void SetCallResolutions(DataSet callResolutions);
        void SetCallSources(DataSet callSources);
        void SetErrorMessages();
        void SetInsertResults();
        void SetValidationSuccess();

    }
}
