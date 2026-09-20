namespace PDMSWebAPI.DocumentService
{
    public class DocumentErrorCodes
    {

        public const string E01 = "Invalid document Extension.";
        public const string E02 = "Unable to process request.";
        public const string E03 = "Unauthorized Access.";
        public const string E04 = "Invalid credentials.";
        public const string E05 = "Invalid document xref type.";
        public const string E06 = "Unable to process request."; //SQL Connectivity issue
        public const string E07 = "Unable to process request."; //Failed SQL transaction
        public const string E08 = "Invalid request.";
        public const string E09 = "Invalid File Size.";
        public const string E10 = "Invalid document type.";

        public const string E11 = "The request has errors - Invalid Identifiers.";
        public const string E12 = "The request has errors - Invalid request.";
        public const string E13 = "The request has errors - Invalid PSM Document Type.";
        public const string E14 = "The request has errors - No active provider found in PNM.";
    }
}