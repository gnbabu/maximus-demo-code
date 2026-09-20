using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface ICallReportView : IView<CallTrackingData>
    {
        void SetCallReasons(DataSet callReasons);
        void SetCallResolutions(DataSet callResolutions);
        void SetCallSources(DataSet callSources);
        void SetCallData(DataSet callData, int outputType);
        void SetErrorMessages();
    }
}
