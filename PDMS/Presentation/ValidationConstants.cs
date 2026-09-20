namespace MAXIMUS.Presentation.PDMS
{
    public class ValidationConstants
    {
        public static class DCSProviderSearch
        {
            public const string AtLeastOneCriterionRequired = "At least 1 search criterion is required.";
            public const string MoreThanFirstNameRequired = "At least 1 search criterion other than First Name is required.";
        }

        public static class PartyEligibility
        {
            public const string GroupRequired = "A DCS Group is required.";
            public const string StartDateRequired = "Start Date is Required.";
            public const string StartDateCannotBeFutureDate = "Start Date cannot be a future date.";
            public const string StartDateMustBePriorToEndDate = "Start Date cannot be piror to End Date.";
        }

        public static class AchFeeInformation
        {
            public const string DepositIDRequired = "Deposit ID is required.";  //Was called Edison Id
            public const string PaymentDateRequired = "Deposit Date is required.";
            public const string PartyIDRequired = "Party ID was not provided.  Required for insert.";
        }

        public static class AchFeeSearch
        {
            public const string AtLeastOneSearchCriteriaRequired = "At least 1 search criterion is required.";
            public const string PaidDateFromNotFuture = "The Paid Date From cannot be a future date.";
            public const string PaidDateToNotFuture = "The Paid Date To cannot be a future date.";
            public const string DueDateRangeError = "The Due Date From and To range cannot span more than 60 days.";
            public const string PaidDateToPriorToDateFrom = "The Paid Date To value cannot be prior to the Paid Date From value.";
            public const string DueDateToPriorToDateFrom = "The Due Date To value cannot be prior to the Due Date From value.";
            public const string DueDateFromRequired = "The Due Date From is required when a Due Date To date is provided.";
            public const string DueDateToRequired = "The Due Date To is required when a Due Date From date is provided.";
            public const string PaidDateRangeError = "The Paid Date From and To range cannot span more than 12 months.";
            public const string PaidDateToOnlyError = "At least one other criterion must be set when only Paid Date To is set.";
            public const string PaidDateFromOnlyError = "At least one other criterion must be set when only Paid Date From is set.";
        }

        public static class ProviderManagerData
        {
            public const string SSNIsInValid = "Tax ID entered already exists as an EIN. Verify this is a valid SSN.";
            public const string ExitingMedicaidIDInvalid = "Invalid exiting medicaid id.";
            public const string ExitingMedicaidIDProviderTypeNotMatch = "Invalid provider type for exiting medicaid id.";
            public const string UserIDRequired = "User ID is required.";
            public const string TaxIDRequired = "Tax ID is required.";
            public const string RegIDRequired = "Reg ID is required.";
            public const string PartyIDRequired = "Party ID is required.";
            public const string ChangeEffectiveDateMustBeDate = "Change Effective Date must be date.";
            public const string ChangeEffectiveDateRequired = "Change Effective Date is required.";
            public const string ChangeEffectiveDateMustBePrior = "Change Effective Date must be prior to existing effective date.";
            public const string ChangeEffectiveDateMustBeAfter = "Change Effective Date must not be prior to the enrollment start date.";
            public const string ChangeEffectiveDateOneYearBack = "Change Effective Date must not be prior to one year ago from today.";
            public const string ChangeEffectiveSameDate = "New Effective Date can not be the same date as the Current Effective Date.";
            public const string RequestedEffectiveDateRequired = "Requested Effective Date is required.";
            public const string RequestedEffectiveDateMustBeDate = "Requested Effective Date must be date.";
            public const string RequestedEffectiveDateNotPastDate = "Requested Effective Date cannot be a past date.";
            public const string RouteToUrlNotSpecified = "Route to url is required.  Rout to url is not setup.";
            public const string CopyToVersionTablesFailed = "CopyToVersionTables returned 0 rows.";
            public const string CreationOfNewWorkflowFailed = "Creation of the new workflow process failed.";
            public const string RevalidationAppSettingsRequired = "Revalidation appsettings are not defined.  Contact Tech Support.";
            public const string TerminationDateRequired = "Termination Date is required.";
            public const string TerminationReasonRequired = "Termination Reason is required.";
            public const string DisenrollmentEffectiveDateRequired = "Disenrollment Effective Date is required.";
            public const string RetroEffectivePart1Error = "Requested Effective Date must be within";
            public const string RetroEffectivePart2Error = "days of current date.";
            public const string RevalidationDateRequired = "Re-Enrollment Date is required.";

            public const string TaxonomyRequired = "Taxonomy is required.";
            public const string ProviderCategoryRequired = "Provider Category is required.";
            public const string ProviderTypeRequired = "Provider Type is required.";
            public const string SpecialtyRequired = "Specialty is required.";
            public const string NPIRequired = "NPI is required.";
            public const string DDFacilityNumberRequired = "DD Facility Number is required.";
            public const string DDFacilityNumberNotExists = "The Facility Number entered is not associated to a valid facility.";
            public const string NPIMustBeType1 = "The NPI entered must be a Type 1 NPI.";
            public const string NPIMustBeType2 = "The NPI entered must be a Type 2 NPI.";
            public const string PracticeLocation = "Practice Location name is required.";
            public const string ZipCodeRequired = "Zip Code is required.";
            public const string ZipCodeExtRequired = "Zip Code Ext is required.";
            public const string FoundUnusedConvertedProvider = "A matching pending converted provider was found. Please close this pop up, then select the converted provider from your Converted Not Managed Providers list to proceed.";
            public const string ZipCodeNotSupport = "Ohio Medicaid is currently not accepting new applications for In-State Psychiatric Residential Treatment Facilities";
            public const string NursingLicenseErrorMessage = "In order to provide these services, you must be an Active Private Duty Nurse. Please cancel this registration and complete a Standard Application to registers as a Private Duty Nurse. You can then apply to provide these services.";

            public const string MedicaidIDRequired = "Provider Number is required.";

            public const string CommentRequired = "Comments are required.";
            public const string NewEnrollmentStatusRequired = "New enrollment status is required.";

            //Individual Provider Only validations
            public const string PracticeTypeRequired = "A value for Practice Type is required.";
            public const string DBARequired = "A value for DBA (doing business as) is required.";
            public const string FirstNameRequired = "A value for First Name is required.";
            public const string LastNameRequired = "A value for Last Name is required.";
            public const string TaxIDTypeRequired = "A selection for Tax ID Type is required.";
            public const string GenderRequired = "A value for Gender is required.";
            public const string BirthDateRequired = "A value for Birth Date is required.";
            public const string ReqEffectiveDateRequired = "A value for Requested Effective Date is required.";
            public const string BirthDateRange = "Birth Date can not be a future date and cannot result in an age over 100 years.";

            //Group Provider Only validations
            public const string OrganizationNameRequired = "A value for Business Name is required.";

            public const string ReferralNumberRequired = "A value for Referral Number is required.";
            public const string ApplicationNumberAlreadyUsed = "The application referral number entered has already been used.";
            public const string ApplicationNumberNotFound = "The application referral number entered was not found.";
            public const string ReferralIDRequired = "The application referral record for this ID was not found.";

            public const string ExistingProviderFoundYours = "Our records indicate that you have already established an Account for this provider location and taxonomy.";
            public const string ExistingProviderFoundOthers = "Our records indicate that another user has already established an Account for this provider.";
            public const string ExistingReferralFoundOthers = "Tax Id, Zip and Zip Ext entered has already been used with a different referral number for another registration.";
            public const string ExistingIndividualFoundOthers = "A provider already exists with the NPI and Provider Type that you have entered.  Please return to the Provider Home page and select the provider in the ‘Other Provider’s using this same Tax ID’ portion on the screen.";
            public const string FoundExistingWhileAdminUpdatesKeyIdentifiers = "The changes entered will result in creating a duplicate Provider in PDMS.  Please use the Provider Search function to find the existing provider in PDMS.";
            public const string ExistingSecondaryTaxonomy = "Chosen Taxonomy is a secondary taxonomy for this registration.";

            public const string WaiverTransactionFailure = "An error occured while processing this transction";
            public const string NoNPIOrTaxonomyFound = "This registration does not have an NPI and one is required, Please select 'Edit Key Provider Identifiers' and add your NPI and associated taxonomy.";
        }

        public static class UserAccountInformation
        {
            public const string UserIDRequired = "User ID is required.";
        }


        public static class PaperRequestQueueData
        {
            public const string PaperRequestQueueIDRequired = "Paper Request Queue ID is required.";
            public const string PaperRequestTypeRequired = "Paper Request Type is required.";
            public const string DocumentTypeRequired = "Document Type is required.";
            public const string DocumentHandleRequired = "Onbase Document Identifier is required.";
            public const string DocumentHandleExists = "The document identifer entered is already associated to a paper request and can not be reused.";
            public const string CommentRequired = "Comments are required.";
            public const string ZipExtValueInvalid = "Zip Extension cannot be all zeroes.";

        }

        public static class CommunicationEventData
        {
            public const string CommunicationEventIDRequired = "Communication Event ID is required.";
        }

        public static class DisenrollmentData
        {
            public const string TermDatePriorToEffectiveDate = "The disenrollment effective date may not be prior to the registration effective date.";
        }

        public static class ExpressTerminationData
        {
            public const string WorkflowRequestUnsuccessful = "The request to create an express workflow was not successful.  Please contact technical support.";
            public const string TermDatePriorToEffectiveDate = "The termination effective date may not be prior to the registration effective date.";
        }

        public static class CallTracking
        {
            public const string RegIDOrNotOnFileRequired = "A value for Reg ID or selection of No Registration on file must be selected.";
            public const string WhoIsCallerRequired = "A value for Who Is Caller is required.";
            public const string ReasonForCallingRequired = "A value for Reason for Calling is required.";
            public const string NextActionRequired = "A value for Office Location is required."; 
            public const string ResolutionRequired = "A value for Action Taken is required.";
            public const string BeginDateRequired = "A value for Start Date is required.";
            public const string EndDateRequired = "A value for End Date is required.";
            public const string StartDateAndEndDateRequired = "Both start and end date must entered to search on date range.";
            public const string AtLeastOneFieldRequired = "At least 1 field must be entered to search";
        }

        public static class ReactivationData
        {
            public const string RevalidationDatePriorToEffectiveDate = "New Re-Enrollment due date must be greater than Effective Date.";
            public const string EffectiveDatePriorToTerminationDate = "If the Effective date is changed it must be greater than or equal to the termination date.";
            public const string RevalidationDatePriorToEndDate = "New Re-Enrollment Due Date must be greater than or equal to the previous end date of the Provider.";
        }
    }
}
