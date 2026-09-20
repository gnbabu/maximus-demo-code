using Corp.Core.Libraries.PriorAuthServiceReference;
using MAXIMUS.Core.Libraries;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace Corp.Core.Libraries
{
    public static class PriorAuthServiceController
    {

        public static PriorAuthRequest278Type fillPriorAuthRequest278Type(DataSet ds)
        {
            PriorAuthRequest278Type atype = new PriorAuthRequest278Type();
            atype.InterchangeControlHeader = fillISAType(ds);
            atype.FunctionalGroupHeader = fillGSType(ds);
            atype.TransactionSetHeader = fillSTType(ds);
            atype.BHTContainter = fillBHTContainterType(ds);
            atype.TransactionSetTrailer = fillSEType(ds);
            atype.FunctionalGroupTrailer = fillGEType(ds);
            atype.InterchangeControlTrailer = fillIEAType(ds);
            return atype;
        }

        public static MessageHeaderTypeBusinessFlow sendToBusinessFlow(int sendTo)
        {
            if (sendTo == CON.PriorAuthBusinessFlow.AddUpdatePriorAuth)
            {
                return MessageHeaderTypeBusinessFlow.AddUpdatePriorAuth;
            }
            else if (sendTo == CON.PriorAuthBusinessFlow.InquirePriorAuth)
            {
                return MessageHeaderTypeBusinessFlow.InquirePriorAuth;
            }
            else
            {
                return MessageHeaderTypeBusinessFlow.SearchPriorAuth;
            }
        }

        public static MessageHeaderTypeSubscriber[] sendToSubscriberService(int sendTo)
        {
            int count = 1;
            if (sendTo > 1)
            {
                count = 2;
            }

            MessageHeaderTypeSubscriber[] mhts = new MessageHeaderTypeSubscriber[count];
            if (sendTo == CON.PriorAuthSubscriber.FI)
            {
                mhts[0] = MessageHeaderTypeSubscriber.FI;
            }
            else if (sendTo == CON.PriorAuthSubscriber.EDI)
            {
                mhts[0] = MessageHeaderTypeSubscriber.EDI;
            }
            else
            {
                mhts[0] = MessageHeaderTypeSubscriber.EDI;
                mhts[1] = MessageHeaderTypeSubscriber.FI;
            }
            return mhts;
        }

        public static MessageHeaderType fillMessageHeader(int passThroughTxID, int subscriber, int businessFlow)
        {
            MessageHeaderType mht = new MessageHeaderType();
            mht.BusinessFlow = sendToBusinessFlow(businessFlow);
            mht.SubscriberSystem = sendToSubscriberService(subscriber);
            mht.StateCode = "OH";
            mht.ModuleTransactionId = passThroughTxID.ToString();
            mht.RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
            mht.AdditionalModuleTransactionId = Guid.NewGuid().ToString();
            mht.SITransactionKey = PriorAuthServiceHelper.GetUniqueKey(32);
            mht.RequestorSystem = MessageHeaderTypeRequestorSystem.PNM;
            return mht;
        }

        public static ISAType fillISAType(DataSet ds)
        {

            string destinationPayerID = string.Empty;
            string transactionID = string.Empty;

            DataTable dt = new DataTable();
            if (PriorAuthServiceHelper.HasRows(ds))
            {
                if (ds.Tables.Contains("PriorAuthHeader"))
                {
                    dt = ds.Tables["PriorAuthHeader"];
                    destinationPayerID = dt.Rows[0]["PriorAuthDestinationPayerMCEID"].ToString();
                }

                if (ds.Tables.Contains("PriorAuthRequestType"))
                {
                    dt = ds.Tables["PriorAuthRequestType"];
                    transactionID = dt.Rows[0]["transactionID"].ToString();
                }
            }

            ISAType isatyp = new ISAType();
            isatyp.ISA01_AuthorizationInformationQualifier = "00";
            isatyp.ISA03_SecurityInformationQualifier = "00";
            isatyp.ISA04_SecurityInformation = PriorAuthServiceHelper.fillSpaces(10);
            isatyp.ISA05_InterchangeIdQualifier = "ZZ";
            isatyp.ISA06_InterchangeSenderId = "MMISODJFS";
            isatyp.ISA07_InterchangeIdQualifier = "ZZ";
            isatyp.ISA08_InterchangeReceiverId = destinationPayerID;
            isatyp.ISA09_InterchangeDate = DateTime.Now.ToString("yyMMdd");
            isatyp.ISA10_InterchangeTime = DateTime.Now.ToString("HHMM");
            isatyp.ISA11_RepetitionSeparator = "^";
            isatyp.ISA12_InterchangeControlVersionNumber = "00501";
            isatyp.ISA13_InterchangeControlNumber = transactionID.PadLeft(9, '0');
            isatyp.ISA14_AcknowledgementRequested = "1";
            isatyp.ISA15_InterchangeUsageIndicator = AppSettings.Get("PAUsageIndicator");
            isatyp.ISA16_ComponentElementSeparator = ":";

            return isatyp;
        }

        public static GSType fillGSType(DataSet ds)
        {

            string destinationPayerID = string.Empty;
            string transactionID = string.Empty;

            DataTable dt = new DataTable();
            if (PriorAuthServiceHelper.HasRows(ds))
            {
                if (ds.Tables.Contains("PriorAuthHeader"))
                {
                    dt = ds.Tables["PriorAuthHeader"];
                    destinationPayerID = dt.Rows[0]["PriorAuthDestinationPayerMCEID"].ToString();
                }

                if (ds.Tables.Contains("PriorAuthRequestType"))
                {
                    dt = ds.Tables["PriorAuthRequestType"];
                    transactionID = dt.Rows[0]["transactionID"].ToString();
                }
            }

            GSType gty = new GSType();
            gty.GS01_FunctionalIdentifierCode = "HI";
            gty.GS02_ApplicationSendersCode = "MMISODJFS";
            gty.GS03_ApplicationReceiversCode = destinationPayerID;
            gty.GS04_Date = DateTime.Now.ToString("yyyyMMdd");
            gty.GS05_Time = DateTime.Now.ToString("HHMM");
            gty.GS06_GroupControlNumber = transactionID.PadLeft(9, '0');
            gty.GS07_ResponsibleAgencyCode = "X";
            gty.GS08_VersionReleaseIndustryIdentifierCode = "005010X217";

            return gty;
        }

        public static STType fillSTType(DataSet ds)
        {
            string transactionID = string.Empty;

            DataTable dt = new DataTable();
            if (PriorAuthServiceHelper.HasRows(ds))
            {
                if (ds.Tables.Contains("PriorAuthRequestType"))
                {
                    dt = ds.Tables["PriorAuthRequestType"];
                    transactionID = dt.Rows[0]["transactionID"].ToString();
                }
            }


            STType sty = new STType();
            sty.ST01_TransactionSetIdentifierCode = "278";
            sty.ST02_TransactionSetControlNumber = transactionID.PadLeft(9, '0');
            sty.ST03_ImplementationConventionReference = "005010X217";

            return sty;
        }


        public static BHTType fillBHTType(string passThroughTxID, string reqType)
        {
            BHTType bht = new BHTType();
            bht.BHT01_HeirarchicalStructureCode = "0007";
            bht.BHT02_TransactionSetPurposeCode = reqType;
            bht.BHT03_SubmitterTransactionIdentifier = passThroughTxID.PadLeft(9, '0');
            bht.BHT04_TransactionSetCreationDate = DateTime.Now.ToString("yyyyMMdd");
            bht.BHT05_TransactionSetCreationTime = DateTime.Now.ToString("HHMM");

            return bht;
        }

        public static UMODetails_2000AType fillUMODetails_2000AType(DataSet ds)
        {
            string destinationPayer = string.Empty;
            string entityType = string.Empty;
            string medID = string.Empty;
            string destinationPayerID = string.Empty;

            DataTable dt = new DataTable();
            if (PriorAuthServiceHelper.HasRows(ds))
            {
                if (ds.Tables.Contains("PriorAuthHeader"))
                {
                    dt = ds.Tables["PriorAuthHeader"];
                    destinationPayer = dt.Rows[0]["PriorAuthDestinationPayer"].ToString();
                    destinationPayerID = dt.Rows[0]["PriorAuthDestinationPayerID"].ToString();
                    entityType = dt.Rows[0]["ENTITY_TYPE_ID"].ToString();
                    if (!entityType.Equals("1"))
                    {
                        entityType = "2";
                    }
                    medID = dt.Rows[0]["MEDICAID_ID"].ToString();
                }
            }


            HLType hlt = new HLType();
            hlt.HL01_HeirarchicalIdNumber = "1";
            hlt.HL03_HeirarchicalLevelCode = "20";
            hlt.HL04_HerarchicalChildCode = "1";

            NM1Type nmlevel = new NM1Type();
            nmlevel.NM101_EntityIdentifierCode = "PR";
            nmlevel.NM102_EntityTypeQualifier = "2"; //VM : OHPNM-8247 : Defaulted to 2 as per the X12 to XML mapping
            nmlevel.NM103_UMOLastOrOrganizationName = destinationPayer.Length <= 35 ? destinationPayer : destinationPayer.Substring(0, 35);
            nmlevel.NM108_IdentificationCodeQualifier = "PI";
            nmlevel.NM109_UMOIdentifier = destinationPayerID;

            UMONameDetails_2010AType umo = new UMONameDetails_2010AType();
            umo.UMOName_2010A = nmlevel;

            UMODetails_2000AType umo2000 = new UMODetails_2000AType();
            umo2000.UMOLevel_2000A = hlt;
            umo2000.UMONameDetails_2010A = umo;
            return umo2000;
        }

        public static RequesterDetails_2000BType fillRequesterDetails_2000B(DataSet ds)
        {

            string npi = string.Empty;
            string entity = string.Empty;
            string lastName = string.Empty;
            string orgName = string.Empty;
            string firstName = string.Empty;
            string middleName = string.Empty;
            string suffix = string.Empty;
            DataTable dt = new DataTable();
            if (PriorAuthServiceHelper.HasRows(ds))
            {
                if (ds.Tables.Contains("PriorAuthHeader"))
                {
                    dt = ds.Tables["PriorAuthHeader"];
                    npi = dt.Rows[0]["NPI"].ToString();
                    entity = dt.Rows[0]["ENTITY_TYPE_ID"].ToString();
                    lastName = dt.Rows[0]["LAST_NAME"].ToString();
                    orgName = dt.Rows[0]["NAME"].ToString();
                    firstName = dt.Rows[0]["FIRST_NAME"].ToString();
                    middleName = dt.Rows[0]["MIDDLE_INITIAL"].ToString();
                    suffix = dt.Rows[0]["TITLE"].ToString();
                }
            }

            string ContactFirstName = string.Empty;
            string ContactLastName = string.Empty;
            string ContactContactNumber = string.Empty;
            string ContactContactExt = string.Empty;

            DataTable dt2 = new DataTable();
            if (PriorAuthServiceHelper.HasRows(ds))
            {
                if (ds.Tables.Contains("PriorAuthRequestorContactInfo"))
                {
                    dt2 = ds.Tables["PriorAuthRequestorContactInfo"];
                    ContactFirstName = dt2.Rows[0]["ContactFirstName"].ToString();
                    ContactLastName = dt2.Rows[0]["ContactLastName"].ToString();
                    ContactContactNumber = dt2.Rows[0]["ContactContactNumber"].ToString();
                    ContactContactExt = dt2.Rows[0]["ContactContactExt"].ToString();
                }
            }


            HLType hlt = new HLType();
            hlt.HL01_HeirarchicalIdNumber = "2";
            hlt.HL02_HeirarchicalParentIdNumber = "1";
            hlt.HL03_HeirarchicalLevelCode = "21";
            hlt.HL04_HerarchicalChildCode = "1";

            NM1Type nm1 = new NM1Type();
            if (!entity.Equals("4"))
            {
                nm1.NM101_EntityIdentifierCode = "1P";
            }
            else
            {
                nm1.NM101_EntityIdentifierCode = "FA";
            }


            if (entity.Equals("1"))
            {
                nm1.NM102_EntityTypeQualifier = "1";
                if (!string.IsNullOrEmpty(lastName) && !string.IsNullOrWhiteSpace(lastName))
                {
                    nm1.NM103_UMOLastOrOrganizationName = lastName.Length <= 35 ? lastName.Trim() : lastName.Substring(0, 35).Trim(); ;
                }
                if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrWhiteSpace(firstName))
                {
                    nm1.NM104_UMOFirstName = firstName.Trim();
                }
                if (!string.IsNullOrEmpty(middleName) && !string.IsNullOrWhiteSpace(middleName))
                {
                    nm1.NM105_UMOMiddleName = middleName.Trim();
                }
                if (!string.IsNullOrEmpty(suffix) && !string.IsNullOrWhiteSpace(suffix))
                {
                    nm1.NM107_UMONameSuffix = suffix.Trim();
                }

            }
            else
            {
                nm1.NM102_EntityTypeQualifier = "2";
                if (!string.IsNullOrEmpty(orgName) && !string.IsNullOrWhiteSpace(orgName))
                {
                    nm1.NM103_UMOLastOrOrganizationName = orgName.Trim().Length <= 35 ? orgName.Trim() : orgName.Trim().Substring(0, 35);
                }
            }

            nm1.NM108_IdentificationCodeQualifier = "XX";
            nm1.NM109_UMOIdentifier = npi;

            string contactNum = Regex.Replace(ContactContactNumber, @"[^\d]", "");

            PERType n5 = new PERType();
            n5.PER01_ContactFunctionCode = "IC";
            n5.PER02_RequesterContactName = ContactLastName + " " + ContactFirstName;
            n5.PER03_CommunicationNumberQualifier = "TE";
            n5.PER04_RequesterContactCommunicationNumber = contactNum;
            n5.PER05_CommunicationNumberQualifier = "EX";
            n5.PER06_RequesterContactCommunicationNumber = ContactContactExt;

            RequesterNameDetails_2010BType rname = new RequesterNameDetails_2010BType();
            rname.RequesterName_2010B = nm1;
            rname.RequesterContactInformation_2010B = n5;

            RequesterDetails_2000BType req2000 = new RequesterDetails_2000BType();
            req2000.RequesterLevel_2000B = hlt;
            req2000.RequesterNameDetails_2010B = rname;

            return req2000;
        }

        public static SubscriberDetails_2000CType fillSubscriberDetails_2000CType(DataSet ds)
        {
            string MedicaidBillingNumber = string.Empty;
            string LastName = string.Empty;
            string FirstName = string.Empty;
            string MiddleName = string.Empty;
            string DOB = string.Empty;
            string PatientTrackingNumber = string.Empty;
            string Gender = string.Empty;
            string AddressLine1 = string.Empty;
            string AddressLine2 = string.Empty;
            string City = string.Empty;
            string State = string.Empty;
            string ZipCode = string.Empty;

            DataTable dt = new DataTable();
            if (PriorAuthServiceHelper.HasRows(ds))
            {
                if (ds.Tables.Contains("PriorAuthRecipientInfo"))
                {
                    dt = ds.Tables["PriorAuthRecipientInfo"];
                    MedicaidBillingNumber = dt.Rows[0]["MedicaidBillingNumber"].ToString();
                    LastName = dt.Rows[0]["LastName"].ToString();
                    FirstName = dt.Rows[0]["FirstName"].ToString();
                    MiddleName = dt.Rows[0]["MiddleName"].ToString();
                    DOB = dt.Rows[0]["DOB"].ToString();
                    PatientTrackingNumber = dt.Rows[0]["PatientTrackingNumber"].ToString();
                    Gender = dt.Rows[0]["Gender"].ToString();
                    AddressLine1 = dt.Rows[0]["AddressLine1"].ToString();
                    AddressLine2 = dt.Rows[0]["AddressLine2"].ToString();
                    City = dt.Rows[0]["City"].ToString();
                    State = dt.Rows[0]["State"].ToString();
                    ZipCode = dt.Rows[0]["ZipCode"].ToString();
                }
            }


            HLType hl = new HLType();
            hl.HL01_HeirarchicalIdNumber = "3";
            hl.HL02_HeirarchicalParentIdNumber = "2";
            hl.HL03_HeirarchicalLevelCode = "22";
            hl.HL04_HerarchicalChildCode = "1";

            N3Type n3 = new N3Type();
            n3.N301_RequesterAddressLine = AddressLine1.Trim();
            if (!string.IsNullOrEmpty(AddressLine2.Trim()) && !string.IsNullOrWhiteSpace(AddressLine2.Trim()))
            {
                n3.N302_RequesterAddressLine = AddressLine2.Trim();
            }

            NM1Type nm = new NM1Type();
            nm.NM101_EntityIdentifierCode = "IL";
            nm.NM102_EntityTypeQualifier = "1";

            nm.NM103_UMOLastOrOrganizationName = string.IsNullOrEmpty(LastName) ? string.Empty : LastName.Length <= 35 ? LastName : LastName.Substring(0, 35);

            nm.NM104_UMOFirstName = FirstName;
            nm.NM105_UMOMiddleName = MiddleName;
            nm.NM108_IdentificationCodeQualifier = "MI";
            nm.NM109_UMOIdentifier = MedicaidBillingNumber;

            N4Type n4 = new N4Type();
            n4.N401_RequesterCityName = City;
            n4.N402_RequesterStateOrProvinceCode = State;
            n4.N403_RequesterPostalZoneOrZipCode = ZipCode;

            DMGType dmg = new DMGType();
            dmg.DMG01_DateTimePeriodFormatQualifier = "D8";

            DateTime dtDOB;

            if (DateTime.TryParse(DOB, out dtDOB))
            {
                dmg.DMG02_SubscriberBirthDate = dtDOB.ToString("yyyyMMdd");
            }

            if (Gender.Equals("Female") || Gender.Equals("F"))
            {
                dmg.DMG03_SubscriberGenderCode = "F";
            }
            else if (Gender.Equals("Male") || Gender.Equals("M"))
            {
                dmg.DMG03_SubscriberGenderCode = "M";
            }
            else
            {
                dmg.DMG03_SubscriberGenderCode = "U";
            }


            SubscriberNameDetails_2010CType sntyp = new SubscriberNameDetails_2010CType();
            sntyp.SubscriberAddress_2010C = n3;
            sntyp.SubscriberName_2010C = nm;
            sntyp.SubscriberCityStateZipCode_2010C = n4;
            sntyp.SubscriberDemographicInformation_2010C = dmg;

            SubscriberDetails_2000CType req2000 = new SubscriberDetails_2000CType();
            req2000.SubscriberLevel_2000C = hl;
            req2000.SubscriberNameDetails_2010C = sntyp;


            return req2000;
        }

        public static DependentDetails_2000DType fillDependentDetails_2000DType(DataSet ds)
        {
            DependentDetails_2000DType req2000 = new DependentDetails_2000DType();


            return req2000;
        }

        public static PatientEventDetails_2000EType fillPatientEventDetails_2000EType(DataSet ds)
        {
            DataTable diagDT = new DataTable();
            DataTable attachDT = new DataTable();

            string scCode = string.Empty;
            string assType = string.Empty;
            string PatientTrackingNumber = string.Empty;
            string taxID = string.Empty;
            string paType = string.Empty;
            string delayRsn = string.Empty;
            string lvlSvc = string.Empty;
            string facType = string.Empty;
            string accDate = string.Empty;
            string associatePANum = string.Empty;
            string placeOfService = string.Empty;

            string mensDate = string.Empty;
            string dobDate = string.Empty;
            string illDate = string.Empty;
            string ptnEventDate = string.Empty;
            string admDate = string.Empty;
            string disDate = string.Empty;

            string admType = string.Empty;
            string admSrc = string.Empty;
            string discStat = string.Empty;
            string provNotes = string.Empty;
            string entityType = string.Empty;
            string svcLastName = string.Empty;
            string svcFirstName = string.Empty;
            string svcNPI = string.Empty;
            string svcMedicaidID = string.Empty;

            string ordLastName = string.Empty;
            string ordFirstName = string.Empty;
            string ordNPI = string.Empty;
            string ordMedicaidID = string.Empty;
            string paNumber = string.Empty;
            int countDiag = 0;
            int countAttch = 0;

            string reqType = "I";

            DataTable dt = new DataTable();
            DataTable dtServiceDetail = new DataTable();

            if (PriorAuthServiceHelper.HasRows(ds))
            {
                if (ds.Tables.Contains("PriorAuthHeader"))
                {
                    dt = ds.Tables["PriorAuthHeader"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        scCode = dt.Rows[0]["PriorAuthServiceType"].ToString();
                        assType = dt.Rows[0]["PriorAuthAssignment"].ToString();
                        provNotes = dt.Rows[0]["PriorAuthProviderNotes"].ToString();
                        paNumber = dt.Rows[0]["PANumber"].ToString();
                        taxID = dt.Rows[0]["TAX_ID"].ToString();
                        paType = dt.Rows[0]["PriorAuthType"].ToString();
                        reqType = dt.Rows[0]["PARequestType"].ToString();

                        entityType = dt.Rows[0]["ENTITY_TYPE_ID"].ToString();
                        if (!entityType.Trim().Equals("1"))
                        {
                            entityType = "2";
                        }
                    }
                }
                if (ds.Tables.Contains("PriorAuthRecipientInfo"))
                {
                    dt = ds.Tables["PriorAuthRecipientInfo"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        PatientTrackingNumber = dt.Rows[0]["PatientTrackingNumber"].ToString();
                    }
                }
                if (ds.Tables.Contains("PriorAuthServiceInformation"))
                {
                    dt = ds.Tables["PriorAuthServiceInformation"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (paType.ToUpper() != "INSTITUTIONAL")
                        {
                            placeOfService = dt.Rows[0]["PlaceofService"].ToString();
                        }

                        delayRsn = dt.Rows[0]["DelayReason"].ToString();
                        lvlSvc = dt.Rows[0]["LevelOfService"].ToString();
                        facType = dt.Rows[0]["FacilityType"].ToString();
                        facType = facType.Contains("-") ? facType.Split('-')[0].TrimStart('0').Trim() : facType.Trim();
                        accDate = dt.Rows[0]["AccidentDate"].ToString();
                        mensDate = dt.Rows[0]["DateOfLastMenstrualPeriod"].ToString();
                        dobDate = dt.Rows[0]["EstimatedDateOfBirth"].ToString();
                        illDate = dt.Rows[0]["DateOfOnsetOfIllness"].ToString();
                        ptnEventDate = dt.Rows[0]["DateOfPatientEvent"].ToString();
                        admDate = dt.Rows[0]["AdmissionDate"].ToString();
                        disDate = dt.Rows[0]["DischargeDate"].ToString();
                        admType = dt.Rows[0]["AdmissionType"].ToString();
                        admSrc = dt.Rows[0]["AdmissionSource"].ToString();
                        discStat = dt.Rows[0]["DischargeStatus"].ToString();

                        if (dt.Rows[0]["AssociatedPANo"] != null)
                        {
                            associatePANum = dt.Rows[0]["AssociatedPANo"].ToString(); //Associate PA No.
                        }
                    }
                }

                if (ds.Tables.Contains("PriorAuthDiagnosisProvider"))
                {
                    diagDT = ds.Tables["PriorAuthDiagnosisProvider"];
                }
                if (ds.Tables.Contains("PriorAuthAttachments"))
                {
                    attachDT = ds.Tables["PriorAuthAttachments"];
                    countAttch = attachDT.Rows.Count;
                }
                if (ds.Tables.Contains("PriorAuthServicingProvider"))
                {
                    dt = ds.Tables["PriorAuthServicingProvider"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        svcNPI = dt.Rows[0]["ServiceProvider"].ToString();
                        svcLastName = dt.Rows[0]["LastName"].ToString();
                        svcFirstName = dt.Rows[0]["FirstName"].ToString();
                        svcMedicaidID = dt.Rows[0]["MEDICAID_ID"].ToString();
                    }
                }
                if (ds.Tables.Contains("PriorAuthOrderingProvider"))
                {
                    dt = ds.Tables["PriorAuthOrderingProvider"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ordNPI = dt.Rows[0]["OrderingProvider"].ToString();
                        ordLastName = dt.Rows[0]["LastName"].ToString();
                        ordFirstName = dt.Rows[0]["FirstName"].ToString();
                        ordMedicaidID = dt.Rows[0]["MEDICAID_ID"].ToString();
                    }
                }

                if (ds.Tables.Contains("PriorAuthServiceDetails"))
                {
                    dtServiceDetail = ds.Tables["PriorAuthServiceDetails"];
                }
            }

            DataSet dsSVCProv = InfoAccessController.getServiceProviderDetailsByMedID(svcMedicaidID);
            DataTable dtSVCProv = new DataTable();
            string addrSVC1 = string.Empty;
            string addrSVC2 = string.Empty;

            string citySVC = string.Empty;
            string stateSVC = string.Empty;
            string zipSVC = string.Empty;

            string cnameSVC = string.Empty;
            string phoneSVC = string.Empty;
            string emailSVC = string.Empty;
            string faxSVC = string.Empty;
            string taxonomySVC = string.Empty;
            string svcProviderEntityTypeId = string.Empty;

            if (PriorAuthServiceHelper.HasRows(dsSVCProv))
            {
                dtSVCProv = dsSVCProv.Tables[0];
                if (dtSVCProv != null && dtSVCProv.Rows.Count > 0)
                {
                    addrSVC1 = dtSVCProv.Rows[0]["ADDRESS_1"].ToString();
                    addrSVC2 = dtSVCProv.Rows[0]["ADDRESS_2"].ToString();

                    citySVC = dtSVCProv.Rows[0]["CITY"].ToString();
                    stateSVC = dtSVCProv.Rows[0]["STATE"].ToString();
                    zipSVC = dtSVCProv.Rows[0]["ZIP"].ToString();
                    cnameSVC = dtSVCProv.Rows[0]["NAME"].ToString();
                    phoneSVC = dtSVCProv.Rows[0]["PHONE"].ToString();
                    emailSVC = dtSVCProv.Rows[0]["EMAIL"].ToString();
                    faxSVC = dtSVCProv.Rows[0]["FAX"].ToString();

                    taxonomySVC = dtSVCProv.Rows[0]["TAXONOMY"].ToString();
                    svcProviderEntityTypeId = dtSVCProv.Rows[0]["ENTITY_TYPE_ID"].ToString();
                }
            }

            //This is only called if "svcProviderEntityTypeId" is null or Empty when the "dsSVCProv" has no rows.
            if (string.IsNullOrEmpty(svcProviderEntityTypeId))
            {
                DataTable dtSvcProvDetails = new DataTable();
                DataSet dsSvcProvDetails = InfoAccessController.GetServiceProviderDetailsByGRPMedicaidID(svcMedicaidID);
                if (dsSvcProvDetails != null)
                {
                    dtSvcProvDetails = dsSvcProvDetails.Tables[0];
                    if (dtSvcProvDetails != null && dtSvcProvDetails.Rows.Count > 0)
                    {
                        svcProviderEntityTypeId = dtSvcProvDetails.Rows[0]["ENTITY_TYPE_ID"].ToString();
                    }
                }
            }

            HLType hl = new HLType();
            hl.HL01_HeirarchicalIdNumber = "4";
            hl.HL02_HeirarchicalParentIdNumber = "3";
            hl.HL03_HeirarchicalLevelCode = "EV";
            hl.HL04_HerarchicalChildCode = "1";


            string serviceTracking = string.Empty;

            switch (paType.ToUpper())
            {
                case "DENTAL":
                    if (dtServiceDetail != null && dtServiceDetail.Rows.Count > 0)
                        serviceTracking = dtServiceDetail.Rows[0]["PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM"].ToString();
                    break;
                case "PROFESSIONAL":
                    if (dtServiceDetail != null && dtServiceDetail.Rows.Count > 0)
                        serviceTracking = dtServiceDetail.Rows[0]["PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM"].ToString();
                    break;
                case "INSTITUTIONAL":
                    if (dtServiceDetail != null && dtServiceDetail.Rows.Count > 0)
                        serviceTracking = dtServiceDetail.Rows[0]["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"].ToString();
                    break;
            }



            PatientEventDetails_2000EType req2000 = new PatientEventDetails_2000EType();
            req2000.PatientEventLevel_2000E = hl;

            UpdatePatientEventTrackingNumber2000E(PatientTrackingNumber, taxID, req2000);

            UpdateHealthCareServicesReviewInformation2000E(paType, serviceTracking, placeOfService, facType, scCode, lvlSvc, delayRsn, accDate, req2000, reqType);

            UpdatePreviousReviewAuthorizationNumber2000E(associatePANum, paNumber, reqType, req2000);

            // UpdateAdministrativeReferenceNumber2000F(paNumber, req2000);

            UpdatePatientEventDetails2000E(accDate, mensDate, dobDate, illDate, ptnEventDate, admDate, disDate, req2000);

            UpdatePatientDiagnosis2000E(diagDT, req2000);

            UpdateInstitutionalClaimCode2000E(paType, admType, admSrc, discStat, req2000);


            UpdateAdditionalPatientInformation2000E(attachDT, req2000);


            UpdateMessageText2000E(ds, assType, req2000);
            UpdateProviderDetails2010EA(ordMedicaidID, ordNPI, ordLastName, ordFirstName, cnameSVC, phoneSVC, emailSVC, faxSVC, svcNPI, svcProviderEntityTypeId, svcLastName, svcFirstName,
                                        addrSVC1, addrSVC2, citySVC, stateSVC, zipSVC, taxonomySVC, req2000);


            return req2000;
        }

        private static void UpdateProviderDetails2010EA(string ordMedicaidID, string ordNPI, string ordLastName, string ordFirstName, string cnameSVC, string phoneSVC, string emailSVC, string faxSVC, string svcNPI, string svcProviderEntityTypeId, string svcLastName, string svcFirstName, string addrSVC1,
                                                       string addrSVC2, string citySVC, string stateSVC, string zipSVC, string taxonomySVC, PatientEventDetails_2000EType res)
        {
            try
            {
                //service / ordering provider node
                ProviderDetails_2010EAType[] provD = new ProviderDetails_2010EAType[2];

                #region Service Provider 2010EA

                PERType perT = new PERType();
                perT.PER01_ContactFunctionCode = "IC";


                if (!string.IsNullOrEmpty(cnameSVC) && !string.IsNullOrWhiteSpace(cnameSVC))
                {
                    perT.PER02_RequesterContactName = cnameSVC;
                }
                perT.PER03_CommunicationNumberQualifier = "TE";
                perT.PER04_RequesterContactCommunicationNumber = phoneSVC;

                if (!string.IsNullOrEmpty(emailSVC) && !string.IsNullOrWhiteSpace(emailSVC))
                {
                    perT.PER05_CommunicationNumberQualifier = "EM";
                    perT.PER06_RequesterContactCommunicationNumber = emailSVC.Trim();
                }
                if (!string.IsNullOrEmpty(faxSVC) && !string.IsNullOrWhiteSpace(faxSVC))
                {
                    perT.PER07_CommunicationNumberQualifier = "FX";
                    perT.PER08_RequesterContactCommunicationNumber = faxSVC;
                }

                NM1Type nm1 = new NM1Type();
                nm1.NM101_EntityIdentifierCode = "SJ";
                nm1.NM108_IdentificationCodeQualifier = "XX";
                nm1.NM109_UMOIdentifier = svcNPI;

                if (svcProviderEntityTypeId.Trim().Equals("1")) //Entity Type ID comes from SVC Provider Panel 
                {
                    nm1.NM102_EntityTypeQualifier = "1";
                    nm1.NM103_UMOLastOrOrganizationName = string.IsNullOrEmpty(svcLastName)
                      ? string.Empty
                      : svcLastName.Length <= 35
                          ? svcLastName
                          : svcLastName.Substring(0, 35);
                    nm1.NM104_UMOFirstName = svcFirstName.Trim();
                }
                else
                {
                    nm1.NM102_EntityTypeQualifier = "2";


                    nm1.NM103_UMOLastOrOrganizationName = string.IsNullOrEmpty(svcLastName)
                        ? string.Empty
                        : svcLastName.Length <= 35
                            ? svcLastName
                            : svcLastName.Substring(0, 35);

                }

                //run query to retrieve address
                N3Type nm3 = new N3Type();
                nm3.N301_RequesterAddressLine = addrSVC1;

                if (!string.IsNullOrEmpty(addrSVC2.Trim()) && !string.IsNullOrWhiteSpace(addrSVC2.Trim()))
                {
                    nm3.N302_RequesterAddressLine = addrSVC2;
                }

                N4Type nm4 = new N4Type();
                nm4.N401_RequesterCityName = citySVC;
                nm4.N402_RequesterStateOrProvinceCode = stateSVC;
                nm4.N403_RequesterPostalZoneOrZipCode = zipSVC;

                PRVType pr1 = new PRVType();
                pr1.PRV01_ProviderCode = "PE";
                pr1.PRV02_ReferenceIdentificationQualifier = "PXC";
                pr1.PRV03_ProviderTaxonomyCode = taxonomySVC; //taxonomy value

                //service provider node 2010EA
                provD[0] = new ProviderDetails_2010EAType();
                provD[0].PatientEventProviderName_2010EA = nm1; //used
                provD[0].PatientEventProviderAddress_2010EA = nm3; //used
                provD[0].PatientEventProviderCityStateZipCode_2010EA = nm4; //used
                provD[0].PatientEventProviderContactInformation_2010EA = perT; //used
                provD[0].PatientEventProviderInformation_2010EA = pr1; //used

                #endregion

                #region Ordering Provider 2010EA


                //ordering provider node 2010EA

                DataSet dsORDProv = InfoAccessController.getServiceProviderDetailsByMedID(ordMedicaidID);
                DataTable dtORDProv = new DataTable();
                string addrORD1 = string.Empty;
                string addrORD2 = string.Empty;

                string cityORD = string.Empty;
                string stateORD = string.Empty;
                string zipORD = string.Empty;

                string cnameORD = string.Empty;
                string phoneORD = string.Empty;
                string emailORD = string.Empty;
                string faxORD = string.Empty;
                string taxonomyORD = string.Empty;
                string ordProviderEntityTypeId = string.Empty;

                if (PriorAuthServiceHelper.HasRows(dsORDProv))
                {
                    dtORDProv = dsORDProv.Tables[0];
                    addrORD1 = dtORDProv.Rows[0]["ADDRESS_1"].ToString();
                    addrORD2 = dtORDProv.Rows[0]["ADDRESS_2"].ToString();

                    cityORD = dtORDProv.Rows[0]["CITY"].ToString();
                    stateORD = dtORDProv.Rows[0]["STATE"].ToString();
                    zipORD = dtORDProv.Rows[0]["ZIP"].ToString();
                    cnameORD = dtORDProv.Rows[0]["NAME"].ToString();
                    phoneORD = dtORDProv.Rows[0]["PHONE"].ToString();
                    emailORD = dtORDProv.Rows[0]["EMAIL"].ToString();
                    faxORD = dtORDProv.Rows[0]["FAX"].ToString();

                    taxonomyORD = dtORDProv.Rows[0]["TAXONOMY"].ToString();
                    ordProviderEntityTypeId = dtORDProv.Rows[0]["ENTITY_TYPE_ID"].ToString();
                }

                if (!string.IsNullOrEmpty(ordNPI) && !string.IsNullOrWhiteSpace(ordNPI))
                {
                    NM1Type nm1ORD = new NM1Type();
                    nm1ORD.NM101_EntityIdentifierCode = "DK";
                    nm1ORD.NM108_IdentificationCodeQualifier = "XX";
                    nm1ORD.NM109_UMOIdentifier = ordNPI;

                    if (ordProviderEntityTypeId.Trim().Equals("1"))
                    {
                        nm1ORD.NM102_EntityTypeQualifier = "1";
                        nm1ORD.NM103_UMOLastOrOrganizationName = string.IsNullOrEmpty(ordLastName) ? string.Empty : ordLastName.Length <= 35 ? ordLastName.Trim() : ordLastName.Substring(0, 35).Trim();
                        nm1ORD.NM104_UMOFirstName = ordFirstName.Trim();
                    }
                    else
                    {
                        nm1ORD.NM102_EntityTypeQualifier = "2";
                        nm1ORD.NM103_UMOLastOrOrganizationName = string.IsNullOrEmpty(ordLastName) ? string.Empty : ordLastName.Length <= 35 ? ordLastName.Trim() : ordLastName.Substring(0, 35).Trim();
                    }

                    //run query to retrieve address
                    N3Type nm3ORD = new N3Type();
                    nm3ORD.N301_RequesterAddressLine = addrORD1;

                    if (!string.IsNullOrEmpty(addrORD2.Trim()) && !string.IsNullOrWhiteSpace(addrORD2.Trim()))
                    {
                        nm3ORD.N302_RequesterAddressLine = addrORD2;
                    }

                    N4Type nm4ORD = new N4Type();
                    nm4ORD.N401_RequesterCityName = cityORD;
                    nm4ORD.N402_RequesterStateOrProvinceCode = stateORD;
                    nm4ORD.N403_RequesterPostalZoneOrZipCode = zipORD;

                    PERType perTORD = new PERType();
                    perTORD.PER01_ContactFunctionCode = "IC";


                    if (!string.IsNullOrEmpty(cnameORD) && !string.IsNullOrWhiteSpace(cnameORD))
                    {
                        perTORD.PER02_RequesterContactName = cnameORD;
                    }
                    perTORD.PER03_CommunicationNumberQualifier = "TE";
                    perTORD.PER04_RequesterContactCommunicationNumber = phoneORD;

                    if (!string.IsNullOrEmpty(emailORD) && !string.IsNullOrWhiteSpace(emailORD))
                    {
                        perTORD.PER05_CommunicationNumberQualifier = "EM";
                        perTORD.PER06_RequesterContactCommunicationNumber = emailORD.Trim();
                    }
                    if (!string.IsNullOrEmpty(faxORD) && !string.IsNullOrWhiteSpace(faxORD))
                    {
                        perTORD.PER07_CommunicationNumberQualifier = "FX";
                        perTORD.PER08_RequesterContactCommunicationNumber = faxORD;
                    }

                    PRVType pr1ORD = new PRVType();
                    pr1ORD.PRV01_ProviderCode = "OR";
                    pr1ORD.PRV02_ReferenceIdentificationQualifier = "PXC";
                    pr1ORD.PRV03_ProviderTaxonomyCode = taxonomyORD; //taxonomy value

                    provD[1] = new ProviderDetails_2010EAType();
                    provD[1].PatientEventProviderName_2010EA = nm1ORD; //used
                    provD[1].PatientEventProviderAddress_2010EA = nm3ORD; //used
                    provD[1].PatientEventProviderCityStateZipCode_2010EA = nm4ORD; //used
                    provD[1].PatientEventProviderContactInformation_2010EA = perTORD; //used
                    provD[1].PatientEventProviderInformation_2010EA = pr1ORD; //used

                }
                #endregion

                res.ProviderDetails_2010EA = provD;
            }
            catch (Exception ex)
            {
                CreateAndReturnLogThreadNumber(ex, "PriorAuthService-UpdateProviderDetails2010EA");
            }
        }

        private static void UpdateMessageText2000E(DataSet ds, string assignType, PatientEventDetails_2000EType res)
        {
            try
            {
                MSGType msg = new MSGType();
                DataTable dtProvNotes = ds.Tables["PriorAuthProviderNotes"];
                string provNotes = string.Empty;
                if (dtProvNotes != null && dtProvNotes.Rows.Count > 0)
                {
                    provNotes = dtProvNotes.Rows[0][0].ToString();
                }
                msg.MSG01_FreeFormMessageText = assignType + provNotes;
                res.MessageText_2000E = msg;
            }
            catch (Exception ex)
            {
                CreateAndReturnLogThreadNumber(ex, "PriorAuthService-UpdateMessageText2000E");
            }
        }

        private static void UpdateAdditionalPatientInformation2000E(DataTable attachDT, PatientEventDetails_2000EType res)
        {
            try
            {
                int countAttch = 0;
                if (attachDT.Rows.Count > 0)
                {
                    countAttch = attachDT.Rows.Count;
                    PWKType[] pwk = new PWKType[countAttch];

                    for (int i = 0; i < countAttch; i++)
                    {
                        pwk[i] = new PWKType();
                        pwk[i].PWK01_AttachmentReportTypeCode = "77";
                        pwk[i].PWK02_ReportTransmissionCode = "EL";
                        pwk[i].PWK05_IdentificationCodeQualifier = "AC";
                        pwk[i].PWK06_AttachmentControlNumber = attachDT.Rows[i]["DOCUMENT_ID"].ToString();
                        pwk[i].PWK07_AttachmentDescription = attachDT.Rows[i]["PRIOR_AUTH_SUB_Note"].ToString();
                    }
                    res.AdditionalPatientInformation_2000E = pwk;
                }
            }
            catch (Exception ex)
            {
                CreateAndReturnLogThreadNumber(ex, "PriorAuthService-UpdateAdditionalPatientInformation2000E");
            }
        }

        private static void UpdatePatientEventTrackingNumber2000E(string PatientTrackingNumber, string taxID, PatientEventDetails_2000EType res)
        {
            try
            {
                TRNType[] trn = new TRNType[1];
                trn[0] = new TRNType();

                trn[0].TRN01_TraceTypeCode = "1";
                trn[0].TRN02_PatientEventTraceNumber = PatientTrackingNumber;

                if (!string.IsNullOrEmpty(PatientTrackingNumber) && !string.IsNullOrWhiteSpace(PatientTrackingNumber))
                {
                    trn[0].TRN03_TraceAssigningEntityIdentifier = "1" + taxID;
                }

                res.PatientEventTrackingNumber_2000E = trn;
            }
            catch (Exception ex)
            {
                CreateAndReturnLogThreadNumber(ex, "PriorAuthService-UpdatePatientEventTrackingNumber2000E");
            }
        }

        private static void UpdateHealthCareServicesReviewInformation2000E(string paType, string serviceTracking, string placeOfService, string facType,
                                                                            string scCode, string lvlSvc, string delayRsn, string accDate, PatientEventDetails_2000EType res, string reqType)
        {
            try
            {
                UMType um = new UMType();

                switch (paType.ToUpper())
                {
                    case "DENTAL":
                        um.UM01_RequestCategoryCode = "HS";
                        um.UM041_FacilityTypeCode = placeOfService;
                        um.UM042_FacilityCodeQualifier = "B";
                        break;
                    case "PROFESSIONAL":
                        um.UM01_RequestCategoryCode = "HS";
                        um.UM041_FacilityTypeCode = placeOfService;
                        um.UM042_FacilityCodeQualifier = "B";
                        break;
                    case "INSTITUTIONAL":
                        um.UM01_RequestCategoryCode = "AR";
                        um.UM041_FacilityTypeCode = facType;
                        um.UM042_FacilityCodeQualifier = "A";


                        break;
                }

                um.UM02_CertificationTypeCode = reqType;
                um.UM03_ServiceTypeCode = scCode.TrimStart('0');
                um.UM04_HealthCareServiceLocationInformation = "Place of Service";

                if (!string.IsNullOrEmpty(lvlSvc) && !string.IsNullOrWhiteSpace(lvlSvc) && !lvlSvc.Equals("-1"))
                {
                    if (lvlSvc.Equals("0"))
                    {
                        um.UM06_LevelOfServiceCode = "E";
                    }
                    else
                    {
                        um.UM06_LevelOfServiceCode = "U";
                    }

                }

                if (!string.IsNullOrEmpty(delayRsn) && !string.IsNullOrWhiteSpace(delayRsn) && !delayRsn.Equals("-1"))
                {
                    um.UM10_DelayReasonCode = delayRsn;
                }

                DateTime accDateField;
                if (DateTime.TryParse(accDate, out accDateField))
                {
                    um.UM051_RelatedCausesCode = "AA";
                }
                res.HealthCareServicesReviewInformation_2000E = um;
            }
            catch (Exception ex)
            {
                CreateAndReturnLogThreadNumber(ex, "PriorAuthService-UpdateHealthCareServicesReviewInformation2000E");
            }

        }



        private static void UpdatePreviousReviewAuthorizationNumber2000E(string associatePANum, string paNumber, string reqType, PatientEventDetails_2000EType res)
        {
            try
            {
                REFType reff = new REFType(); //part of 2000E
                if (!string.IsNullOrEmpty(associatePANum) && !string.IsNullOrWhiteSpace(associatePANum))
                {
                    reff.REF01_ReferenceIdentificationQualifier = "BB"; //Constant Qualifier
                    reff.REF02_RequesterSupplementalIdentification = associatePANum; //Associate Number.
                }

                if (reqType == "S" || reqType == "3")//Update and Cancel.
                {
                    reff.REF02_RequesterSupplementalIdentification = paNumber;
                }

                res.PreviousReviewAuthorizationNumber_2000E = reff;
            }
            catch (Exception ex)
            {
                CreateAndReturnLogThreadNumber(ex, "PriorAuthService-UpdatePreviousReviewAuthorizationNumber2000E");
            }
        }



        private static void UpdatePatientEventDetails2000E(string accDate, string mensDate, string dobDate, string illDate, string ptnEventDate,
                                                           string admDate, string disDate, PatientEventDetails_2000EType res)
        {
            try
            {
                //accident date
                DTPType dtt = new DTPType();
                DateTime accDateField;
                if (DateTime.TryParse(accDate, out accDateField))
                {

                    dtt.DTP01_DateTimeQualifier = "439";
                    dtt.DTP02_DateTimePeriodFormatQualifier = "D8";
                    dtt.DTP03_AccidentDate = accDateField.ToString("yyyyMMdd");
                }

                //menstrual date
                DTPType dtt2 = new DTPType();
                DateTime mensDateField;
                if (DateTime.TryParse(mensDate, out mensDateField))
                {
                    dtt2.DTP01_DateTimeQualifier = "484";
                    dtt2.DTP02_DateTimePeriodFormatQualifier = "D8";
                    dtt2.DTP03_AccidentDate = mensDateField.ToString("yyyyMMdd");
                }

                //dob date
                DTPType dtt3 = new DTPType();
                DateTime dobDateField;
                if (DateTime.TryParse(dobDate, out dobDateField))
                {
                    dtt3.DTP01_DateTimeQualifier = "ABC";
                    dtt3.DTP02_DateTimePeriodFormatQualifier = "D8";
                    dtt3.DTP03_AccidentDate = dobDateField.ToString("yyyyMMdd");
                }

                //onset date
                DTPType dtt4 = new DTPType();
                DateTime illDateField;
                if (DateTime.TryParse(illDate, out illDateField))
                {
                    dtt4.DTP01_DateTimeQualifier = "431";
                    dtt4.DTP02_DateTimePeriodFormatQualifier = "D8";
                    dtt4.DTP03_AccidentDate = illDateField.ToString("yyyyMMdd");
                }

                //event date
                DTPType dtt5 = new DTPType();
                DateTime ptnEventDateField;
                if (DateTime.TryParse(ptnEventDate, out ptnEventDateField))
                {
                    dtt5.DTP01_DateTimeQualifier = "AAH";
                    dtt5.DTP02_DateTimePeriodFormatQualifier = "D8";
                    dtt5.DTP03_AccidentDate = ptnEventDateField.ToString("yyyyMMdd");
                }

                //admission date
                DTPType dtt6 = new DTPType();
                DateTime admDateField;
                if (DateTime.TryParse(admDate, out admDateField))
                {
                    dtt6.DTP01_DateTimeQualifier = "435";
                    dtt6.DTP02_DateTimePeriodFormatQualifier = "D8";
                    dtt6.DTP03_AccidentDate = admDateField.ToString("yyyyMMdd");
                }

                //discharge date
                DTPType dtt7 = new DTPType();
                DateTime disDateField;
                if (DateTime.TryParse(disDate, out disDateField))
                {
                    dtt7.DTP01_DateTimeQualifier = "096";
                    dtt7.DTP02_DateTimePeriodFormatQualifier = "D8";
                    dtt7.DTP03_AccidentDate = disDateField.ToString("yyyyMMdd");
                }

                res.AccidentDate_2000E = dtt;
                res.LastMenstrualPeriodDate_2000E = dtt2;
                res.EstimatedDateOfBirth_2000E = dtt3;
                res.OnsetOfCurrentSymptomsOrIllnessDate_2000E = dtt4;
                res.EventDate_2000E = dtt5;
                res.AdmissionDate_2000E = dtt6;
                res.DischargeDate_2000E = dtt7;
            }
            catch (Exception ex)
            {
                CreateAndReturnLogThreadNumber(ex, "PriorAuthService-UpdatePatientEventDetails2000E");
            }
        }

        private static void UpdatePatientDiagnosis2000E(DataTable diagDT, PatientEventDetails_2000EType res)
        {
            try
            {
                HIType hii = new HIType();
                int countDiag = 0;
                // diagnosis panel gizmo
                if (diagDT.Rows.Count > 0)
                {

                    countDiag = diagDT.Rows.Count;
                    string diagCodeType = string.Empty;
                    string diagCode = string.Empty;
                    string diagDate = string.Empty;

                    if (new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }.Contains(countDiag))
                    {
                        diagCodeType = getDiagnosisCode(diagDT.Rows[0]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString());
                        diagCode = diagDT.Rows[0]["PRIOR_AUTH_DIAGNOSIS_CODE"].ToString();
                        diagDate = diagDT.Rows[0]["PRIOR_AUTH_DIAGNOSIS_DATE"].ToString();
                        hii.HI011_DiagnosisTypeCode = diagCodeType;// use sql query linq to get this "ABK";
                        hii.HI012_DiagnosisCode = diagCode;

                        DateTime dtdiagDate;
                        if (DateTime.TryParse(diagDate, out dtdiagDate))
                        {
                            hii.HI013_DateTimePeriodFormatQualifier = "D8";
                            hii.HI014_DiagnosisDate = dtdiagDate.ToString("yyyyMMdd");
                        }
                    }
                    if (new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }.Contains(countDiag))
                    {
                        diagCodeType = getDiagnosisCode(diagDT.Rows[1]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString());
                        diagCode = diagDT.Rows[1]["PRIOR_AUTH_DIAGNOSIS_CODE"].ToString();
                        diagDate = diagDT.Rows[1]["PRIOR_AUTH_DIAGNOSIS_DATE"].ToString();
                        hii.HI021_DiagnosisTypeCode = diagCodeType;// use sql query linq to get this "ABK";
                        hii.HI022_DiagnosisCode = diagCode;

                        DateTime dtdiagDate;
                        if (DateTime.TryParse(diagDate, out dtdiagDate))
                        {
                            hii.HI023_DateTimePeriodFormatQualifier = "D8";
                            hii.HI024_DiagnosisDate = dtdiagDate.ToString("yyyyMMdd");
                        }
                    }
                    if (new[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }.Contains(countDiag))
                    {
                        diagCodeType = getDiagnosisCode(diagDT.Rows[2]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString());
                        diagCode = diagDT.Rows[2]["PRIOR_AUTH_DIAGNOSIS_CODE"].ToString();
                        diagDate = diagDT.Rows[2]["PRIOR_AUTH_DIAGNOSIS_DATE"].ToString();
                        hii.HI031_DiagnosisTypeCode = diagCodeType;// use sql query linq to get this "ABK";
                        hii.HI032_DiagnosisCode = diagCode;

                        DateTime dtdiagDate;
                        if (DateTime.TryParse(diagDate, out dtdiagDate))
                        {
                            hii.HI033_DateTimePeriodFormatQualifier = "D8";
                            hii.HI034_DiagnosisDate = dtdiagDate.ToString("yyyyMMdd");
                        }
                    }
                    if (new[] { 4, 5, 6, 7, 8, 9, 10, 11, 12 }.Contains(countDiag))
                    {
                        diagCodeType = getDiagnosisCode(diagDT.Rows[3]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString());
                        diagCode = diagDT.Rows[3]["PRIOR_AUTH_DIAGNOSIS_CODE"].ToString();
                        diagDate = diagDT.Rows[3]["PRIOR_AUTH_DIAGNOSIS_DATE"].ToString();
                        hii.HI041_DiagnosisTypeCode = diagCodeType;// use sql query linq to get this "ABK";
                        hii.HI042_DiagnosisCode = diagCode;

                        DateTime dtdiagDate;
                        if (DateTime.TryParse(diagDate, out dtdiagDate))
                        {
                            hii.HI043_DateTimePeriodFormatQualifier = "D8";
                            hii.HI044_DiagnosisDate = dtdiagDate.ToString("yyyyMMdd");
                        }
                    }
                    if (new[] { 5, 6, 7, 8, 9, 10, 11, 12 }.Contains(countDiag))
                    {
                        diagCodeType = getDiagnosisCode(diagDT.Rows[4]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString());
                        diagCode = diagDT.Rows[4]["PRIOR_AUTH_DIAGNOSIS_CODE"].ToString();
                        diagDate = diagDT.Rows[4]["PRIOR_AUTH_DIAGNOSIS_DATE"].ToString();
                        hii.HI051_DiagnosisTypeCode = diagCodeType;// use sql query linq to get this "ABK";
                        hii.HI052_DiagnosisCode = diagCode;

                        DateTime dtdiagDate;
                        if (DateTime.TryParse(diagDate, out dtdiagDate))
                        {
                            hii.HI053_DateTimePeriodFormatQualifier = "D8";
                            hii.HI054_DiagnosisDate = dtdiagDate.ToString("yyyyMMdd");
                        }
                    }
                    if (new[] { 6, 7, 8, 9, 10, 11, 12 }.Contains(countDiag))
                    {
                        diagCodeType = getDiagnosisCode(diagDT.Rows[5]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString());
                        diagCode = diagDT.Rows[5]["PRIOR_AUTH_DIAGNOSIS_CODE"].ToString();
                        diagDate = diagDT.Rows[5]["PRIOR_AUTH_DIAGNOSIS_DATE"].ToString();
                        hii.HI061_DiagnosisTypeCode = diagCodeType;// use sql query linq to get this "ABK";
                        hii.HI062_DiagnosisCode = diagCode;

                        DateTime dtdiagDate;
                        if (DateTime.TryParse(diagDate, out dtdiagDate))
                        {
                            hii.HI063_DateTimePeriodFormatQualifier = "D8";
                            hii.HI064_DiagnosisDate = dtdiagDate.ToString("yyyyMMdd");
                        }
                    }
                    if (new[] { 7, 8, 9, 10, 11, 12 }.Contains(countDiag))
                    {
                        diagCodeType = getDiagnosisCode(diagDT.Rows[6]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString());
                        diagCode = diagDT.Rows[6]["PRIOR_AUTH_DIAGNOSIS_CODE"].ToString();
                        diagDate = diagDT.Rows[6]["PRIOR_AUTH_DIAGNOSIS_DATE"].ToString();
                        hii.HI071_DiagnosisTypeCode = diagCodeType;// use sql query linq to get this "ABK";
                        hii.HI072_DiagnosisCode = diagCode;

                        DateTime dtdiagDate;
                        if (DateTime.TryParse(diagDate, out dtdiagDate))
                        {
                            hii.HI073_DateTimePeriodFormatQualifier = "D8";
                            hii.HI074_DiagnosisDate = dtdiagDate.ToString("yyyyMMdd");
                        }
                    }
                    if (new[] { 8, 9, 10, 11, 12 }.Contains(countDiag))
                    {
                        diagCodeType = getDiagnosisCode(diagDT.Rows[7]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString());
                        diagCode = diagDT.Rows[7]["PRIOR_AUTH_DIAGNOSIS_CODE"].ToString();
                        diagDate = diagDT.Rows[7]["PRIOR_AUTH_DIAGNOSIS_DATE"].ToString();
                        hii.HI081_DiagnosisTypeCode = diagCodeType;// use sql query linq to get this "ABK";
                        hii.HI082_DiagnosisCode = diagCode;

                        DateTime dtdiagDate;
                        if (DateTime.TryParse(diagDate, out dtdiagDate))
                        {
                            hii.HI083_DateTimePeriodFormatQualifier = "D8";
                            hii.HI084_DiagnosisDate = dtdiagDate.ToString("yyyyMMdd");
                        }
                    }
                    if (new[] { 9, 10, 11, 12 }.Contains(countDiag))
                    {
                        diagCodeType = getDiagnosisCode(diagDT.Rows[8]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString());
                        diagCode = diagDT.Rows[8]["PRIOR_AUTH_DIAGNOSIS_CODE"].ToString();
                        diagDate = diagDT.Rows[8]["PRIOR_AUTH_DIAGNOSIS_DATE"].ToString();
                        hii.HI091_DiagnosisTypeCode = diagCodeType;// use sql query linq to get this "ABK";
                        hii.HI092_DiagnosisCode = diagCode;

                        DateTime dtdiagDate;
                        if (DateTime.TryParse(diagDate, out dtdiagDate))
                        {
                            hii.HI093_DateTimePeriodFormatQualifier = "D8";
                            hii.HI094_DiagnosisDate = dtdiagDate.ToString("yyyyMMdd");
                        }
                    }
                    if (new[] { 10, 11, 12 }.Contains(countDiag))
                    {
                        diagCodeType = getDiagnosisCode(diagDT.Rows[9]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString());
                        diagCode = diagDT.Rows[9]["PRIOR_AUTH_DIAGNOSIS_CODE"].ToString();
                        diagDate = diagDT.Rows[9]["PRIOR_AUTH_DIAGNOSIS_DATE"].ToString();
                        hii.HI101_DiagnosisTypeCode = diagCodeType;// use sql query linq to get this "ABK";
                        hii.HI102_DiagnosisCode = diagCode;

                        DateTime dtdiagDate;
                        if (DateTime.TryParse(diagDate, out dtdiagDate))
                        {
                            hii.HI103_DateTimePeriodFormatQualifier = "D8";
                            hii.HI104_DiagnosisDate = dtdiagDate.ToString("yyyyMMdd");
                        }
                    }
                    if (new[] { 11, 12 }.Contains(countDiag))
                    {
                        diagCodeType = getDiagnosisCode(diagDT.Rows[10]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString());
                        diagCode = diagDT.Rows[10]["PRIOR_AUTH_DIAGNOSIS_CODE"].ToString();
                        diagDate = diagDT.Rows[10]["PRIOR_AUTH_DIAGNOSIS_DATE"].ToString();
                        hii.HI111_DiagnosisTypeCode = diagCodeType;// use sql query linq to get this "ABK";
                        hii.HI112_DiagnosisCode = diagCode;

                        DateTime dtdiagDate;
                        if (DateTime.TryParse(diagDate, out dtdiagDate))
                        {
                            hii.HI113_DateTimePeriodFormatQualifier = "D8";
                            hii.HI114_DiagnosisDate = dtdiagDate.ToString("yyyyMMdd");
                        }
                    }
                    if (new[] { 12 }.Contains(countDiag))
                    {
                        diagCodeType = getDiagnosisCode(diagDT.Rows[11]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString());
                        diagCode = diagDT.Rows[11]["PRIOR_AUTH_DIAGNOSIS_CODE"].ToString();
                        diagDate = diagDT.Rows[11]["PRIOR_AUTH_DIAGNOSIS_DATE"].ToString();
                        hii.HI121_DiagnosisTypeCode = diagCodeType;// use sql query linq to get this "ABK";
                        hii.HI122_DiagnosisCode = diagCode;

                        DateTime dtdiagDate;
                        if (DateTime.TryParse(diagDate, out dtdiagDate))
                        {
                            hii.HI123_DateTimePeriodFormatQualifier = "D8";
                            hii.HI124_DiagnosisDate = dtdiagDate.ToString("yyyyMMdd");
                        }
                    }

                    res.PatientDiagnosis_2000E = hii;
                }
            }
            catch (Exception ex)
            {
                CreateAndReturnLogThreadNumber(ex, "PriorAuthService-UpdatePatientDiagnosis2000E");
            }
        }

        private static void UpdateInstitutionalClaimCode2000E(string paType, string admType, string admSrc, string discStat, PatientEventDetails_2000EType res)
        {
            try
            {
                CL1Type cl1 = new CL1Type();

                string serviceTracking = string.Empty;

                switch (paType.ToUpper())
                {
                    case "INSTITUTIONAL":
                        //Adding more conditions to validate the fields 
                        if (!string.IsNullOrEmpty(admType) && !string.IsNullOrWhiteSpace(admType) && !admType.Equals("-1"))
                        {
                            cl1.CL101_AdmissionTypeCode = admType;
                        }

                        if (!string.IsNullOrEmpty(admSrc) && !string.IsNullOrWhiteSpace(admSrc) && !admSrc.Equals("-1"))
                        {
                            cl1.CL102_AdmissionSourceCode = admSrc;
                        }

                        if (!string.IsNullOrEmpty(discStat) && !string.IsNullOrWhiteSpace(discStat) && !discStat.Equals("-1"))
                        {
                            cl1.CL103_PatientStatusCode = discStat;
                        }
                        break;
                    default:
                        break;
                }
                res.InstitutionalClaimCode_2000E = cl1;
            }
            catch (Exception ex)
            {
                CreateAndReturnLogThreadNumber(ex, "PriorAuthService-UpdateInstitutionalClaimCode2000E");
            }
        }
        private static string getDiagnosisCode(string str)
        {
            string strr = string.Empty;
            switch (str)
            {
                case "1":
                    strr = "ABK";
                    break;
                case "2":
                    strr = "ABJ";
                    break;
                case "3":
                    strr = "APR";
                    break;
                case "4":
                    strr = "ABF";
                    break;
            }
            return strr;
        }

        public static ServiceDetails_2000FType fillServiceDetails_2000FType(DataSet ds, int serviceLineRows)
        {
            string PatientTrackingNumber = string.Empty;
            string taxID = string.Empty;
            string paType = string.Empty;
            string assType = string.Empty;
            string provNotes = string.Empty;


            DataTable dt = new DataTable();
            if (PriorAuthServiceHelper.HasRows(ds))
            {
                if (ds.Tables.Contains("PriorAuthHeader"))
                {
                    dt = ds.Tables["PriorAuthHeader"];
                    taxID = dt.Rows[0]["TAX_ID"].ToString();
                    paType = dt.Rows[0]["PriorAuthType"].ToString();
                    assType = dt.Rows[0]["PriorAuthAssignment"].ToString();
                    provNotes = dt.Rows[0]["PriorAuthProviderNotes"].ToString();
                }

                if (ds.Tables.Contains("PriorAuthRecipientInfo"))
                {
                    dt = ds.Tables["PriorAuthRecipientInfo"];
                    PatientTrackingNumber = dt.Rows[0]["PatientTrackingNumber"].ToString();

                }
                if (ds.Tables.Contains("PriorAuthServiceDetails"))
                {
                    dt = ds.Tables["PriorAuthServiceDetails"];
                }
            }

            HLType hlt = new HLType();
            hlt.HL01_HeirarchicalIdNumber = "5";
            hlt.HL02_HeirarchicalParentIdNumber = "4";
            hlt.HL03_HeirarchicalLevelCode = "SS";
            hlt.HL04_HerarchicalChildCode = "0";

            TRNType[] trn = new TRNType[1];
            trn[0] = new TRNType();

            DTPType dtp = new DTPType();
            dtp.DTP01_DateTimeQualifier = "472";
            dtp.DTP02_DateTimePeriodFormatQualifier = "RD8";

            MSGType msg = new MSGType();


            ServiceDetails_2000FType req2000 = new ServiceDetails_2000FType();
            req2000.ServiceLevel_2000F = hlt;



            switch (paType.ToUpper())
            {
                case "DENTAL":
                    //datatable dental
                    string fdosDental = string.Empty;
                    string tdosDental = string.Empty;

                    string toothNum = string.Empty;

                    string tooth1 = string.Empty;
                    string tooth2 = string.Empty;
                    string tooth3 = string.Empty;
                    string tooth4 = string.Empty;
                    string tooth5 = string.Empty;

                    string procID = string.Empty;
                    string procDescp = string.Empty;
                    string reqDollar = string.Empty;
                    string oralCav1 = string.Empty;
                    string oralCav2 = string.Empty;
                    string oralCav3 = string.Empty;
                    string oralCav4 = string.Empty;
                    string oralCav5 = string.Empty;
                    string reqUnits = string.Empty;
                    string prosCrown = string.Empty;
                    string svcDescp = string.Empty;
                    string serviceTracking = string.Empty;

                    try
                    {
                        fdosDental = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"].ToString();
                        tdosDental = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"].ToString();

                        procID = dt.Rows[serviceLineRows]["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString();
                        procDescp = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC"].ToString();
                        reqDollar = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR"].ToString();
                        oralCav1 = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS"].ToString();
                        oralCav2 = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS"].ToString();
                        oralCav3 = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS"].ToString();
                        oralCav4 = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS"].ToString();
                        oralCav5 = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS"].ToString();
                        reqUnits = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS"].ToString();
                        prosCrown = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID"].ToString();
                        svcDescp = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"].ToString();

                        toothNum = dt.Rows[serviceLineRows]["PRIOR_AUTH_TOOTH_NUMBER_ID"].ToString();
                        //toothNum = dt.Rows[serviceLineRows]["PRIOR_AUTH_TOOTH_NUMBER_CODE"].ToString();
                        tooth1 = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE"].ToString();
                        tooth2 = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE"].ToString();
                        tooth3 = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE"].ToString();
                        tooth4 = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE"].ToString();
                        tooth5 = dt.Rows[serviceLineRows]["PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE"].ToString();
                    }
                    catch (Exception ex) { }

                    DateTime dtfdosDental;
                    if (DateTime.TryParse(fdosDental, out dtfdosDental)) { }
                    DateTime dttdosDental;
                    if (DateTime.TryParse(tdosDental, out dttdosDental)) { }

                    if (string.IsNullOrEmpty(fdosDental) && string.IsNullOrEmpty(tdosDental) || (fdosDental.Contains("1/1/0001") && tdosDental.Contains("1/1/0001")))
                    {
                        dtp.DTP03_AccidentDate = string.Empty;
                        dtp.DTP01_DateTimeQualifier = string.Empty;
                        dtp.DTP02_DateTimePeriodFormatQualifier = string.Empty;
                    }
                    else
                    {
                        dtp.DTP03_AccidentDate = dtfdosDental.ToString("yyyyMMdd") + "-" + dttdosDental.ToString("yyyyMMdd");
                    }

                    SV3Type s33 = new SV3Type();
                    s33.SV3011_ProductOrServiceIdQualifier = "AD";

                    string strprocIDTrail = procID.Trim();
                    s33.SV3012_ProcedureCode = strprocIDTrail;

                    if (!string.IsNullOrEmpty(procDescp) && !string.IsNullOrWhiteSpace(procDescp))
                    {
                        s33.SV307_Description = procDescp;
                    }

                    s33.SV302_LineItemChargeAmount = reqDollar;

                    if (!string.IsNullOrEmpty(oralCav1) && !string.IsNullOrWhiteSpace(oralCav1) && !oralCav1.Equals("0"))
                    {
                        s33.SV3041_OralCavityDesignationCode = oralCav1;
                    }
                    if (!string.IsNullOrEmpty(oralCav2) && !string.IsNullOrWhiteSpace(oralCav2) && !oralCav2.Equals("0"))
                    {
                        s33.SV3042_OralCavityDesignationCode = oralCav2;
                    }
                    if (!string.IsNullOrEmpty(oralCav3) && !string.IsNullOrWhiteSpace(oralCav3) && !oralCav3.Equals("0"))
                    {
                        s33.SV3043_OralCavityDesignationCode = oralCav3;
                    }
                    if (!string.IsNullOrEmpty(oralCav4) && !string.IsNullOrWhiteSpace(oralCav4) && !oralCav4.Equals("0"))
                    {
                        s33.SV3044_OralCavityDesignationCode = oralCav4;
                    }
                    if (!string.IsNullOrEmpty(oralCav5) && !string.IsNullOrWhiteSpace(oralCav5) && !oralCav5.Equals("0"))
                    {
                        s33.SV3045_OralCavityDesignationCode = oralCav5;
                    }

                    if (prosCrown.Equals("3"))
                    {
                        s33.SV305_ProthesisCrownOrInlayCode = "I";
                    }
                    else if (prosCrown.Equals("4"))
                    {
                        s33.SV305_ProthesisCrownOrInlayCode = "R";
                    }

                    s33.SV306_ServiceUnitCount = reqUnits;

                    if (!string.IsNullOrEmpty(svcDescp) && !string.IsNullOrWhiteSpace(svcDescp))
                    {
                        msg.MSG01_FreeFormMessageText = svcDescp;
                        //s33.SV307_Description = svcDescp;
                    }

                    req2000.DentalService_2000F = s33;

                    TOOType[] too = new TOOType[1];
                    too[0] = new TOOType();

                    if (!string.IsNullOrEmpty(toothNum) && !string.IsNullOrWhiteSpace(toothNum) && !toothNum.Equals("0"))
                    {
                        too[0].TOO01_CodeListQualifierCode = "JP";
                        too[0].TOO02_ToothCode = toothNum;
                    }
                    if (!string.IsNullOrEmpty(tooth1) && !string.IsNullOrWhiteSpace(tooth1) && !tooth1.Equals("0"))
                    {
                        too[0].TOO01_CodeListQualifierCode = "JP";
                        too[0].TOO031_ToothSurfaceCode = tooth1;
                    }
                    if (!string.IsNullOrEmpty(tooth2) && !string.IsNullOrWhiteSpace(tooth2) && !tooth2.Equals("0"))
                    {
                        too[0].TOO01_CodeListQualifierCode = "JP";
                        too[0].TOO032_ToothSurfaceCode = tooth2;
                    }
                    if (!string.IsNullOrEmpty(tooth3) && !string.IsNullOrWhiteSpace(tooth3) && !tooth3.Equals("0"))
                    {
                        too[0].TOO01_CodeListQualifierCode = "JP";
                        too[0].TOO033_ToothSurfaceCode = tooth3;
                    }
                    if (!string.IsNullOrEmpty(tooth4) && !string.IsNullOrWhiteSpace(tooth4) && !tooth4.Equals("0"))
                    {
                        too[0].TOO01_CodeListQualifierCode = "JP";
                        too[0].TOO034_ToothSurfaceCode = tooth4;
                    }
                    if (!string.IsNullOrEmpty(tooth5) && !string.IsNullOrWhiteSpace(tooth5) && !tooth5.Equals("0"))
                    {
                        too[0].TOO01_CodeListQualifierCode = "JP";
                        too[0].TOO035_ToothSurfaceCode = tooth5;
                    }

                    req2000.ToothInformation_2000F = too;
                    serviceTracking = dt.Rows[0]["PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM"].ToString();
                    if (!string.IsNullOrEmpty(serviceTracking) && !string.IsNullOrWhiteSpace(serviceTracking))
                    {
                        trn[0].TRN01_TraceTypeCode = "1";
                        trn[0].TRN03_TraceAssigningEntityIdentifier = "1" + taxID;
                        trn[0].TRN02_PatientEventTraceNumber = serviceTracking;
                    }

                    break;
                case "PROFESSIONAL":
                    //datatable professional
                    string fdosProff = string.Empty;
                    string tdosProff = string.Empty;

                    string mod1 = string.Empty;
                    string mod2 = string.Empty;
                    string mod3 = string.Empty;
                    string mod4 = string.Empty;

                    string procCodeProff = string.Empty;
                    string procCodeDescpProff = string.Empty;
                    string amtProff = string.Empty;
                    string mesCode = string.Empty;
                    string quant = string.Empty;
                    string svcDescpProf = string.Empty;

                    try
                    {
                        fdosProff = dt.Rows[serviceLineRows]["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"].ToString();
                        tdosProff = dt.Rows[serviceLineRows]["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"].ToString();

                        mod1 = dt.Rows[serviceLineRows]["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1"].ToString();
                        mod2 = dt.Rows[serviceLineRows]["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2"].ToString();
                        mod3 = dt.Rows[serviceLineRows]["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3"].ToString();
                        mod4 = dt.Rows[serviceLineRows]["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4"].ToString();

                        procCodeProff = dt.Rows[serviceLineRows]["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString();
                        procCodeDescpProff = dt.Rows[serviceLineRows]["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC"].ToString();
                        amtProff = dt.Rows[serviceLineRows]["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"].ToString();
                        mesCode = dt.Rows[serviceLineRows]["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"].ToString();
                        svcDescpProf = dt.Rows[serviceLineRows]["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"].ToString();
                        quant = dt.Rows[serviceLineRows]["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"].ToString();
                    }
                    catch (Exception ex) { }

                    DateTime dtfdosProff;
                    if (DateTime.TryParse(fdosProff, out dtfdosProff)) { }
                    DateTime dttdosProff;
                    if (DateTime.TryParse(tdosProff, out dttdosProff)) { }

                    if ((string.IsNullOrEmpty(fdosProff) && string.IsNullOrEmpty(tdosProff)) || (fdosProff.Contains("1/1/0001") && tdosProff.Contains("1/1/0001")))
                    {
                        dtp.DTP03_AccidentDate = string.Empty;
                        dtp.DTP01_DateTimeQualifier = string.Empty;
                        dtp.DTP02_DateTimePeriodFormatQualifier = string.Empty;
                    }
                    else
                    {
                        dtp.DTP03_AccidentDate = dtfdosProff.ToString("yyyyMMdd") + "-" + dttdosProff.ToString("yyyyMMdd");
                    }

                    SV1Type sv1 = new SV1Type();
                    sv1.SV1011_ProductOrServiceIdQualifier = "HC";
                    sv1.SV1012_ProcedureCode = procCodeProff;

                    if (!string.IsNullOrEmpty(mod1) && !string.IsNullOrWhiteSpace(mod1) && !mod1.Equals("0"))
                    {
                        sv1.SV1013_ProcedureModifier = mod1;
                    }
                    if (!string.IsNullOrEmpty(mod2) && !string.IsNullOrWhiteSpace(mod2) && !mod2.Equals("0"))
                    {
                        sv1.SV1014_ProcedureModifier = mod2;
                    }
                    if (!string.IsNullOrEmpty(mod3) && !string.IsNullOrWhiteSpace(mod3) && !mod3.Equals("0"))
                    {
                        sv1.SV1015_ProcedureModifier = mod3;
                    }
                    if (!string.IsNullOrEmpty(mod4) && !string.IsNullOrWhiteSpace(mod4) && !mod4.Equals("0"))
                    {
                        sv1.SV1016_ProcedureModifier = mod4;
                    }
                    if (!string.IsNullOrEmpty(procCodeDescpProff) && !string.IsNullOrWhiteSpace(procCodeDescpProff))
                    {
                        sv1.SV1017_ProcedureCodeDescription = procCodeDescpProff;
                    }

                    if (mesCode.Equals("1"))
                    {
                        sv1.SV103_UnitOrBasisForMeasurementCode = "F2";
                    }
                    else if (mesCode.Equals("2"))
                    {
                        sv1.SV103_UnitOrBasisForMeasurementCode = "MJ";
                    }
                    else
                    {
                        sv1.SV103_UnitOrBasisForMeasurementCode = "UN";
                    }
                    sv1.SV102_ServiceLineAmount = amtProff;
                    sv1.SV104_ServiceUnitCount = quant;

                    req2000.ProfessionalService_2000F = sv1;
                    serviceTracking = dt.Rows[0]["PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM"].ToString();
                    if (!string.IsNullOrEmpty(serviceTracking) && !string.IsNullOrWhiteSpace(serviceTracking))
                    {
                        trn[0].TRN01_TraceTypeCode = "1";
                        trn[0].TRN03_TraceAssigningEntityIdentifier = "1" + taxID;
                        trn[0].TRN02_PatientEventTraceNumber = serviceTracking;
                    }

                    if (!string.IsNullOrEmpty(svcDescpProf) && !string.IsNullOrWhiteSpace(svcDescpProf))
                    {
                        msg.MSG01_FreeFormMessageText = svcDescpProf;
                    }

                    break;
                case "INSTITUTIONAL":
                    //datatable institutional
                    string fdosInsti = string.Empty;
                    string tdosInsti = string.Empty;

                    string procSVCInst = string.Empty;
                    string procSVCQualInst = string.Empty;
                    string procRevInst = string.Empty;
                    string procSVCDescpInst = string.Empty;
                    string reqDollInst = string.Empty;
                    string requnitCodeInst = string.Empty;
                    string reqUnitsInst = string.Empty;
                    string lvlCareInst = string.Empty;
                    string procSVCProcedureCode = string.Empty;

                    string svcDescpInst = string.Empty;

                    try
                    {
                        fdosInsti = dt.Rows[serviceLineRows]["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"].ToString();
                        tdosInsti = dt.Rows[serviceLineRows]["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"].ToString();
                        procSVCInst = dt.Rows[serviceLineRows]["PRIOR_AUTH_SERVICE_REVENUE_CODE"].ToString();
                        procSVCProcedureCode = dt.Rows[serviceLineRows]["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"].ToString().Trim();
                        procSVCQualInst = dt.Rows[serviceLineRows]["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"].ToString();
                        procSVCDescpInst = dt.Rows[serviceLineRows]["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"].ToString();
                        reqDollInst = dt.Rows[serviceLineRows]["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"].ToString();
                        requnitCodeInst = dt.Rows[serviceLineRows]["PRIOR_AUTH_REQUESTED_UNITS_ID"].ToString();
                        reqUnitsInst = dt.Rows[serviceLineRows]["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"].ToString();
                        lvlCareInst = dt.Rows[serviceLineRows]["PRIOR_AUTH_LEVEL_CARE_ID"].ToString();
                        svcDescpInst = dt.Rows[serviceLineRows]["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"].ToString();
                    }
                    catch (Exception ex) { }

                    DateTime dtfdosInsti;
                    if (DateTime.TryParse(fdosInsti, out dtfdosInsti)) { }
                    DateTime dttdosInsti;
                    if (DateTime.TryParse(tdosInsti, out dttdosInsti)) { }

                    if (string.IsNullOrEmpty(fdosInsti) && string.IsNullOrEmpty(tdosInsti) || (fdosInsti.Contains("1/1/0001") && tdosInsti.Contains("1/1/0001")))
                    {
                        dtp.DTP03_AccidentDate = string.Empty;
                        dtp.DTP01_DateTimeQualifier = string.Empty;
                        dtp.DTP02_DateTimePeriodFormatQualifier = string.Empty;
                    }
                    else
                    {
                        dtp.DTP03_AccidentDate = dtfdosInsti.ToString("yyyyMMdd") + "-" + dttdosInsti.ToString("yyyyMMdd");
                    }

                    SV2Type sv2 = new SV2Type();

                    //sv2-01 - Revenue Code
                    if (!string.IsNullOrEmpty(procSVCInst))
                    {
                        sv2.SV201_ServiceLineRevenueCode = procSVCInst;
                    }
                    else
                    {
                        sv2.SV201_ServiceLineRevenueCode = string.Empty;
                    }

                    //sv2-02-1 - Procedure code type field

                    switch (procSVCQualInst)
                    {
                        case "3":
                            sv2.SV2021_ProductOrServiceIdQualifier = "HC";
                            break;

                        case "4":
                            sv2.SV2021_ProductOrServiceIdQualifier = "ZZ";
                            break;

                        default:
                            sv2.SV2021_ProductOrServiceIdQualifier = string.Empty;
                            break;
                    }

                    //sv2-02-2 - procedure code field
                    sv2.SV2022_ProcedureCode = procSVCProcedureCode.Trim();

                    //SV2-02-7 Procedure Code Description
                    if (!string.IsNullOrEmpty(procSVCDescpInst))
                    {
                        sv2.SV2027_ProcedureCodeDescription = procSVCDescpInst;
                    }
                    else
                    {
                        sv2.SV2027_ProcedureCodeDescription = string.Empty;
                    }

                    //sv2-03 requested dollars
                    if (!string.IsNullOrEmpty(reqDollInst) && reqDollInst != "0.00" && reqDollInst != "0")
                        sv2.SV203_ServiceLineAmount = reqDollInst;
                    else
                        sv2.SV203_ServiceLineAmount = string.Empty;

                    //sv2-04 - Requested Units Type/Unit of measure
                    if (!string.IsNullOrEmpty(requnitCodeInst))
                    {
                        if (requnitCodeInst == "4" || requnitCodeInst == "UN-Unit")
                            sv2.SV204_UnitOrBasisForMeasurementCode = "UN";
                        else if (requnitCodeInst == "2" || requnitCodeInst == "DA-Days")
                            sv2.SV204_UnitOrBasisForMeasurementCode = "DA";
                        else if (requnitCodeInst == "2" || requnitCodeInst == "MJ-Minute")
                            sv2.SV204_UnitOrBasisForMeasurementCode = "MJ";
                        else if (requnitCodeInst == "1" || requnitCodeInst == "F2-International Unit")
                            sv2.SV204_UnitOrBasisForMeasurementCode = "F2";
                    }
                    else
                    {
                        sv2.SV204_UnitOrBasisForMeasurementCode = string.Empty;
                    }

                    //sv2-05 Quantity/Requested units
                    if (!string.IsNullOrEmpty(reqUnitsInst))
                        sv2.SV205_ServiceUnitCount = reqUnitsInst;
                    else
                        sv2.SV205_ServiceUnitCount = string.Empty;

                    //sv2-10  level of care code
                    if (!string.IsNullOrEmpty(lvlCareInst) && !lvlCareInst.Equals("0"))
                    {
                        sv2.SV210_LevelOfCareCode = lvlCareInst;
                    }
                    else
                    {
                        sv2.SV210_LevelOfCareCode = string.Empty;
                    }

                    req2000.InstitutionalServiceLine_2000F = sv2;
                    serviceTracking = dt.Rows[0]["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"].ToString();
                    if (!string.IsNullOrEmpty(serviceTracking) && !string.IsNullOrWhiteSpace(serviceTracking))
                    {
                        trn[0].TRN01_TraceTypeCode = "1";
                        trn[0].TRN03_TraceAssigningEntityIdentifier = "1" + taxID;
                        trn[0].TRN02_PatientEventTraceNumber = serviceTracking;
                    }
                    else
                    {
                        trn[0].TRN01_TraceTypeCode = string.Empty;
                        trn[0].TRN03_TraceAssigningEntityIdentifier = string.Empty;
                        trn[0].TRN02_PatientEventTraceNumber = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(svcDescpInst) && !string.IsNullOrWhiteSpace(svcDescpInst))
                    {
                        msg.MSG01_FreeFormMessageText = svcDescpInst;
                    }
                    else
                    {
                        msg.MSG01_FreeFormMessageText = string.Empty;
                    }
                    break;
            }
            req2000.ServiceTraceNumber_2000F = trn;
            req2000.ServiceDate_2000F = dtp;
            req2000.MessageText_2000F = msg;
            return req2000;
        }

        public static BHTContainterType fillBHTContainterType(DataSet ds)
        {

            string requestType = string.Empty;
            string transactionID = string.Empty;

            DataTable dt = new DataTable();
            if (PriorAuthServiceHelper.HasRows(ds))
            {
                if (ds.Tables.Contains("PriorAuthRequestType"))
                {
                    dt = ds.Tables["PriorAuthRequestType"];
                    requestType = dt.Rows[0]["RequestType"].ToString();
                    transactionID = dt.Rows[0]["transactionID"].ToString();
                }
            }

            ServiceDetails_2000FType[] sf = new ServiceDetails_2000FType[999];


            //If PAType if institutional, then add empty service details

            string paType = string.Empty;
            if (ds.Tables.Contains("PriorAuthHeader"))
            {
                dt = ds.Tables["PriorAuthHeader"];
                paType = dt.Rows[0]["PriorAuthType"].ToString();
            }

            string svcMedicaidID = string.Empty;
            if (ds.Tables.Contains("PriorAuthServicingProvider"))
            {
                dt = ds.Tables["PriorAuthServicingProvider"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    svcMedicaidID = dt.Rows[0]["MEDICAID_ID"].ToString();
                }
            }

            if (!string.IsNullOrEmpty(paType) && paType.ToUpper() == "INSTITUTIONAL")
            {
                DataTable dtInstitutionalServiceDetails = new DataTable("PriorAuthServiceDetails");
                if (ds.Tables.Contains("PriorAuthServiceDetails"))
                {
                    dtInstitutionalServiceDetails = ds.Tables["PriorAuthServiceDetails"];
                }

                if (dtInstitutionalServiceDetails.Rows.Count <= 0)
                {
                    DataRow workRow = dtInstitutionalServiceDetails.NewRow();
                    workRow["PRIOR_AUTH_SERVICE_DETAIL_ID"] = 0;
                    workRow["PRIOR_AUTH_SERVICE_REVENUE_CODE"] = string.Empty;
                    workRow["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"] = 0;
                    workRow["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"] = string.Empty;
                    workRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"] = 0;
                    //workRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] = string.Empty;
                    //workRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] = string.Empty;
                    workRow["PRIOR_AUTH_STATUS_ID"] = 0;
                    workRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"] = string.Empty;
                    workRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"] = 0;
                    //workRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"] = string.Empty;
                    //workRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"] = string.Empty;
                    workRow["Line"] = 0;
                    //workRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"] = string.Empty;
                    //workRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"] = string.Empty;
                    workRow["PRIOR_AUTH_REQUESTED_UNITS_ID"] = 0;
                    workRow["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"] = string.Empty;
                    workRow["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"] = string.Empty;
                    workRow["PRIOR_AUTH_LEVEL_CARE_ID"] = 0;
                    workRow["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"] = string.Empty;
                    workRow["MedicaidID"] = svcMedicaidID;
                    workRow["PRIOR_AUTH_INSTITUTIONALSAVE_ID"] = 0;

                    dtInstitutionalServiceDetails.Rows.Add(workRow);
                }
            }

            DataTable dtServiceLines = new DataTable();
            int serviceLinesRecords = 0;

            if (PriorAuthServiceHelper.HasRows(ds))
            {
                if (ds.Tables.Contains("PriorAuthServiceDetails"))
                {
                    dtServiceLines = ds.Tables["PriorAuthServiceDetails"];
                }
                serviceLinesRecords = dtServiceLines.Rows.Count;
            }

            for (int i = 0; i < serviceLinesRecords; i++)
            {
                sf[i] = fillServiceDetails_2000FType(ds, i);
            }


            BHTContainterType bty = new BHTContainterType();
            bty.BeginningOfHierarchicalTransaction = fillBHTType(transactionID, requestType);
            bty.UMODetails_2000A = fillUMODetails_2000AType(ds);
            bty.RequesterDetails_2000B = fillRequesterDetails_2000B(ds);
            bty.SubscriberDetails_2000C = fillSubscriberDetails_2000CType(ds);
            bty.DependentDetails_2000D = fillDependentDetails_2000DType(ds);
            bty.PatientEventDetails_2000E = fillPatientEventDetails_2000EType(ds);
            bty.ServiceDetails = sf;



            return bty;
        }

        public static SEType fillSEType(DataSet ds)
        {
            string transactionID = string.Empty;

            DataTable dt = new DataTable();
            if (PriorAuthServiceHelper.HasRows(ds))
            {
                if (ds.Tables.Contains("PriorAuthRequestType"))
                {
                    dt = ds.Tables["PriorAuthRequestType"];
                    transactionID = dt.Rows[0]["transactionID"].ToString();
                }
            }


            SEType sety = new SEType();
            sety.SE01_TransactionSegmentCount = "28";
            sety.SE02_TransactionSetControlNumber = transactionID.PadLeft(9, '0');

            return sety;
        }

        public static GEType fillGEType(DataSet ds)
        {

            string transactionID = string.Empty;

            DataTable dt = new DataTable();
            if (PriorAuthServiceHelper.HasRows(ds))
            {
                if (ds.Tables.Contains("PriorAuthRequestType"))
                {
                    dt = ds.Tables["PriorAuthRequestType"];
                    transactionID = dt.Rows[0]["transactionID"].ToString();
                }
            }

            GEType gety = new GEType();
            gety.GE01_NumberTransactionSetsIncluded = "1";
            gety.GE02_GroupControlNumber = transactionID.PadLeft(9, '0');

            return gety;
        }

        public static IEAType fillIEAType(DataSet ds)
        {
            string transactionID = string.Empty;

            DataTable dt = new DataTable();
            if (PriorAuthServiceHelper.HasRows(ds))
            {
                if (ds.Tables.Contains("PriorAuthRequestType"))
                {
                    dt = ds.Tables["PriorAuthRequestType"];
                    transactionID = dt.Rows[0]["transactionID"].ToString();
                }
            }


            IEAType ieatyp = new IEAType();
            ieatyp.IEA01_NumberIncludedFunctionalGroups = "1";
            ieatyp.IEA02_InterchangeControlNumber = transactionID.PadLeft(9, '0');

            return ieatyp;
        }


        public static string CreateAndReturnLogThreadNumber(Exception ex, string errorKey = "")
        {
            string logid = HttpContext.Current.Session["LogKey"] != null ? HttpContext.Current.Session["LogKey"].ToString() : CON.appAdminUserId;
            Logging logging = new Logging(new Guid(logid));
            string logMessage = errorKey + " " + logging.GetRecursiveException(ex);
            logging.CreateLogEntry(logMessage, CON.WebPageProcessName.SubmitPriorAuthorization);
            return logging.ThreadId.ToString();
        }

        public static string CreateAndReturnLogInfoThreadNumber(string logMessage = "")
        {
            string logid = HttpContext.Current.Session["LogKey"] != null ? HttpContext.Current.Session["LogKey"].ToString() : CON.appAdminUserId;
            Logging logging = new Logging(new Guid(logid));
            logging.CreateLogEntry(logMessage, CON.WebPageProcessName.SubmitPriorAuthorization);
            return logging.ThreadId.ToString();
        }


    }
}
