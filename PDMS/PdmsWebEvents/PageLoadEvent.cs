using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Management;

namespace PdmsWebEvents
{
    public class PageLoadEvent : WebAuditEvent
    {
        private Dictionary<string, string> AdditionalLogDetailsDictionary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items">Dictionary with string key and string value</param>
        /// <param name="message">string </param>
        /// <param name="eventSource">object</param>
        public PageLoadEvent(Dictionary<string, string> items, string message, object eventSource) : base(message, eventSource, 100004)
        {
            AdditionalLogDetailsDictionary = items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="message"></param>
        /// <param name="eventSource"></param>
        /// <param name="eventCode"></param>
        public PageLoadEvent(Dictionary<string, string> items, string message, object eventSource, int eventCode) : base(message, eventSource, 100004)
        {
            AdditionalLogDetailsDictionary = items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="message"></param>
        /// <param name="eventSource"></param>
        /// <param name="eventCode"></param>
        /// <param name="eventDetailCode"></param>
        public PageLoadEvent(Dictionary<string, string> items, string message, object eventSource, int eventCode, int eventDetailCode) : base(message, eventSource, 100004, 0)
        {
            AdditionalLogDetailsDictionary = items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatter"></param>
        public override void FormatCustomEventDetails(WebEventFormatter formatter)
        {
            base.FormatCustomEventDetails(formatter);
            formatter.AppendLine("[Activity Description]: Page Load");

            string additionalDetails = GetMessageFromDictionary(AdditionalLogDetailsDictionary);

            if (additionalDetails != null && additionalDetails.Length > 0)
                formatter.AppendLine(additionalDetails);
        }

        private static string GetMessageFromDictionary(Dictionary<string, string> items)
        {
            if (items != null && items.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (string key in items.Keys)
                {
                    sb.AppendLine((key != null ? "[" + key + "]" : "[NULL]") + ": " + (items[key] != null ? items[key] : ""));
                }

                if (sb.Length > 0)
                    sb.Remove(sb.ToString().LastIndexOf(Environment.NewLine), Environment.NewLine.Length);

                return sb.Length > 0 ? sb.ToString() : "";
            }

            return "";
        }

        /// <summary>
        /// Static method for raising this event.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="message"></param>
        /// <param name="eventSource"></param>
        public static void Raise(Dictionary<string, string> items, string message, object eventSource)
        {
            new PageLoadEvent(items, message, eventSource).Raise();
        }
    }
}
