using System;

namespace Corp.Core.Libraries.Helper
{
    public static class ProviderFeedHelper
    {
        public static int InsertProviderFeedNotes(int regId, int regProviderFeedId, string initiatedBy, string note,
            string personReviewedBy = null, string enrollmentType = null, string finalDisposition = null, int processID = 0)
        {
            try
            {
                using (var client = new PDMSService.PDMSServiceClient())
                {
                    return client.InsertProviderFeedNotes(
                        regId,
                        regProviderFeedId,
                        initiatedBy,
                        note,
                        personReviewedBy,
                        enrollmentType,
                        finalDisposition,
                        processID);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error inserting provider feed notes: {ex.Message}");
                throw;
            }
        }
    }
}