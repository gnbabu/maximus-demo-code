using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MAXIMUS.Core.Libraries;

namespace MAXIMUS.DataExchange.PDMS.MMISInterface
{  
    /// <summary>
    ///     Description:    This c# class is used by the MMIS classes to generate records for the 
    ///                     retrieve waiver provider service type process.
    /// </summary>
    /// <remarks>
    ///     Helpful Links:  http://www.filehelpers.com/attributes.html
    /// </remarks>
    [IgnoreEmptyLines(true)]
    [FixedLengthRecord(FixedMode.AllowLessChars)]
    public class RetrieveWaiverProviderServiceType : MMISRequest
    {
        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimCharNFOCUS)]
        public string transactionId;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimCharNFOCUS)]
        public string pdmsProviderId;
                
        [FieldFixedLength(8)]
        [FieldTrim(TrimMode.Both, Constants.TrimCharNFOCUS)]
        public string organizationId;

        [FieldFixedLength(4)]
        [FieldTrim(TrimMode.Both, Constants.TrimCharNFOCUS)]
        public string serviceType;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimCharNFOCUS)]
        public string programCode;

        [FieldOptional()]
        [FieldFixedLength(50)]
        [FieldTrim(TrimMode.Both, Constants.TrimCharNFOCUS)]
        public string statusCode;
    }
}