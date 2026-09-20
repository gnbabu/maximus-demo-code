namespace ReportingService.Services.Security
{
    /// <summary>
    /// Standard permission provider
    /// </summary>
    public partial class StandardCapabilityProvider
    {
        #region UserManagement
        public static class UserManagement
        {
            public const string UpdateUser = "UpdateUser";
        }
        #endregion
        #region SMA
        public static class SMA
        {
            public const string Add = "RTPAdd"; 
            public const string Delete = "RTPDelete";
            public const string RTPManagement = "RTPManagement";
            public const string ReturnToView = "ReturnToView";
            public const string ViewNotes = "ViewNotes";

            public const string IndicatorsEdit = "IndicatorsEdit";
            public const string ScreeningEdit = "ScreeningEdit";

            public const string ApplicationSummaryEdit = "ApplicationSummaryEdit";
            public const string ProviderIdentifiersEdit = "ProviderIdentifiersEdit";
            public const string ManageAffiliationsEdit = "ManageAffiliationsEdit";
            public const string ManageServicesEdit = "ManageServicesEdit";
            public const string ManageRenderingServicingProvidersEdit = "ManageRenderingServicingProvidersEdit";
            public const string FingureprintBackgroundCheckEdit = "FingureprintBackgroundCheckEdit";
            public const string ProviderCorrespondence = "ProviderCorrespondence";
            public const string ManageAppealsEdit = "ManageAppealsEdit";
        }
        
        public static class Home
        {
            public const string Member = "Home.Member"; 
            public const string Claims = "Claims";
            public const string Authorization = "Authorization";
            public const string Enrollment = "Enrollment";
            public const string SiteVisit = "SiteVisit";     
            public const string Correspondence = "Correspondence";
            public const string Analytics = "Analytics"; 
            public const string AccountAdministration = "AccountAdministration";
            public const string DIDDReferrals = "DIDDReferrals";   
            public const string Appeals = "Appeals";
        }

        public static class Member
        {
            public const string MemberBenefits = "Member.MemberBenefits";
            public const string ClaimBasedMedicalHistory = "Member.ClaimBasedMedicalHistory";

        }

        public static class Claims
        {
            public const string ClaimManagement = "Claims.ClaimManagement";
            public const string ClaimAppeal = "Claims.ClaimAppeal";
            public const string ClaimAppealWorkflow = "Claims.ClaimAppealWorkflow";
            public const string RemittanceAdvice = "Claims.ClaimPayments.RemittanceAdvice";
            public const string Download835Search = "Claims.ClaimPayments.835";
        }

        public static class Authorization
        {
            public const string PriorAuthorization = "PriorAuthorization";
        }

        public static class Enrollment
        {
            public const string EnrollmentManagement = "Enrollment.EnrollmentManagement";
            public const string ReferenceData = "Enrollment.ReferenceData";
            public const string WorkflowManagement = "Home.Enrollment.Workflow Management";
            public const string BankingInformationEdit = "Enrollment.BankingInformationEdit";
            public const string FederalTaxInformationEdit = "Enrollment.FederalTaxInformationEdit";
            public const string ApplicationFeePaymentEdit = "Enrollment.ApplicationFeePaymentEdit";
            public const string DemographicsManagementEdit = "Enrollment.DemographicsManagementEdit";
            public const string BusinessEligibilityEdit = "Enrollment.BusinessEligibilityEdit";
            public const string RevalidationEdit = "Enrollment.RevalidationEdit";
            public const string ProviderMaintenanceEdit = "Enrollment.ProviderMaintenanceEdit";

            
        }
         public static class EnrollmentManagement
        {
            public const string EnrollmentWorkbench = "Enrollment.EnrollmentManagement.EnrollmentWorkbench";
            public const string EnrollmentManagementWorkbench = "Enrollment.EnrollmentManagement.EnrollmentManagementWorkbench"; 
            public const string EnrollmentMaintainanceWorkbench = "Enrollment.EnrollmentManagement.EnrollmentMaintainanceWorkbench";
            public const string ProviderMaintainanceWorkbench = "Enrollment.EnrollmentManagement.ProviderMaintainanceWorkbench";
            public const string AddProvider = "Enrollment.EnrollmentManagement.EnrollmentWorkbench.AddProvider";
        }
        public static class ReferenceData
        {
            public const string ProviderTypeSpecialityWorkbench = "Enrollment.ReferenceData.ProviderTypeSpecialityWorkbench";
            public const string MoratoriaMaintenance = "Enrollment.MoratoriaMaintenance";
            public const string AssociationsMaintenance= "Enrollment.AssociationsMaintenance";
            public const string EnrollmentWorkflow = "Enrollment.ReferenceData.EnrollmentWorkflow";
        }
        public static class WorkflowManagement
        {
            public const string EnrollmentManagementWorkflow = "Enrollment.WorkflowManagement.EnrollmentManagementWorkflow";
            public const string EnrollmentAppealWorkflow = "Enrollment.WorkflowManagement.EnrollmentAppealWorkflow";
        }

        public static class SiteVisit
        {
            public const string SiteVisitManagementWorkbench = "SiteVisit.SiteVisitManagementWorkbench";
            public const string SiteVisitQueue = "SiteVisit.SiteVisitQueue";
        }
       

        public static class Analytics
        {
            public const string ReportManagement = "Analytics.ReportManagement";
            public const string CustomReports = "Analytics.CustomReports";
            public const string ExceptionManagement = "Analytics.ExceptionManagement";

        }
        public static class AccountAdministration
        {
            public const string ProviderUserAdministration = "AccountAdministration.ProviderUserAdministration";
            public const string GlobalUserAdministration = "AccountAdministration.GlobalUserAdministration";
            public const string BillingProviderRelationship = "AccountAdministration.BillingProviderRelationship";
            public const string Alertmanagement = "AccountAdministration.Alertmanagement";
            public const string ConfigurationConsole = "AccountAdministration.ConfigurationConsole";
            public const string DocumentManager = "AccountAdministration.DocumentManager";
            public const string About360 = "AccountAdministration.About360";
            public const string ThemeConfiguration = "AccountAdministration.ThemeConfiguration";

            public const string DocumentManagerEdit = "AccountAdministration.DocumentManagerEdit";
        }

        public static class DIDDReferrals
        {
            public const string CreateDIDDReferral = "DIDDReferrals.CreateDIDDReferral";
            public const string SearchDIDDProviders = "DIDDReferrals.SearchDIDDProviders";
        }
        public static class Appeals
        {
            public const string ClaimsAppeals = "Appeals.ClaimsAppeals";
            public const string EnrollmentAppeals = "Appeals.EnrollmentAppeals";
            public const string PriorAuthorizationAppeals = "Appeals.PriorAuthorizationAppeals";

        }
        public static class CRM
        {
            public const string CRMDashboard = "CRM";
        }
        public static class Correspondence
        {
            public const string CorrespondenceWorkbench = "Correspondence";
        }

        public static class Reporting
        {
            public const string Base = "Reporting";
            public const string Edit = "Reporting.Edit";
            public const string Copy = "Reporting.Copy";
            public const string Download = "Reporting.Download";
            public const string AdHoc = "Reporting.AdHoc";
        }

        #endregion
        #region [System]
        public const string AccessProfiling = "System.AccessProfiling"; 
        #endregion
    }
}