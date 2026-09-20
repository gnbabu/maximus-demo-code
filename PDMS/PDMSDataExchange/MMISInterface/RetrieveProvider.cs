using FileHelpers;
using MAXIMUS.Core.Libraries;

namespace MAXIMUS.DataExchange.PDMS.MMISInterface
{
    /// <summary>
    ///     Description:    This c# class is used by the MMIS classes to retrieve records for the 
    ///                     MMIS submit provider process.
    /// </summary>
    /// <remarks>
    ///     Helpful Links:  http://www.filehelpers.com/attributes.html
    /// </remarks>
    [IgnoreEmptyLines(true)]
    [FixedLengthRecord(FixedMode.AllowLessChars)]
    public class RetrieveProvider : MMISRequest
    {
        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string transactionId;

        [FieldFixedLength(9)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string medicaidId;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string pdmsProviderId;


        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string npi;

        [FieldFixedLength(8)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string statusCode;

        [FieldOptional()]
        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string errorCode;

        [FieldOptional()]
        [FieldFixedLength(50)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string rejectedValue;
    }
}
