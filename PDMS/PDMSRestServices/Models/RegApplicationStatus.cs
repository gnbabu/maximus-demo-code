using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDMSRestServices.Models
{
    public class RegApplicationStatus
    {
        public int RegId { get; set; }
        public int ProcessId { get; set; }
        public string ApplicationStatus { get; set; }
    }
}