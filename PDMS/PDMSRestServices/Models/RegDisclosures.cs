using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDMSRestServices.Models
{
    public class DisclosureDto
    {


        public int REG_ID { get; set; }
        public int REG_DISCLOSURES_ID { get; set; }
        public int REG_QUESTION_ID { get; set; }
        /// <summary>
        /// Unique identifier for the disclosure record.
        /// </summary>
        public string QUESTION_TYPE_ID { get; set; }

        /// <summary>
        /// Incident date in "MM/dd/yyyy" format.
        /// </summary>
        public string INCIDENT_DATE { get; set; }

        /// <summary>
        /// State code where the incident occurred (e.g., OH, CA, TX).
        /// </summary>
        public string STATE_CODE { get; set; }

        public string COUNTY_ID { get; set; }

        public string COURT_ID { get; set; }

        public string COUNTRY_ID { get; set; }

        public string CHARGE { get; set; }

        public string AGREEMENT { get; set; }

        public string CASE_NUMBER { get; set; }
        
        /// <summary>
        /// Program affected by the incident.
        /// </summary>
        public string PROGRAM_AFFECTED { get; set; }

        /// <summary>
        /// Agency taking the action.
        /// </summary>
        public string AGENCY_TAKING_ACTION { get; set; }

        /// <summary>
        /// Action taken by the agency.
        /// </summary>
        public string ACTION_TAKEN { get; set; }

        /// <summary>
        /// Detailed explanation of the incident.
        /// </summary>
        public string EXPLANATION_DETAILS { get; set; }

        public string IS_SELECTED { get; set; }
    }
}