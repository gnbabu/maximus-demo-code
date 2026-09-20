using System;

namespace ReportingService.Services.Data.Attributes
{
    /// <summary>
    /// Attribute to set the TimeZone as the TimeZoneInfo.Id
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TimeZoneAttribute : Attribute
    {
        /// <summary>
        /// Constuctor to set the TimeZone as the TimeZoneInfo.Id
        /// </summary>
        /// <param name="timeZoneInfo">TimeZoneInfo</param>
        public TimeZoneAttribute(TimeZoneInfo timeZone)
        {
            Id = timeZone.Id;
        }

        /// <summary>
        /// Sets the TimeZone from a valid TimeZoneInfo.Id or from a ConfigurationManager Key
        /// </summary>
        /// <param name="timeZone">TimeZoneInfo.Id or ConfigurationManager key of Valid TimeZoneInfo.Id value</param>
        public TimeZoneAttribute(string timeZone)
        {
            Id = timeZone;
        }

        public string Id { get; protected set; }
    }
}
