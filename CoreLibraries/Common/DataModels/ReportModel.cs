using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Core.Libraries.DataModels
{
    public class ReportModel
    {
        public int ReportId { get; set; }

        public Guid Id { get; set; }

        public string ReportName { get; set; }

        public string Name { get; set; }

        public string ReportPath { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CategoryName { get; set; }

        public string ItemLocation { get; set; }

        public string CreatedByDisplayName { get; set; }
    }
}
