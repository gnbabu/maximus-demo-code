using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDMSRestServices.Models
{
    public class ServiceDetailsInstitutional
    {
        public Int32 PRIOR_AUTH_SERVICE_DETAIL_ID { get; set; }
        public string PRIOR_AUTH_SERVICE_REVENUE_CODE { get; set; }
        public string PRIOR_AUTH_SERVICE_CODE_TYPE_ID { get; set; }
        public string PRIOR_AUTH_SERVICE_CODE_TYPE_TEXT { get; set; }
        public string PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE { get; set; }
        public string PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE { get; set; }
        public string PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS { get; set; }
        public string PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS { get; set; }
        public string PRIOR_AUTH_STATUS_ID { get; set; }

        public string StatusDesc { get; set; }

        public int? PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS { get; set; }

        public int PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS { get; set; }

        public decimal PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR { get; set; }

        public decimal PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR { get; set; }

        public string PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS { get; set; }

        public string PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS { get; set; }

        public string PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC { get; set; }

        public int PRIOR_AUTH_REQUESTED_UNITS_ID { get; set; }

        public string PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE { get; set; }

        public int PRIOR_AUTH_LEVEL_CARE_ID { get; set; }

        public int PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS { get; set; }

        public string PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO { get; set; }

    }

    public class ServiceDetailsProfessional
    {
        public Int32 PRIOR_AUTH_PROFFSERVICE_DETAIL_ID { get; set; }
        public string PRIOR_AUTH_PROCEDURE_CODE_ID { get; set; }
        public string PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1 { get; set; }
        public string PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2 { get; set; }
        public string PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3 { get; set; }
        public string PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4 { get; set; }
        public int? PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS { get; set; }
        public int PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID { get; set; }
        public decimal? PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR { get; set; }
        public string PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS { get; set; }
        public string PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS { get; set; }
        public string PRIOR_AUTH_REQUESTED_UNITS { get; set; }
        public int PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS { get; set; }
        public decimal PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR { get; set; }
        public string PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM { get; set; }
        public string PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC { get; set; }
        public string PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE { get; set; }
        public int PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS { get; set; }
        public string PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS { get; set; }
        public string PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS { get; set; }

        public int Line { get; set; }

        public string PRIOR_AUTH_STATUS_ID { get; set; }
        public string StatusDesc { get; set; }

        public bool isErrorOccured { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class ServiceDetailsDental
    {
        public string PRIOR_AUTH_PROCEDURE_CODE_ID { get; set; }
        public int PRIOR_AUTH_TOOTH_NUMBER_ID { get; set; }
        public string PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS { get; set; }
        public string PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS { get; set; }
        public string PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS { get; set; }
        public string PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS { get; set; }
        public string PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS { get; set; }
        public string PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE { get; set; }
        public string PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE { get; set; }
        public string PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE { get; set; }
        public string PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE { get; set; }
        public string PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE { get; set; }
        public int? PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS { get; set; }
        public decimal? PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR { get; set; }
        public string PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS { get; set; }
        public string PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS { get; set; }
        public string PRIOR_AUTH_STATUS_ID { get; set; }
        public string PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC { get; set; }
        public string PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE { get; set; }
        public Int32 PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID { get; set; }
        public string PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM { get; set; }
        public Int32 PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS { get; set; }
        public decimal PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR { get; set; }
        public string PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS { get; set; }
        public string PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS { get; set; }
        public int PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS { get; set; }
        public Int32 PRIOR_AUTH_DENTALSERVICE_DETAIL_ID { get; set; }
        public Int32 PRIOR_AUTH_DENTALSAVE_ID { get; set; }
        public string Line { get; set; }
        public string MedicaidID { get; set; }

        public string StatusDesc { get; set; }

        public string PRIOR_AUTH_TOOTH_NUMBER_CODE { get; set; }
    }


    public class DentalToothInfo
    {
        public string PRIOR_AUTH_TOOTH_NUMBER_ID { get; set; }
        public string PRIOR_AUTH_TOOTH_NUMBER_CODE { get; set; }
    }

    public class ServiceDetailsIstitutionalSaveReq
    {
        public string codeType { get; set; }
        public string procedureCode { get; set; }
        public string procCodeDesc { get; set; }
        public string providerServiceNote { get; set; }
        public string revenueCode { get; set; }
        public string levelCareID { get; set; }
        public string reqUnits { get; set; }
        public string unitMeasure { get; set; }
        public string requnitsfee { get; set; }
        public string fdos { get; set; }
        public string tdos { get; set; }
        public string status { get; set; }
        public string MedicaidId { get; set; }
        public string trackingNo { get; set; }
        public string codeTypeText { get; set; }
        public string operation { get; set; }
        public string lineNumber { get; set; }

        public string operationType { get; set; }
    }
    public class ServiceDetailsProfessionalSaveReq
    {
        public string procedureCode { get; set; }
        public string procCodeDesc { get; set; }
        public string providerServiceNote { get; set; }
        public string reqUnits { get; set; }
        public string remainUnits { get; set; }
        public string requnitsfee { get; set; }
        public string fdos { get; set; }
        public string tdos { get; set; }
        public string status { get; set; }
        public string unitMeasurement { get; set; }
        public string modifier1 { get; set; }
        public string modifier2 { get; set; }
        public string modifier3 { get; set; }
        public string modifier4 { get; set; }
        public string MedicaidId { get; set; }
        public string serviceTrackingNo { get; set; }
        public string operation { get; set; }
        public string lineNumber { get; set; }

    }
    public class ServiceDetailsDentalSaveReq
    {
        public string procedureCode { get; set; }
        public string procCodeDesc { get; set; }
        public string providerServiceNote { get; set; }
        public string reqUnits { get; set; }
        public string remainUnits { get; set; }
        public string requnitsfee { get; set; }
        public string fdos { get; set; }
        public string tdos { get; set; }
        public string status { get; set; }
        public string toothNum { get; set; }
        public string prosthesisNum { get; set; }
        public string cavity1 { get; set; }
        public string cavity2 { get; set; }
        public string cavity3 { get; set; }
        public string cavity4 { get; set; }
        public string cavity5 { get; set; }
        public string surface1 { get; set; }
        public string surface2 { get; set; }
        public string surface3 { get; set; }
        public string surface4 { get; set; }
        public string surface5 { get; set; }
        public string MedicaidId { get; set; }
        public string serviceTrackingNo { get; set; }
        public string operation { get; set; }
        public string lineNumber { get; set; }
    }
}