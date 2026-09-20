using FileHelpers;
using MAXIMUS.Core.Libraries;

namespace MAXIMUS.DataExchange.PDMS.MMISInterface
{
    /// <summary>
    ///     Description:    This c# class is used by the MMIS classes to generate records for the 
    ///                     retrieve waiver provider process.
    /// </summary>
    /// <remarks>
    ///     Helpful Links:  http://www.filehelpers.com/attributes.html
    /// </remarks>
    [IgnoreEmptyLines(true)]
    [FixedLengthRecord(FixedMode.AllowLessChars)]
    public class RetrieveWaiverProvider : MMISRequest
    {
        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Left, Constants.TrimCharNFOCUS)]
        public string transactionId;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Left, Constants.TrimCharNFOCUS)]
        public string pdmsProviderId;

        [FieldFixedLength(8)]
        [FieldTrim(TrimMode.Left, Constants.TrimCharNFOCUS)]
        public string organizationId;

        [FieldOptional()]
        [FieldFixedLength(50)]
        [FieldTrim(TrimMode.Left, Constants.TrimCharNFOCUS)]
        public string statusCode;

        public string StatusCode { get { return statusCode == "0" ? string.Empty : statusCode; } }
        public string OrganizationId { get { return organizationId == "0" ? string.Empty : organizationId; } }
    }
}