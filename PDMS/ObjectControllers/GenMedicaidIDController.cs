using MAXIMUS.Core.Libraries;
using System;
using System.Text.RegularExpressions;

namespace MAXIMUS.Controllers.PDMS
{
    public static class GenMedicaidIDController
    {
        // Moved regular expression pattern to a read only variable to see if this helps to mitigate the Sonarqube security hotspots medium issue
        private static readonly string pattern = @"([a-zA-Z]+)(\d+)";
        private static string cleanInput(string input)
        {
            Regex re = new Regex(pattern);
            Match result = re.Match(input);
            string alphaPart = result.Groups[1].Value;
            string numberPart = result.Groups[2].Value;
            string newString = numberPart.PadLeft(6, '0');
            string cleanInput = String.Concat(alphaPart, newString);
            return cleanInput;
        }

        private static string cleanInputDigits(string input)
        {
            string cleanInput = input.PadLeft(9, '0');
            return cleanInput;
        }

        public static string GenerateNextMedicaid(string medicaid, bool checkLTC = false)
        {
            string nextMedID = string.Empty;
            if (checkLTC)
            {
                nextMedID = GetNextLTCMedicaidId(medicaid);
            }
            else
            {
                nextMedID = GetNextMedicaidId(medicaid);
            }
            return nextMedID;
        }

        private static string GetNextMedicaidId(string seed)
        {
            string nextId = string.Empty;
            if (seed == "9999999")
            {
                Logging log = new Logging();
                log.CreateLogEntry("No more Medicaid ID left", Logging.LogPriority.Error);
                throw new ArgumentOutOfRangeException("I can handle till 9999999 only!");
            }

            int medID = 0;
            if (Int32.TryParse(seed, out medID))
            {
                medID = medID + 1;
                nextId = medID.ToString().PadLeft(7, '0');
            }
            else
            {
                Logging log = new Logging();
                log.CreateLogEntry("Error while converting!", Logging.LogPriority.Error);
                throw new ArgumentOutOfRangeException("Error while converting!");
            }
            return nextId;
        }

        private static string GetNextLTCMedicaidId(string seed)
        {
            seed = cleanInput(seed);
            var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string nextId = string.Empty;

            if (seed == "LTC999999")
                throw new ArgumentOutOfRangeException("I can handle till LTC999999 only!");
            var intPart = int.Parse(seed.Substring(3, 6));
            if (intPart < 999999)
                nextId = seed.Substring(0, 3) + (++intPart).ToString();
            else
            {
                var char2idx = letters.IndexOf(seed.Substring(2, 1));
                if (char2idx < letters.Length - 1)
                    nextId = seed.Substring(0, 2) + letters.Substring(++char2idx, 1) + "000000";
                else
                {
                    var char1idx = letters.IndexOf(seed.Substring(1, 1));
                    if (char2idx == 25)
                    {
                        if (char1idx == 25)
                        {
                            var char0idx = letters.IndexOf(seed.Substring(0, 1));
                            nextId = letters.Substring(++char0idx, 1) + "AA000001";
                        }
                        else
                        {
                            nextId = seed.Substring(0, 1) + letters.Substring(++char1idx, 1) + "A000001";
                        }
                    }
                    else
                    {
                        nextId = seed.Substring(0, 1) + letters.Substring(++char1idx, 1) + letters.Substring(++char2idx, 1) + "000001";
                    }

                }
            }
            return nextId;
        }
        public static string GetNext4digitMCPNID(string seed)
        {
            string nextId = string.Empty;
            if (seed == "9999")
            {
                Logging log = new Logging();
                log.CreateLogEntry("No more Medicaid ID left", Logging.LogPriority.Error);
                throw new ArgumentOutOfRangeException("I can handle till 9999 only!");
            }

            int medID = 0;
            if (Int32.TryParse(seed, out medID))
            {
                medID = medID + 1;
                nextId = medID.ToString().PadLeft(4, '0');
            }
            else
            {
                Logging log = new Logging();
                log.CreateLogEntry("Error while converting!", Logging.LogPriority.Error);
                throw new ArgumentOutOfRangeException("Error while converting!");
            }
            return nextId;
        }
    }
}
