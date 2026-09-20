using System;

namespace MAXIMUS.DataExchange.PDMS
{
    public class Utility
    {
        public static string ConvertPartyCategoryTypeToMMISCode(int partyCategoryTypeId)
        {
            switch (partyCategoryTypeId)
            {
                case 1:
                    return "I";
                case 2:
                    return "G";
                case 3:
                    return "E";
                case 4:
                    return "E";
                default:
                    throw new Exception("Party type id not found!");
            }
        }
    }
}
