using MAXIMUS.Core.Libraries;
using System;
using System.Data;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class ProviderManagementData
    {
        public ProviderManagementData() { }

        public Guid UserID { get; set; }

        public int RegID { get; set; }

        public string ProviderName { get; set; }

        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public string LastName { get; set; }

        public string IndividualFullName
        {
            get
            {
                return string.Concat(FirstName, string.IsNullOrEmpty(MiddleInitial) ? string.Empty : " ", MiddleInitial, string.IsNullOrEmpty(LastName) ? string.Empty : " ", LastName);
            }
        }

        public string DBA { get; set; }

        public int? TypeOfPracticeID { get; set; }

        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }

        public string NPI { get; set; }
      
        public string TaxID { get; set; }

        public int TaxIDTypeID { get; set; }

        public int ProviderTypeID { get; set; }

        public string MMISProviderTypeID { get; set; }

        public string ProviderTypeName { get; set; }

        public int ProviderCategoryTypeID { get; set; }

        public string ProviderCategoryTypeName { get; set; }

        public string MedicaidID { get; set; }

        public string PracticeLocation { get; set; }

        public string PracticeLocationZip { get; set; }

        public int SpecialtyTypeID { get; set; }

        public string SpecialtyTypeName { get; set; }

        public int ReferralID { get; set; }

        public string ReferralNumber { get; set; }

        public DateTime? RequestedEffectiveDate { get; set; }

        public bool RetroEffectiveDate { get; set; }

        public DateTime? ChangeEffectiveDate { get; set; }

        public DateTime? SubmitDateTime { get; set; }

        public DateTime? RevalidationDate { get; set; }

        public int RevalidationDueWindow { get; set; }

        public DateTime? TerminationDate { get; set; }

        public Guid? ProcessOwnerID { get; set; }

        public int ProcessID { get; set; }

        public int CurrentStepID { get; set; }

        public int CurrentTaskID { get; set; }

        public string CurrentTaskName { get; set; }

        public string CurrentTaskClassName { get; set; }

        public string CurrentStepOwner { get; set; }

        public Guid? CurrentStepOwnerID { get; set; }

        public string RoleName { get; set; }

        //public BaseWorkflow Workflow { get; set; }

        public int WorkflowID { get; set; }

        public string WorkflowName { get; set; }

        public string FormCompletionPhone { get; set; }

        public string FormCompletionName { get; set; }

        public bool InProviderDataEntry { get; set; }

        public DateTime? ProcessStartDateTime { get; set; }

        public DateTime? ProcessEndDateTime { get; set; }

        public int RegistrationStatusTypeID { get; set; }
        
        public string RegistrationStatusType { get; set; }

        public int RegistrationProgramStatusTypeID { get; set; }

        public string RegProgramStatusTypeInternal { get; set; }
        
        public string RegProgramStatusTypeExternal { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public Guid? LastModifiedUser { get; set; }

        public bool IsOwnershipUpdate { get; set; }

        public int TaxonomyTypeID { get; set; }
       

        public string TaxonomyCode { get; set; }
       

        public string TaxonomyDetail { get; set; }
      
        public string ZipCode { get; set; }
      

        public string ZipExt { get; set; }
       

        public int WorkflowIDRequested { get; set; }

        public int PaperRequestQueueID { get; set; }

        public string EnrollmentStatusCode { get; set; }
        public string enrollmentStatusReason { get; set; }
        

        public string EnrollmentStatusCodeDescription { get; set; }

        public string Comments { get; set; }

        public DateTime? MoratoriaBeginDate { get; set; }

        public DateTime? MoratoriaEndDate { get; set; }

        public int ReferralTypeID;
        private bool nursingLicense;

        public bool GetNursingLicense()
        {
            return nursingLicense;
        }

        public void SetNursingLicense(bool value)
        {
            nursingLicense = value;
        }

        public DateTime? NPIStartDate { get; set; }

        public DateTime? NPIEndDate { get; set; }
        
        public DateTime? EndDate { get; set; }

        public string PracticeLocationFull
        {
            get { return string.Format("{0} {1}", PracticeLocation, PracticeLocationZip); }
        }

        public bool IsPaperApplication { get; set; }

        public bool AdminKeyFieldEditRequest { get; set; }
        public bool KeyFieldEditRequest { get; set; }
        public int WorkflowEventType { get; set; }

        public DateTime? RegCreateDateTime { get; set; }

        public bool IsRevalidation { get; set; }
        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeName { get; set; }

        public int RegEventInfoTypeID { get; set; }

        public int IsInUpdateProcess { get; set; }

        public bool ChangedApplicationTypeID { get; set; }

        public bool IsCredentialingProcess { get; set; }
        public bool IsCredentialingProvider { get; set; }

        public DateTime? PreviousChangeEffectiveDate { get; set; }

        public DateTime? PreviousEndDate { get; set; }

        public bool AllowReactivate { get; set; }

        public bool AllowManage { get; set; }

        public bool IsProviderReactivation { get; set; }
        public string ExitingMedicaId { get; set; }


        public bool CheckNPIRequired { get; set; }
        // public string OrganizationName { get; set; }
        public int WaiverTypeID { get; set; }
        public string WaiverTypeName { get; set; }

        public string DDContractNumber { get; set; }
        public string DDFacilityNumber { get; set; }

        public DateTime DODDStartDate { get; set; }
        public DateTime DODDEndDate { get; set; }
        public bool IsProviderReapplication { get; set; }

        public int WaiverServiceUpdateTypeID { get; set; }

        public string TERMCOMMENTS { get; set; }

        public string MMISCPCPracticeTypeID { get; set; }
        public bool IsAddODMorODAMedicaid { get; set; }
        public bool HasODMSpecialty { get; set; }
        public bool HasActiveODMSpecialty { get; set; }
        public bool HasDODDSpecialty { get; set; }
        public bool HasActiveDODDSpecialty { get; set; }
        public bool HasODASpecialty { get; set; }
        public bool HasActiveODASpecialty { get; set; }

        #region Public Methods

        public void LoadObjectFromDataset(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
                return;

            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                return;

            DataRow dr = dt.Rows[0];
            RegID = Methods.GetIntValue(dr["RegID"]);

            //The helper functions used for GetStringValue and GetIntValue here will check to make sure the column exists in the datarow before grabbing it.
            //will default to empty string for string and 0 for int.
            ProviderName = Methods.GetStringValue(dr, "ProviderName");
            NPI = Methods.GetStringValue(dr, "NPI");
            TaxID = Methods.GetStringValue(dr, "TaxID");
            PracticeLocation = Methods.GetStringValue(dr, "PracticeLocation");
            ZipCode = Methods.GetStringValue(dr, "ZipCode");
            ZipExt = Methods.GetStringValue(dr, "ZipExt");
            PracticeLocationZip = Methods.GetStringValue(dr, "PracticeLocationZip");
            TaxonomyCode = Methods.GetStringValue(dr, "TaxonomyCode");
            TaxonomyDetail = Methods.GetStringValue(dr, "TaxonomyDetail");
            FirstName = Methods.GetStringValue(dr, "FirstName");
            MiddleInitial = Methods.GetStringValue(dr, "MiddleInitial");
            LastName = Methods.GetStringValue(dr, "LastName");
            Gender = Methods.GetStringValue(dr, "Gender");
            MedicaidID = Methods.GetStringValue(dr, "MedicaidID");
            ProviderTypeName = Methods.GetStringValue(dr, "ProviderTypeName");
            SpecialtyTypeName = Methods.GetStringValue(dr, "SpecialtyTypeName");
            ProviderCategoryTypeName = Methods.GetStringValue(dr, "ProviderCategoryTypeName");
            DBA = Methods.GetStringValue(dr, "DBA");
            CurrentTaskName = Methods.GetStringValue(dr, "CurrentTaskName");
            CurrentTaskClassName = Methods.GetStringValue(dr,"CurrentTaskClassName");
            CurrentStepOwner = Methods.GetStringValue(dr,"CurrentStepOwner");
            RoleName = Methods.GetStringValue(dr,"RoleName");
            ApplicationTypeName = Methods.GetStringValue(dr, "ApplicationTypeName");
            WaiverTypeID = Methods.GetIntValue(dr, "WaiverTypeID");
            WaiverTypeName = Methods.GetStringValue(dr, "WaiverTypeName");

            WorkflowName = Methods.GetStringValue(dr,"WorkflowName");
            ReferralNumber = Methods.GetStringValue(dr, "ReferralNumber");
            FormCompletionName = Methods.GetStringValue(dr,"FormCompletionName");
            FormCompletionPhone = Methods.GetStringValue(dr,"FormCompletionPhone");
            RegistrationStatusType = Methods.GetStringValue(dr, "RegistrationStatusType");
            RegProgramStatusTypeInternal = Methods.GetStringValue(dr, "RegProgramStatusTypeInternal");
            RegProgramStatusTypeExternal = Methods.GetStringValue(dr, "RegProgramStatusTypeExternal");
            EnrollmentStatusCodeDescription = Methods.GetStringValue(dr, "EnrollmentStatusCodeDescription");
            enrollmentStatusReason = Methods.GetStringValue(dr, "EnrollmentStatusReasonCode"); 
            EnrollmentStatusCode = Methods.GetStringValue(dr, "EnrollmentStatusCode");

            //INT
            ProviderTypeID = Methods.GetIntValue(dr, "ProviderTypeID");
            MMISProviderTypeID = Methods.GetStringValue(dr, "MMISProviderTypeID");
            ProviderCategoryTypeID = Methods.GetIntValue(dr, "ProviderCategoryTypeID");
            SpecialtyTypeID = Methods.GetIntValue(dr, "SpecialtyTypeID");
            TaxonomyTypeID = Methods.GetIntValue(dr, "TaxonomyTypeID");
            TypeOfPracticeID = Methods.GetIntValue(dr, "TypeOfPracticeID");
            TaxIDTypeID = Methods.GetIntValue(dr, "TaxIDTypeID");
            ReferralID = Methods.GetIntValue(dr, "ReferralID");
            ReferralTypeID = Methods.GetIntValue(dr, "DIDD_REFERRAL_TYPE_ID");
            PaperRequestQueueID = Methods.GetIntValue(dr, "PaperRequestQueueID");
            if (Methods.ColumnExists("CurrentTaskID", dr))
            {
                CurrentTaskID = Methods.GetIntValue(dr["CurrentTaskID"]);
            }
            else
            {
                CurrentTaskID = -1;
            }
            if (Methods.ColumnExists("CurrentStepID", dr))
            {
                CurrentStepID = Methods.GetIntValue(dr["CurrentStepID"]);
            }
            else
            {
                CurrentStepID = -1;
            }
            ProcessID = Methods.GetIntValue(dr, "ProcessID");
            WorkflowID = Methods.GetIntValue(dr, "WorkflowID");
            RegistrationStatusTypeID = Methods.GetIntValue(dr, "RegistrationStatusTypeID");
            RegistrationProgramStatusTypeID = Methods.GetIntValue(dr, "RegProgramStatusTypeID");
            ApplicationTypeID = Methods.GetIntValue(dr, "ApplicationTypeID");
            WorkflowEventType = Methods.GetIntValue(dr, "WORKFLOW_EVENT_TYPE_ID");
            DDContractNumber = Methods.GetStringValue(dr, "DD_Contract_number");
            DODDStartDate = Methods.GetDateValue(dr, "dodd_start_Date");
            DODDEndDate = Methods.GetDateValue(dr, "dodd_end_date");
            TERMCOMMENTS = Methods.GetStringValue(dr, "TERM_COMMENTS");
            if (Methods.ColumnExists("CHANGED_APPLICATION_TYPE", dr))
            {
                if (dr["CHANGED_APPLICATION_TYPE"] != DBNull.Value)
                {
                    ChangedApplicationTypeID = Methods.GetBoolean(dr["CHANGED_APPLICATION_TYPE"]);
                }
            }

            //BOOLEANS
            if (Methods.ColumnExists("InProviderDataEntry", dr))
            {
                InProviderDataEntry = Methods.GetBoolean(dr["InProviderDataEntry"]);
            }
            
            //GUIDS
            if (Methods.ColumnExists("ProcessOwnerID", dr))
            {
                if (dr["ProcessOwnerID"] != DBNull.Value)
                {
                    ProcessOwnerID = new Guid(Methods.GetStringValue(dr["ProcessOwnerID"]));
                }
            }
            if (Methods.ColumnExists("CurrentStepOwnerID", dr))
            {
                if (dr["CurrentStepOwnerID"] != DBNull.Value)
                {
                    CurrentStepOwnerID = new Guid(Methods.GetStringValue(dr["CurrentStepOwnerID"]));
                }
            }
            if (Methods.ColumnExists("UserID", dr))
            {
                if (dr["UserID"] != DBNull.Value)
                {
                    UserID = new Guid(Methods.GetStringValue(dr["UserID"]));
                }
            }

            //DATES
            if (Methods.ColumnExists("BirthDate", dr))
            {
                if (dr["BirthDate"] != DBNull.Value)
                {
                    BirthDate = Methods.GetDateValue(dr["BirthDate"]);
                }
            }
            if (Methods.ColumnExists("LastModifiedDateTime", dr))
            {
                if (dr["LastModifiedDateTime"] != DBNull.Value)
                {
                    LastModifiedDate = Methods.GetDateValue(dr["LastModifiedDateTime"]);
                }
            }
            if (Methods.ColumnExists("RequestedEffectiveDate", dr))
            {
                if (dr["RequestedEffectiveDate"] != DBNull.Value)
                {
                    RequestedEffectiveDate = Methods.GetDateValue(dr["RequestedEffectiveDate"]);
                }
            }
            if (Methods.ColumnExists("ChangeEffectiveDate", dr))
            {
                if (dr["ChangeEffectiveDate"] != DBNull.Value)
                {
                    ChangeEffectiveDate = Methods.GetDateValue(dr["ChangeEffectiveDate"]);
                }
            }
            if (Methods.ColumnExists("SubmitDateTime", dr))
            {
                if (dr["SubmitDateTime"] != DBNull.Value)
                {
                    SubmitDateTime = Methods.GetDateValue(dr["SubmitDateTime"]);
                }
            }
            if (Methods.ColumnExists("RevalidationDate", dr))
            {
                if (dr["RevalidationDate"] != DBNull.Value)
                {
                    RevalidationDate = Methods.GetDateValue(dr["RevalidationDate"]);
                }
            }
            if (Methods.ColumnExists("TerminationDate", dr))
            {
                if (dr["TerminationDate"] != DBNull.Value)
                {
                    TerminationDate = Methods.GetDateValue(dr["TerminationDate"]);
                }
            }
            if (Methods.ColumnExists("MoratoriaBeginDate", dr))
            {
                if (dr["MoratoriaBeginDate"] != DBNull.Value)
                {
                    MoratoriaBeginDate = Methods.GetDateValue(dr["MoratoriaBeginDate"]);
                }
            }
            if (Methods.ColumnExists("MoratoriaEndDate", dr))
            {
                if (dr["MoratoriaEndDate"] != DBNull.Value)
                {
                    MoratoriaEndDate = Methods.GetDateValue(dr["MoratoriaEndDate"]);
                }
            }
            if (Methods.ColumnExists("NPIStartDate", dr))
            {
                if (dr["NPIStartDate"] != DBNull.Value)
                {
                    NPIStartDate = Methods.GetDateValue(dr["NPIStartDate"]);
                }
            }
            if (Methods.ColumnExists("NPIEndDate", dr))
            {
                if (dr["NPIEndDate"] != DBNull.Value)
                {
                    NPIEndDate = Methods.GetDateValue(dr["NPIEndDate"]);
                }
            }
            if (Methods.ColumnExists("EndDate", dr))
            {
                if (dr["EndDate"] != DBNull.Value)
                {
                    EndDate = Methods.GetDateValue(dr["EndDate"]);
                }
            }
            if (Methods.ColumnExists("PreviousEndDate", dr))
            {
                if (dr["PreviousEndDate"] != DBNull.Value)
                {
                    PreviousEndDate = Methods.GetDateValue(dr["PreviousEndDate"]);
                }
            }

            if (Methods.ColumnExists("IsInUpdateProcess", dr))
            {
                if (dr["IsInUpdateProcess"] != DBNull.Value)
                {
                    IsInUpdateProcess = Methods.GetIntValue(dr["IsInUpdateProcess"]);
                }
            }

            if (Methods.ColumnExists("AllowReactivate", dr))
            {
                if (dr["AllowReactivate"] != DBNull.Value)
                {
                    AllowReactivate = Methods.GetBoolean(dr["AllowReactivate"]);
                }
            }

            if (Methods.ColumnExists("AllowManage", dr))
            {
                if (dr["AllowManage"] != DBNull.Value)
                {
                    AllowManage = Methods.GetBoolean(dr["AllowManage"]);
                }
            }

            if (Methods.ColumnExists("IsProviderReactivation", dr))
            {
                if (dr["IsProviderReactivation"] != DBNull.Value)
                {
                    IsProviderReactivation = Methods.GetBoolean(dr["IsProviderReactivation"]);
                }
            }
            IsCredentialingProvider = Methods.GetStringValue(dr, "IsCredentialingProvider")=="1"?true:false;
            if (Methods.ColumnExists("IsReapplication", dr))
            {
                if (dr["IsReapplication"] != DBNull.Value)
                {
                    IsProviderReapplication = Methods.GetBoolean(dr["IsReapplication"]);
                }
            }
            if (Methods.ColumnExists("WaiverServiceUpdateTypeID", dr))
            {
                if (dr["WaiverServiceUpdateTypeID"] != DBNull.Value)
                {
                    WaiverServiceUpdateTypeID = Methods.GetIntValue(dr["WaiverServiceUpdateTypeID"]);
                }
            }
            if (Methods.ColumnExists("MMISCPCPracticeTypeID", dr))
            {
                if (dr["MMISCPCPracticeTypeID"] != DBNull.Value)
                {
                    MMISCPCPracticeTypeID = Methods.GetStringValue(dr["MMISCPCPracticeTypeID"]);
                }
            }
            if (Methods.ColumnExists("IsAddODMorODAMedSvc", dr))
            {
                if (dr["IsAddODMorODAMedSvc"] != DBNull.Value)
                {
                    IsAddODMorODAMedicaid = Methods.GetBoolean(dr["IsAddODMorODAMedSvc"]);
                }
            }
            if (Methods.ColumnExists("HasODMSpecialty", dr))
            {
                if (dr["HasODMSpecialty"] != DBNull.Value)
                {
                    HasODMSpecialty = Methods.GetBoolean(dr["HasODMSpecialty"]);
                }
            }
            if (Methods.ColumnExists("HasActiveODMSpecialty", dr))
            {
                if (dr["HasActiveODMSpecialty"] != DBNull.Value)
                {
                    HasActiveODMSpecialty = Methods.GetBoolean(dr["HasActiveODMSpecialty"]);
                }
            }
            if (Methods.ColumnExists("HasDODDSpecialty", dr))
            {
                if (dr["HasDODDSpecialty"] != DBNull.Value)
                {
                    HasDODDSpecialty = Methods.GetBoolean(dr["HasDODDSpecialty"]);
                }
            }
            if (Methods.ColumnExists("HasActiveDODDSpecialty", dr))
            {
                if (dr["HasActiveDODDSpecialty"] != DBNull.Value)
                {
                    HasActiveDODDSpecialty = Methods.GetBoolean(dr["HasActiveDODDSpecialty"]);
                }
            }
            if (Methods.ColumnExists("HasODASpecialty", dr))
            {
                if (dr["HasODASpecialty"] != DBNull.Value)
                {
                    HasODASpecialty = Methods.GetBoolean(dr["HasODASpecialty"]);
                }
            }
            if (Methods.ColumnExists("HasActiveODASpecialty", dr))
            {
                if (dr["HasActiveODASpecialty"] != DBNull.Value)
                {
                    HasActiveODASpecialty = Methods.GetBoolean(dr["HasActiveODASpecialty"]);
                }
            }
            if (Methods.ColumnExists("TAXONOMY_CODE", dr))
            {
                if (dr["TAXONOMY_CODE"] != DBNull.Value)
                {
                    TaxonomyCode = Methods.GetString("TAXONOMY_CODE",dr);
                }
            }
        }

        #endregion


    }
}
