using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Service.Reporting
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Permission
    {
        public string PermissionAccess { get; set; }
        public string PermissionEntity { get; set; }
        public int UserId { get; set; }
        public string ItemName { get; set; }
        public int PermissionId { get; set; }
        public string ItemId { get; set; }
    }

    public enum AccessLevel
    {
        Read,
        Write,
        ReadWrite,
        ReadWriteDelete,
        Create,
        Delete,
        Download
    }

    public enum PermissionEntity
    {
        [Description("All Categories")]
        AllCategories,
        [Description("All Dashboards")]
        AllDashboards,
        [Description("All Data Sources")]
        AllDataSources,
        [Description("All Datasets")]
        AllDatasets,
        [Description("All Files")]
        AllFiles,
        [Description("All Groups")]
        AllGroups,
        [Description("All ItemViews")]
        AllItemViews,
        [Description("All Permissions")]
        AllPermissions,
        [Description("All Reports")]
        AllReports,
        [Description("All Schedules")]
        AllSchedules,
        [Description("All Settings")]
        AllSettings,
        [Description("All Slideshow")]
        AllSlideshow,
        [Description("All Widgets")]
        AllWidgets,
        [Description("Dashboards in Category")]
        DashboardsInCategory,
        [Description("Reports in Category")]
        ReportsInCategory,
        [Description("Specific Category")]
        SpecificCategory,
        [Description("Specific Dashboard")]
        SpecificDashboard,
        [Description("Specific Data Source")]
        SpecificDataSource,
        [Description("Specific Dataset")]
        SpecificDataset,
        [Description("Specific File")]
        SpecificFile,
        [Description("Specific Group")]
        SpecificGroup,
        [Description("Specific ItemView")]
        SpecificItemView,
        [Description("Specific Permissions")]
        SpecificPermissions,
        [Description("Specific Report")]
        SpecificReport,
        [Description("Specific Schedule")]
        SpecificSchedule,
        [Description("Specific Settings")]
        SpecificSettings,
        [Description("Specific Slideshow")]
        SpecificSlideshow,
        [Description("Specific Widget")]
        SpecificWidget,
        [Description("Users and Groups")]
        UsersAndGroups
    }
}
