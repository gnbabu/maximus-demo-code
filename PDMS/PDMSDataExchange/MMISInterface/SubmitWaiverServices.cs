using FileHelpers;
using System;

namespace MAXIMUS.DataExchange.PDMS.MMISInterface
{
    /// <summary>
    ///     Description:    This c# class is used by the MMIS classes to generate records for the 
    ///                     submit waiver provider service type process.
    /// </summary>
    /// <remarks>
    ///     Helpful Links:  http://www.filehelpers.com/attributes.html
    /// </remarks>
    [IgnoreEmptyLines(true)]
    [DelimitedRecord("|")]
    public class SubmitWaiverServices : MMISRequest
    {
        public string actionFlag;

        public string transactionId;

        public string pdmsProviderId;

        public string organizationId;

        public string serviceType;

        public string programCode;

        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? programBeginDate;

        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? programEndDate;
    }
}
