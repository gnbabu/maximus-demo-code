using System.ServiceModel;

namespace PDMSWebAPI.CostReports
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICostReports" in both code and config file together.
    [ServiceContract(Namespace = "http://mes.gov/costreports", Name = "CostReports")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    public interface ICostReports
    {
        [OperationContract]
        submitCostSettlementReportResponse submitCostSettlementReport(submitCostSettlementReportRequest settlementRequest);
        //cSubmitCostSettlementReportResponse submitCostSettlementReport(MessageHeader MessageHeader, cSubmitCostSettlementReport Payload); // cSubmitCostSettlementReportResponse submitCostSettlementReport(submitCostSettlementReportRequest Request);

    }
}
