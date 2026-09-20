/// <summary>
/// Summary description for Constants
/// </summary>
public class Constants
{
    public enum BitValues : uint
    {
        NoBits = 0,
        Bit1 = 0x00000001,
        Bit2 = 0x00000002,
        Bit3 = 0x00000004,
        Bit4 = 0x00000008,
        Bit5 = 0x00000010,
        AllBits = 0xFFFFFFFF
    }

    // Status passed to tell the FCR Service how to handle the storing of the record
    public static class RecordStatus
    {
        public static int Read = 1;
        public static int Create = 2;
        public static int Update = 3;
        public static int Delete = 4;
        public static int Inactive = 5;
    }

    // Resolving Action
    public static class ResolvingActionType
    {
        public static int Rejection = 1;
        public static int AdditionalInfo = 2;
        public static int Termination = 3;
        public static int Override = 4;
        public static int UpdateRecord = 5;
        public static int Warning = 6;
    }
    public static class RegistrationStatustype
    {
        public static int NotSubmitted = 1;
        public static int Submitted = 2;
        public static int Approved = 3;
        public static int Denied = 4;
        public static int ReturntoProvider = 5;
        public static int Terminated = 6;
        public static int Deleted = 7;
        public static int ReturntoProviderServices = 8;
        public static int StateReview = 9;
        public static int SiteVisit = 10;
        public static int Complete = 11;
    }
    public static class PNMStatus
    {
        public static int New = 1;
        public static int Maintenance = 2;
        public static int Approved = 3;
        public static int Terminated = 4;
        public static int Denied = 5;
        public static int Conversion = 6;
        public static int Revalidation = 7;
        public static int MonthlyScreening = 8;
        public static int Disenrolled = 9;
        public static int PendingTermination = 10;
        public static int EnforceMoratoria = 11;
        public static int AnnualScreening = 12;
    }

    public static class EnrollmentType
    {
        public static string AdHocComment = "Ad-hoc Comment";
        public static string DataFix = "Data Fix (Superuser)";
        public static string Job = "Job";
    }

    public static class FinalDisposition
    {
        public static string Other = "Other";
    }
}