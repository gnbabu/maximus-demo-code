using System.Runtime.Serialization;
/// <summary>
/// Summary description for Constants
/// </summary>
namespace MAXIMUS.Services.PDMS
{

    [DataContract]
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



    }
}

