using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Service.Reporting
{
    public enum ItemType
    {
        [EnumMember]
        [Description("Category")]
        Category = 1,
        [EnumMember]
        [Description("Dashboard")]
        Dashboard,
        [EnumMember]
        [Description("Report")]
        Report,
        [EnumMember]
        [Description("Datasource")]
        Datasource,
        [EnumMember]
        [Description("Dataset")]
        Dataset,
        [EnumMember]
        [Description("File")]
        File,
        [EnumMember]
        [Description("Schedule")]
        Schedule
    }
}
