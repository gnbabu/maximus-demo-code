using System;

namespace MAXIMUS.Services.PDMS.Contracts
{
    public class Request
    {
        public string RequestingApplication { get; set; }

        public DateTime RequestDateTime { get; set; }
    }
}