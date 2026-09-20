using Corp.Core.Libraries;
using Corp.Core.Libraries.AttachmentServiceReference;
using Corp.Core.Libraries.HospiceReference;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;

/// <summary>
/// Summary description for WorkflowPage
/// </summary>
public class WorkflowPage : RegistrationProvider
{

    protected void FillRegistrationData()
    {
        IsWaiverServiceProvider = Helper.IsWaiverServiceProvider(RegistrationId);
        PDMSService.PDMSServiceClient client = null;
        DataSet ds1 = null;

        try
        {
            client = new PDMSService.PDMSServiceClient();
            ds1 = client.SelectRegistrationByRegID(RegistrationId);
            client.Close();
        }
        catch (CommunicationException)
        {
            client.Abort();
        }
        catch (TimeoutException)
        {
            client.Abort();
        }
        catch (Exception)
        {
            client.Abort();
            throw;
        }
        if (Helper.HasRows(ds1))
        {
            DataRow dr = ds1.Tables[0].Rows[0];
            WF_TaskID = Methods.GetIntValue(dr["CurrentTaskID"]);
            WF_ProcessID = Methods.GetIntValue(dr, "ProcessID");
            WF_StepID = Methods.GetIntValue(dr, "CurrentStepID");
            WF_WorkflowID = Methods.GetIntValue(dr, "WorkflowID");
            WF_TaskType = Methods.GetStringValue(dr, "CurrentTaskType");
            WF_StepOwner = Methods.GetStringValue(dr, "CurrentStepOwnerID");
            ReferralTypeID = Methods.GetIntValue(dr, "DIDD_REFERRAL_TYPE_ID");
            //RegistrationStatusTypeID = Methods.GetIntValue(dr, "RegistrationStatusTypeID");
            //RegistrationProgramStatusTypeID = Methods.GetIntValue(dr, "RegProgramStatusTypeID");
            //ApplicationTypeID = Methods.GetIntValue(dr, "ApplicationTypeID");
            WorkflowEventTypeId = Methods.GetIntValue(dr, "WORKFLOW_EVENT_TYPE_ID");
            CurrentTaskName = Methods.GetStringValue(dr, "CurrentTaskName");
            WorkflowName = Methods.GetStringValue(dr, "WorkflowName");
            MedicaidID = Methods.GetStringValue(dr, "MedicaidID");
            EntityTypeID = Methods.GetIntValue(dr, "ProviderCategoryTypeID");
            ApplicationTypeID = Methods.GetIntValue(dr, "ApplicationTypeID");
            ProviderTypeID = Methods.GetIntValue(dr, "ProviderTypeID");
            MMISProviderTypeID = Methods.GetStringValue(dr, "MMISProviderTypeID");
            NPI = Methods.GetStringValue(dr, "NPI");
            IsCredentialingProvider = Methods.GetIntValue(dr, "IsCredentialingProvider") == 1 ? true : false;
            IsProviderReactivation = Methods.GetIntValue(dr, "IsProviderReactivation") == 1 ? true : false;
            IsReapplication = Methods.GetIntValue(dr, "isreapplication") == 1 ? true : false;
            WorkingState = "Montana"; //Commentted this as chip information no need to display as per kelly which uses this.
            IsAddODMorODAMedSvc = Methods.GetIntValue(dr, "IsAddODMorODAMedSvc") == 1 ? true : false;
            HasODMSpecialty = Methods.GetIntValue(dr, "HasODMSpecialty") == 1 ? true : false;
            HasActiveODMSpecialty = Methods.GetIntValue(dr, "HasActiveODMSpecialty") == 1 ? true : false;
            HasDODDSpecialty = Methods.GetIntValue(dr, "HasDODDSpecialty") == 1 ? true : false;
            HasActiveDODDSpecialty = Methods.GetIntValue(dr, "HasActiveDODDSpecialty") == 1 ? true : false;
            HasODASpecialty = Methods.GetIntValue(dr, "HasODASpecialty") == 1 ? true : false;
            HasActiveODASpecialty = Methods.GetIntValue(dr, "HasActiveODASpecialty") == 1 ? true : false;
			WaiverServiceUpdateTypeID = Methods.GetIntValue(dr, "WaiverServiceUpdateTypeID");
            WaiverTypeID = Methods.GetIntValue(dr["WaiverTypeID"]);
			IsReactivation = Methods.GetIntValue(dr, "IsProviderReactivation") == 1 ? true : false;
            IsSuspendedProvider = Methods.GetIntValue(dr, "RegistrationProgramStatusTypeID") == CON.RegistrationProgramStatusTypeId.Suspended ? true : false;
        }
        GetProcessParameters();
        GetStepParameters();
        GetStepInfo();
    }

    private void LoadProviderInformation(string medicaidNumber)
    {
        PDMSService.PDMSServiceClient client = new PDMSService.PDMSServiceClient();
        DataSet ds = client.SelectProviderByGRPMedicaidID(medicaidNumber);
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
        if (Helper.HasRows(dtMisc))
        {
            DataRow dr = dtMisc.Rows[0];
            this.RegistrationId = Helper.GetInt("REG_ID", dr);
        }
    }
    
    public InquireHospiceResponse ConvertDatasetToInquireResponse(DataSet ds)
    {
        InquireHospiceResponse response = new InquireHospiceResponse
        {
            ResponseHeader = (ds.Tables["ResponseHeader"] != null) ? DatatableHelper.ConvertDataTableToList<InquireResponseResponseHeader>
            (ds.Tables["ResponseHeader"]).FirstOrDefault() : null,
            HospiceRequestResponse = new InquireResponseHospiceRequestResponse
            {
                //MessageHeader = (ds.Tables["MessageHeader"] != null) ?DatatableHelper.ConvertDataTableToList<HospiceRequestResponseMessageHeader>
                //(ds.Tables["MessageHeader"]).FirstOrDefault() : null,
                //Payload = new HospiceRequestResponsePayload
                //{
                ServiceCountyState = (ds.Tables["ServiceCountyState"] != null) ? DatatableHelper.ConvertDataTableToList<InquireResponseHospiceRequestResponseServiceCountyState>
                (ds.Tables["ServiceCountyState"]).ToArray() : null,
                ElectionDisenrollDates = (ds.Tables["ElectionDisenrollDates"] != null) ? DatatableHelper.ConvertDataTableToList<InquireResponseHospiceRequestResponseElectionDisenrollDates>
                (ds.Tables["ElectionDisenrollDates"]).FirstOrDefault() : null,
                LongTermCareFacility = (ds.Tables["LongTermCareFacility"] != null) ? DatatableHelper.ConvertDataTableToList<InquireResponseHospiceRequestResponseLongTermCareFacility>
                (ds.Tables["LongTermCareFacility"]).ToArray() : null,
                BenefitPeriods = (ds.Tables["BenefitPeriods"] != null) ? DatatableHelper.ConvertDataTableToList<InquireResponseHospiceRequestResponseBenefitPeriods>
                (ds.Tables["BenefitPeriods"]).ToArray() : null,
                OtherPayerInfo = (ds.Tables["OtherPayerInfo"] != null) ? DatatableHelper.ConvertDataTableToList<InquireResponseHospiceRequestResponseOtherPayerInfo>
                (ds.Tables["OtherPayerInfo"]).ToArray() : null,
                //    HLTCFProviderService = (ds.Tables["HLTCFProviderService"]) != null) ? DatatableHelper.ConvertDataTableToList<HospiceRequestResponsePayloadLongTermCareFacility>
                //(ds.Tables["HLTCFProviderService"]).ToArray() : null,
                DiagnosisCodes = (ds.Tables["DiagnosisCodes"] != null) ? DatatableHelper.ConvertDataTableToList<InquireResponseHospiceRequestResponseDiagnosisCodes>
                (ds.Tables["DiagnosisCodes"]).ToArray() : null,
                ProvService = (ds.Tables["ProvService"] != null) ? DatatableHelper.ConvertDataTableToList<InquireResponseHospiceRequestResponseProvService>
                (ds.Tables["ProvService"]).ToArray() : null,
                Attachments = (ds.Tables["Attachments"] != null) ? DatatableHelper.ConvertDataTableToList<InquireResponseHospiceRequestResponseAttachments>
                (ds.Tables["Attachments"]).ToArray() : null,
                ActionType = (ds.Tables["HospiceRequestResponse"] != null) ? Convert.ToString(ds.Tables["HospiceRequestResponse"].Rows[0]["ActionType"]) : null,
                RecipID = (ds.Tables["HospiceRequestResponse"] != null) ? Convert.ToString(ds.Tables["HospiceRequestResponse"].Rows[0]["RecipID"]) : null,
                ConsBirthDate = (ds.Tables["HospiceRequestResponse"] != null) ? Convert.ToDateTime(ds.Tables["HospiceRequestResponse"].Rows[0]["ConsBirthDate"]) : DateTime.Now,
                ConsFirstName = (ds.Tables["HospiceRequestResponse"] != null) ? Convert.ToString(ds.Tables["HospiceRequestResponse"].Rows[0]["ConsFirstName"]) : null,
                ConsLastName = (ds.Tables["HospiceRequestResponse"] != null) ? Convert.ToString(ds.Tables["HospiceRequestResponse"].Rows[0]["ConsLastName"]) : null,
                HospiceTrackNo = (ds.Tables["HospiceRequestResponse"] != null) ? Convert.ToInt32(ds.Tables["HospiceRequestResponse"].Rows[0]["HospiceTrackNo"]) : 0,
                //}
            },

            EpisodeOfCare = (ds.Tables["EpisodeOfCare"] != null) ? DatatableHelper.ConvertDataTableToList<InquireResponseEpisodeOfCare>
                (ds.Tables["EpisodeOfCare"]).ToArray() : null,
            Errors = (ds.Tables["Errors"] != null) ? DatatableHelper.ConvertDataTableToList<InquireResponseErrors>
                (ds.Tables["Errors"]).ToArray() : null
        };

        this.HospiceRequestResponse = new AddUpdateHospiceRequest
        {
            Payload = new HospiceRequestResponsePayload
            {
                ServiceCountyState = (ds.Tables["ServiceCountyState"] != null) ? DatatableHelper.ConvertDataTableToList<HospiceRequestResponsePayloadServiceCountyState>
         (ds.Tables["ServiceCountyState"]).ToArray() : null,
                ElectionDisenrollDates = (ds.Tables["ElectionDisenrollDates"] != null) ? DatatableHelper.ConvertDataTableToList<HospiceRequestResponsePayloadElectionDisenrollDates>
         (ds.Tables["ElectionDisenrollDates"]).FirstOrDefault() : null,
                LongTermCareFacility = (ds.Tables["LongTermCareFacility"] != null) ? DatatableHelper.ConvertDataTableToList<HospiceRequestResponsePayloadLongTermCareFacility>
         (ds.Tables["LongTermCareFacility"]).ToArray() : null,
                BenefitPeriods = (ds.Tables["BenefitPeriods"] != null) ? DatatableHelper.ConvertDataTableToList<HospiceRequestResponsePayloadBenefitPeriods>
         (ds.Tables["BenefitPeriods"]).ToArray() : null,
                OtherPayerInfo = (ds.Tables["OtherPayerInfo"] != null) ? DatatableHelper.ConvertDataTableToList<HospiceRequestResponsePayloadOtherPayerInfo>
         (ds.Tables["OtherPayerInfo"]).ToArray() : null,
                //    HLTCFProviderService = (ds.Tables["HLTCFProviderService"]) != null) ? DatatableHelper.ConvertDataTableToList<HospiceRequestResponsePayloadLongTermCareFacility>
                //(ds.Tables["HLTCFProviderService"]).ToArray() : null,
                DiagnosisCodes = (ds.Tables["DiagnosisCodes"] != null) ? DatatableHelper.ConvertDataTableToList<HospiceRequestResponsePayloadDiagnosisCodes>
         (ds.Tables["DiagnosisCodes"]).ToArray() : null,
                ProvService = (ds.Tables["ProvService"] != null) ? DatatableHelper.ConvertDataTableToList<HospiceRequestResponsePayloadProvService>
         (ds.Tables["ProvService"]).ToArray() : null,
                Attachments = (ds.Tables["Attachments"] != null) ? DatatableHelper.ConvertDataTableToList<HospiceRequestResponsePayloadAttachments>
         (ds.Tables["Attachments"]).ToArray() : null,
                ActionType = (response.HospiceRequestResponse != null) ? response.HospiceRequestResponse.ActionType : null,
                RecipID = (response.HospiceRequestResponse != null) ? response.HospiceRequestResponse.RecipID : null,
                ConsBirthDate = (response.HospiceRequestResponse != null) ? response.HospiceRequestResponse.ConsBirthDate : DateTime.MinValue,
                ConsFirstName = (response.HospiceRequestResponse != null) ? response.HospiceRequestResponse.ConsFirstName : null,
                ConsLastName = (response.HospiceRequestResponse != null) ? response.HospiceRequestResponse.ConsLastName : null,
                HospiceTrackNo = (response.HospiceRequestResponse != null) ? response.HospiceRequestResponse.HospiceTrackNo : 0,
                HospiceTrackNoSpecified = (response.HospiceRequestResponse != null) ? response.HospiceRequestResponse.HospiceTrackNoSpecified : false
            }
        };

        return response;
    }
    //public HospiceRequestResponse ConvertInquireResponseToHospiceRequestResponse(InquireResponse inquireResponse)
    //{
    //    HospiceRequestResponse response = new HospiceRequestResponse
    //    { 
    //            Payload = new HospiceRequestResponsePayload
    //            {
    //                ServiceCountyState = (inquireResponse.HospiceRequestResponse.Payload.ServiceCountyState != null) ? DatatableHelper.MapSourceToDest<InquireResponseHospiceRequestResponsePayloadServiceCountyState, HospiceRequestResponsePayloadServiceCountyState>(inquireResponse.HospiceRequestResponse.Payload.ServiceCountyState.ToList()).ToArray() : null, 
    //                ElectionDisenrollDates = (inquireResponse.HospiceRequestResponse.Payload.ElectionDisenrollDates != null) ? DatatableHelper.MapSourceToDest<InquireResponseHospiceRequestResponsePayloadElectionDisenrollDates, HospiceRequestResponsePayloadElectionDisenrollDates>(inquireResponse.HospiceRequestResponse.Payload.ElectionDisenrollDates.ToList()).ToArray() : null,
    //                LongTermCareFacility = (inquireResponse.HospiceRequestResponse.Payload.LongTermCareFacility != null) ? DatatableHelper.MapSourceToDest<InquireResponseHospiceRequestResponsePayloadLongTermCareFacility, HospiceRequestResponsePayloadLongTermCareFacility>(inquireResponse.HospiceRequestResponse.Payload.LongTermCareFacility.ToList()).ToArray() : null,
    //                BenefitPeriods = (inquireResponse.HospiceRequestResponse.Payload.BenefitPeriods != null) ? DatatableHelper.MapSourceToDest<InquireResponseHospiceRequestResponsePayloadBenefitPeriods, HospiceRequestResponsePayloadBenefitPeriods>(inquireResponse.HospiceRequestResponse.Payload.BenefitPeriods.ToList()).ToArray() : null,
    //                OtherPayerInfo = (inquireResponse.HospiceRequestResponse.Payload.OtherPayerInfo != null) ? DatatableHelper.MapSourceToDest<InquireResponseHospiceRequestResponsePayloadOtherPayerInfo, HospiceRequestResponsePayloadOtherPayerInfo>(inquireResponse.HospiceRequestResponse.Payload.OtherPayerInfo.ToList()).ToArray() : null,
    //                //    HLTCFProviderService = (ds.Tables["HLTCFProviderService"]) != null) ? DatatableHelper.ConvertDataTableToList<HospiceRequestResponsePayloadLongTermCareFacility>
    //                //(ds.Tables["HLTCFProviderService"]).ToArray() : null,
    //                DiagnosisCodes = (inquireResponse.HospiceRequestResponse.Payload.DiagnosisCodes != null) ? DatatableHelper.MapSourceToDest<InquireResponseHospiceRequestResponsePayloadDiagnosisCodes, HospiceRequestResponsePayloadDiagnosisCodes>(inquireResponse.HospiceRequestResponse.Payload.DiagnosisCodes.ToList()).ToArray() : null,
    //                ProvService = (inquireResponse.HospiceRequestResponse.Payload.ProvService != null) ? DatatableHelper.MapSourceToDest<InquireResponseHospiceRequestResponsePayloadProvService, HospiceRequestResponsePayloadProvService>(inquireResponse.HospiceRequestResponse.Payload.ProvService.ToList()).ToArray() : null,
    //                Attachments = (inquireResponse.HospiceRequestResponse.Payload.Attachments != null) ? DatatableHelper.MapSourceToDest<InquireResponseHospiceRequestResponsePayloadAttachments, HospiceRequestResponsePayloadAttachments>(inquireResponse.HospiceRequestResponse.Payload.Attachments.ToList()).ToArray() : null,
    //                ActionType = (inquireResponse.HospiceRequestResponse.Payload != null) ? inquireResponse.HospiceRequestResponse.Payload.ActionType : null,
    //                RecipID = (inquireResponse.HospiceRequestResponse.Payload != null) ? inquireResponse.HospiceRequestResponse.Payload.RecipID : null,
    //                ConsBirthDate = (inquireResponse.HospiceRequestResponse.Payload != null) ? inquireResponse.HospiceRequestResponse.Payload.ConsBirthDate : DateTime.Now,
    //                ConsFirstName = (inquireResponse.HospiceRequestResponse.Payload != null) ? inquireResponse.HospiceRequestResponse.Payload.ConsFirstName : null,
    //                ConsLastName = (inquireResponse.HospiceRequestResponse.Payload != null) ? inquireResponse.HospiceRequestResponse.Payload.ConsLastName : null,
    //                HospiceTrackNo = (inquireResponse.HospiceRequestResponse.Payload != null) ? inquireResponse.HospiceRequestResponse.Payload.HospiceTrackNo : 0,
    //                HospiceTrackNoSpecified= (inquireResponse.HospiceRequestResponse.Payload != null) ? inquireResponse.HospiceRequestResponse.Payload.HospiceTrackNoSpecified : false
    //            }
    //    };

    //    return response;
    //}
    public DataTable GetHospiceBenifitSegmentIndicatorType()
    {
        DataTable dtseg = new DataTable();
        dtseg.Columns.Add("INDICATOR_TYPE_VALUE", typeof(int));
        dtseg.Columns.Add("INDICATOR_TYPE_DESC", typeof(string));
        var inquireResponse = this.HospiceRequestResponse;
        if (inquireResponse != null
            && inquireResponse.Payload != null
            && inquireResponse.Payload.BenefitPeriods != null
            && inquireResponse.Payload.BenefitPeriods.Count() > 0)
        {
            var benefitPeriods = (from r in inquireResponse.Payload.BenefitPeriods.ToList()
                                  select
                                        GetSegmentIndicatorByValue(r.BenPeriodType.ToString())
                                         + "-" + r.BenPeriodEffDate.ToString("MM/dd/yyyy")
                                         + "-" + r.BenPeriodEndDate.ToString("MM/dd/yyyy")).ToList();
            int i = 1;
            foreach (var benefitPeriod in benefitPeriods)
            {
                DataRow row = dtseg.NewRow();
                row["INDICATOR_TYPE_VALUE"] = i;
                row["INDICATOR_TYPE_DESC"] = benefitPeriod;
                dtseg.Rows.Add(row);
                //TODO
                //inquireResponse.HospiceRequestResponse.Payload.BenefitPeriods[i - 1].SegmentIndicator = benefitPeriod;
                i++;
            }
        }

        return dtseg;
    }
    public DataTable GetHospiceBenifitLineNo()
    {
        DataTable dtseg = new DataTable();
        dtseg.Columns.Add("LineNo", typeof(int));
        dtseg.Columns.Add("BenefitPeriod", typeof(string));
        var inquireResponse = this.HospiceRequestResponse;
        if (inquireResponse != null
            && inquireResponse.Payload != null
            && inquireResponse.Payload.BenefitPeriods != null
            && inquireResponse.Payload.BenefitPeriods.Count() > 0)
        {
            var benefitPeriods = (from r in inquireResponse.Payload.BenefitPeriods.ToList()
                                  where r.IsDifferentProdvider == false || this.HospiceSelectedActionType == "CHGPR"
                                  select r.BenPeriod).ToList();
            int i = 1;
            foreach (var lineNo in benefitPeriods)
            {
                DataRow row = dtseg.NewRow();
                row["LineNo"] = lineNo;
                row["BenefitPeriod"] = inquireResponse.Payload.BenefitPeriods.Where(x => x.BenPeriod == lineNo).Select(y => GetSegmentIndicatorByValue(y.BenPeriodType.ToString())
                                           + ";" + y.BenPeriodEffDate.ToString("MM/dd/yyyy")
                                           + "-" + y.BenPeriodEndDate.ToString("MM/dd/yyyy")).FirstOrDefault();
                dtseg.Rows.Add(row);
                //TODO
                //inquireResponse.HospiceRequestResponse.Payload.BenefitPeriods[i - 1].SegmentIndicator = benefitPeriod;
                i++;
            }
        }

        return dtseg;
    }
    public string GetSegmentIndicatorByValue(string segmentIndicatorId)
    {
        var dt = GetHospiceBenifitSegmentIndicatorTypes();
        return (from q in dt.AsEnumerable()
                                    where q.Field<int>("INDICATOR_TYPE_VALUE") == Convert.ToInt32(segmentIndicatorId)
                                    select q.Field<string>("INDICATOR_TYPE_DESC")).FirstOrDefault();
    }
    public int GetSegmentIndicatorByText(string segmentIndicator)
    {
        var dt = GetHospiceBenifitSegmentIndicatorTypes();
        return (from q in dt.AsEnumerable()
                where q.Field<string>("INDICATOR_TYPE_DESC") == segmentIndicator
                select q.Field<int>("INDICATOR_TYPE_VALUE")).FirstOrDefault();
    }
    private DataTable GetHospiceBenifitSegmentIndicatorTypes()
    {
        if (this.HospiceBenifitSegmentIndicatorType != null && this.HospiceBenifitSegmentIndicatorType.Rows.Count > 0)
        {
            return this.HospiceBenifitSegmentIndicatorType;
        }
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            this.HospiceBenifitSegmentIndicatorType = psc.SelectHospiceBenifitSegmentIndicatorType().Tables[0];
            return this.HospiceBenifitSegmentIndicatorType;
        }
    }
    public InquireHospiceResponse InquireHospiceResponse
    {
        get
        {
            return ViewState["InquireHospiceResponse"] == null ? null : (InquireHospiceResponse)ViewState["InquireHospiceResponse"];
        }
        set
        {
            ViewState["InquireHospiceResponse"] = value;
        }
    }
    public AddUpdateHospiceRequest HospiceRequestResponse
    {
        get
        {
            return ViewState["HospiceRequestResponse"] == null ? null : (AddUpdateHospiceRequest)ViewState["HospiceRequestResponse"];
        }
        set
        {
            ViewState["HospiceRequestResponse"] = value;
        }
    }
    public string HospiceSelectedActionType
    {
        get
        {
            return ViewState["HospiceSelectedActionType"] == null ? null : (string)ViewState["HospiceSelectedActionType"];
        }
        set
        {
            ViewState["HospiceSelectedActionType"] = value;
        }
    }
    public AddUpdateHospiceRequest PreviousProviderHospiceRequestResponse
    {
        get
        {
            return ViewState["PreviousProviderHospiceRequestResponse"] == null ? null : (AddUpdateHospiceRequest)ViewState["PreviousProviderHospiceRequestResponse"];
        }
        set
        {
            ViewState["PreviousProviderHospiceRequestResponse"] = value;
        }
    }
    public string PreviousSelectedHospiceAttachmentDocType
    {
        get
        {
            if (ViewState["PreviousSelectedHospiceAttachmentDocType"] == null) ViewState["PreviousSelectedHospiceAttachmentDocType"] = string.Empty;
            return (string)ViewState["PreviousSelectedHospiceAttachmentDocType"];
        }
        set { ViewState["PreviousSelectedHospiceAttachmentDocType"] = value; }
    }
    public SearchHospiceResponse SearchHospiceResponse
    {
        get
        {
            return ViewState["SearchHospiceResponse"] == null ? null : (SearchHospiceResponse)ViewState["SearchHospiceResponse"];
        }
        set
        {
            ViewState["SearchHospiceResponse"] = value;
        }
    }
    public virtual void ucRegistrationNavigation_RefreshEvent(int step)
    {

    }
    private void GetProcessParameters()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.WF_SelectProcessParameters(WF_ProcessID);
        ProcessParameter = new Dictionary<string, string>();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow myRow = ds.Tables[0].Rows[0];
            for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
            {
                if (!ProcessParameter.ContainsKey(ds.Tables[0].Columns[j].ColumnName.ToString()))
                {
                    ProcessParameter.Add(ds.Tables[0].Columns[j].ColumnName.ToString(), myRow.ItemArray[j].ToString());
                }
                else
                {
                    Logging log = new Logging(new Guid(), null);
                    log.CreateLogEntry("An item with the same key already been added in the dictionary ", Logging.LogPriority.Error);
                }
            }
        }
    }

    private void GetStepParameters()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.WF_SelectStepParameters(WF_StepID);
        StepParameter = new Dictionary<string, string>();
        if (ds.Tables.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StepParameter.Add(dr["PARAMETER_NAME"].ToString(), dr["PARAMETER_VALUE"].ToString());
            }
        }
    }

    private void GetStepInfo()
    {
        //DataSet ds = svc.WF_SelectStepInfo(WF_StepID);
        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
        //    WorkflowID = Convert.ToInt32(ds.Tables[0].Rows[0]["WORKFLOW_ID"]);
        //    WorkflowName = ds.Tables[0].Rows[0]["WORKFLOW_NAME"].ToString();
        //    TaskID = Convert.ToInt32(ds.Tables[0].Rows[0]["TASK_ID"]);
        //    TaskType = ds.Tables[0].Rows[0]["TASK_TYPE"].ToString();
        //    TaskName = ds.Tables[0].Rows[0]["TASK_NAME"].ToString();
        //    //this.ucRegTreeView.CurrentTaskName = TaskName;
        //    this.WorkflowPage.CurrentTaskName = TaskName;
        //    AssemblyName = ds.Tables[0].Rows[0]["ASSEMBLY_NAME"].ToString();
        //    ClassName = ds.Tables[0].Rows[0]["CLASS_NAME"].ToString();
        //    GroupNameList = ds.Tables[0].Rows[0]["GROUP_NAME_LIST"].ToString();
        //    StepID = Convert.ToInt32(ds.Tables[0].Rows[0]["STEP_ID"]);
        //    CallingStepID = Convert.ToInt32(ds.Tables[0].Rows[0]["CALLING_STEP_ID"]);
        //    StepOwner = ds.Tables[0].Rows[0]["STEP_OWNER_ID"].ToString();
        //    StepCreateDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["STEP_CREATE_DATE_TIME"]);
        //    StepStartDate = ds.Tables[0].Rows[0]["STEP_START_DATE_TIME"] != DBNull.Value
        //        ? Convert.ToDateTime(ds.Tables[0].Rows[0]["STEP_START_DATE_TIME"])
        //        : StepCreateDate;
        //    StepEndDate = ds.Tables[0].Rows[0]["STEP_END_DATE_TIME"] != DBNull.Value
        //        ? (DateTime?)Convert.ToDateTime(ds.Tables[0].Rows[0]["STEP_END_DATE_TIME"])
        //        : null;
        //    StepNotes = ds.Tables[0].Rows[0]["STEP_NOTES"].ToString();
        //    ProcessID = Convert.ToInt32(ds.Tables[0].Rows[0]["PROCESS_ID"]);
        //    ProcessOwner = ds.Tables[0].Rows[0]["PROCESS_OWNER_ID"].ToString();
        //    ProcessStartDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["PROCESS_START_DATE_TIME"]);
        //    ProcessEndDate = ds.Tables[0].Rows[0]["PROCESS_END_DATE_TIME"] != DBNull.Value
        //        ? (DateTime?)Convert.ToDateTime(ds.Tables[0].Rows[0]["PROCESS_END_DATE_TIME"])
        //        : null;
        //    LogThreadID = new Guid(ds.Tables[0].Rows[0]["LOG_THREAD_NUMBER"].ToString());

        //    // Only get task parameters if Task ID is retrieved in Step Info (should always happen.)
        //    GetTaskParameters();

        //}
        //else
        //    this.WorkflowPage.CurrentTaskName = string.Empty;
    }

    public Dictionary<string, string> TaskParameter;
    public Dictionary<string, string> ProcessParameter;
    public Dictionary<string, string> StepParameter;

    #region RegistrationData
    // TODO: EDV There shouldnt be a need for current task name. Need refactor afterwards on this.
    public string CurrentTaskName
    {
        get
        {
            if (ViewState["CurrentTaskName"] == null) ViewState["CurrentTaskName"] = string.Empty;
            return (string)ViewState["CurrentTaskName"];
        }
        set { ViewState["CurrentTaskName"] = value; }
    }

    public string MedicaidID
    {
        get
        {
            if (ViewState["MedicaidID"] == null) ViewState["MedicaidID"] = "0";
            return (string)ViewState["MedicaidID"];
        }
        set { ViewState["MedicaidID"] = value; }
    }

    public string ProviderName
    {
        get
        {
            if (ViewState["ProviderName"] == null) ViewState["ProviderName"] = 0;
            return (string)ViewState["ProviderName"];
        }
        set { ViewState["ProviderName"] = value; }
    }

    public string HospiceTrackNo
    {
        get
        {
            if (ViewState["HospiceTrackNo"] == null) ViewState["HospiceTrackNo"] = "0";
            return (string)ViewState["HospiceTrackNo"];
        }
        set { ViewState["HospiceTrackNo"] = value; }
    }

    public bool IsHospiceEnrollmentClick
    {
        get
        {
            if (ViewState["IsHospiceEnrollmentClick"] == null) ViewState["IsHospiceEnrollmentClick"] = false;
            return (bool)ViewState["IsHospiceEnrollmentClick"];
        }
        set { ViewState["IsHospiceEnrollmentClick"] = value; }
    }
    public bool IsNewHospiceBenefitPeriod
    {
        get
        {
            if (ViewState["IsNewHospiceBenefitPeriod"] == null) ViewState["IsNewHospiceBenefitPeriod"] = false;
            return (bool)ViewState["IsNewHospiceBenefitPeriod"];
        }
        set { ViewState["IsNewHospiceBenefitPeriod"] = value; }
    }
    public bool IsHospiceTerminatedAndNewEnrollment
    {
        get
        {
            if (ViewState["IsHospiceTerminatedAndNewEnrollment"] == null) ViewState["IsHospiceTerminatedAndNewEnrollment"] = false;
            return (bool)ViewState["IsHospiceTerminatedAndNewEnrollment"];
        }
        set { ViewState["IsHospiceTerminatedAndNewEnrollment"] = value; }
    }
    public bool IsNewHospiceEnrollment
    {
        get
        {
            if (ViewState["IsNewHospiceEnrollment"] == null) ViewState["IsNewHospiceEnrollment"] = false;
            return (bool)ViewState["IsNewHospiceEnrollment"];
        }
        set { ViewState["IsNewHospiceEnrollment"] = value; }
    }
    public int HospiceTerminatedBenefitNumber
    {
        get
        {
            if (ViewState["HospiceTerminatedBenefitNumber"] == null) ViewState["HospiceTerminatedBenefitNumber"] = 0;
            return (int)ViewState["HospiceTerminatedBenefitNumber"];
        }
        set { ViewState["HospiceTerminatedBenefitNumber"] = value; }
    }
    public string HospiceStatus
    {
        get
        {
            if (ViewState["HospiceStatus"] == null) ViewState["HospiceStatus"] = "";
            return (string)ViewState["HospiceStatus"];
        }
        set { ViewState["HospiceStatus"] = value; }
    }
    public int IsBenefitPeriodAdded
    {
        get
        {
            if (ViewState["IsBenefitPeriodAdded"] == null) ViewState["IsBenefitPeriodAdded"] = 0;
            return (int)ViewState["IsBenefitPeriodAdded"];
        }
        set { ViewState["IsBenefitPeriodAdded"] = value; }
    }
    public bool IsHospiceCheckEligibilityClick
    {
        get
        {
            if (ViewState["IsHospiceCheckEligibilityClick"] == null) ViewState["IsHospiceCheckEligibilityClick"] = false;
            return (bool)ViewState["IsHospiceCheckEligibilityClick"];
        }
        set { ViewState["IsHospiceCheckEligibilityClick"] = value; }
    }
    public string MedicaidBillingNumber
    {
        get
        {
            if (ViewState["MedicaidBillingNumber"] == null) ViewState["MedicaidBillingNumber"] = "0";
            return (string)ViewState["MedicaidBillingNumber"];
        }
        set { ViewState["MedicaidBillingNumber"] = value; }
    }

    public string RecipientDateOfBirth
    {
        get
        {
            if (ViewState["RecipientDateOfBirth"] == null) ViewState["RecipientDateOfBirth"] = string.Empty;
            return (string)ViewState["RecipientDateOfBirth"];
        }
        set { ViewState["RecipientDateOfBirth"] = value; }
    }
    public RecipientInformation RecipientInformation
    {
        get
        {
            if (ViewState["RecipientInformation"] == null) ViewState["RecipientInformation"] = new RecipientInformation();
            return (RecipientInformation)ViewState["RecipientInformation"];
        }
        set { ViewState["RecipientInformation"] = value; }
    }
    public string WorkflowName
    {
        get
        {
            if (ViewState["WorkflowName"] == null) ViewState["WorkflowName"] = string.Empty;
            return (string)ViewState["WorkflowName"];
        }
        set { ViewState["WorkflowName"] = value; }
    }

    public int WF_ProcessID
    {
        get
        {
            if (ViewState["WF_ProcessID"] == null) ViewState["WF_ProcessID"] = 0;
            return (int)ViewState["WF_ProcessID"];
        }
        set { ViewState["WF_ProcessID"] = value; }
    }

    public int WF_StepID
    {
        get
        {
            if (ViewState["WF_StepID"] == null) ViewState["WF_StepID"] = 0;
            return (int)ViewState["WF_StepID"];
        }
        set { ViewState["WF_StepID"] = value; }
    }

    public int WF_WorkflowID
    {
        get
        {
            if (ViewState["WF_WorkflowID"] == null) ViewState["WF_WorkflowID"] = 0;
            return (int)ViewState["WF_WorkflowID"];
        }
        set { ViewState["WF_WorkflowID"] = value; }
    }

    public int WF_TaskID
    {
        get
        {
            if (ViewState["WF_TaskID"] == null) ViewState["WF_TaskID"] = 0;
            return (int)ViewState["WF_TaskID"];
        }
        set { ViewState["WF_TaskID"] = value; }
    }
	
    public string WF_TaskType
    {
        get
        {
            if (ViewState["WF_TaskType"] == null) ViewState["WF_TaskType"] = string.Empty;
            return (string)ViewState["WF_TaskType"];
        }
        set { ViewState["WF_TaskType"] = value; }
    }

    public string WF_StepOwner
    {
        get
        {
            if (ViewState["WF_StepOwner"] == null) ViewState["WF_StepOwner"] = string.Empty;
            return (string)ViewState["WF_StepOwner"];
        }
        set { ViewState["WF_StepOwner"] = value; }
    }

    public int WorkflowEventTypeId
    {
        get
        {
            if (ViewState["WorkflowEventTypeId"] == null) ViewState["WorkflowEventTypeId"] = 0;
            return (int)ViewState["WorkflowEventTypeId"];
        }
        set { ViewState["WorkflowEventTypeId"] = value; }
    }

    // TODO: EDV We shiouldnt need this
    public int RegistrationIdSelected
    {
        get
        {
            if (ViewState["RegistrationIdSelected"] == null) ViewState["RegistrationIdSelected"] = 0;
            return Convert.ToInt32(ViewState["RegistrationIdSelected"]);
        }
        set { ViewState["RegistrationIdSelected"] = value; }
    }

    public new int RegistrationId
    {
        get
        {
            if (ViewState["RegistrationId"] == null) ViewState["RegistrationId"] = 0;
            return Convert.ToInt32(ViewState["RegistrationId"]);
        }
        set { ViewState["RegistrationId"] = value; }
    }

    public bool IsWaiverServiceProvider
    {
        get
        {
            if (ViewState["IsWaiverServiceProvider"] == null) ViewState["IsWaiverServiceProvider"] = false;
            return (bool)ViewState["IsWaiverServiceProvider"];
        }
        set
        {
            ViewState["IsWaiverServiceProvider"] = value;
        }
    }

    public bool IsProviderReactivation
    {
        get
        {
            if (ViewState["IsProviderReactivation"] == null) ViewState["IsProviderReactivation"] = false;
            return (bool)ViewState["IsProviderReactivation"];
        }
        set
        {
            ViewState["IsProviderReactivation"] = value;
        }
    }

    public bool IsReapplication
    {
        get
        {
            if (ViewState["IsReapplication"] == null) ViewState["IsReapplication"] = false;
            return (bool)ViewState["IsReapplication"];
        }
        set
        {
            ViewState["IsReapplication"] = value;
        }
    }

    public int ReferralTypeID
    {
        get
        {
            if (ViewState["ReferralTypeID"] == null) ViewState["ReferralTypeID"] = 0;
            return (int)ViewState["ReferralTypeID"];
        }
        set { ViewState["ReferralTypeID"] = value; }
    }

    public int EntityTypeID
    {
        get
        {
            if (ViewState["EntityTypeID"] == null) ViewState["EntityTypeID"] = 0;
            return (int)ViewState["EntityTypeID"];
        }
        set { ViewState["EntityTypeID"] = value; }
    }

    public int DIDDReferralId
    {
        get
        {
            if (ViewState["DIDDReferralId"] == null) ViewState["DIDDReferralId"] = 0;
            return Convert.ToInt32(ViewState["DIDDReferralId"]);
        }
        set { ViewState["DIDDReferralId"] = value; }
    }

    public int ApplicationTypeID
    {
        get
        {
            if (ViewState["ApplicationTypeID"] == null) ViewState["ApplicationTypeID"] = 0;
            return (int)ViewState["ApplicationTypeID"];
        }
        set { ViewState["ApplicationTypeID"] = value; }
    }

    public int ProviderTypeID
    {
        get
        {
            if (ViewState["ProviderTypeID"] == null) ViewState["ProviderTypeID"] = 0;
            return (int)ViewState["ProviderTypeID"];
        }
        set { ViewState["ProviderTypeID"] = value; }
    }
    public string MMISProviderTypeID
    {
        get
        {
            if (ViewState["MMISProviderTypeID"] == null) ViewState["MMISProviderTypeID"] = string.Empty;
            return (string)ViewState["MMISProviderTypeID"];
        }
        set { ViewState["MMISProviderTypeID"] = value; }
    }

    public string NPI
    {
        get
        {
            if (ViewState["NPI"] == null) ViewState["NPI"] = string.Empty;
            return (string)ViewState["NPI"];
        }
        set { ViewState["NPI"] = value; }
    }

    #endregion
    public bool HasODMSpecialty
    {
        get
        {
            if (ViewState["HasODMSpecialty"] == null) ViewState["HasODMSpecialty"] = false;
            return (bool)ViewState["HasODMSpecialty"];
        }
        set { ViewState["HasODMSpecialty"] = value; }
    }
    public bool HasActiveODMSpecialty
    {
        get
        {
            if (ViewState["HasActiveODMSpecialty"] == null) ViewState["HasActiveODMSpecialty"] = false;
            return (bool)ViewState["HasActiveODMSpecialty"];
        }
        set { ViewState["HasActiveODMSpecialty"] = value; }
    }
    public bool HasDODDSpecialty
    {
        get
        {
            if (ViewState["HasDODDSpecialty"] == null) ViewState["HasDODDSpecialty"] = false;
            return (bool)ViewState["HasDODDSpecialty"];
        }
        set { ViewState["HasDODDSpecialty"] = value; }
    }
    public bool HasActiveDODDSpecialty
    {
        get
        {
            if (ViewState["HasActiveDODDSpecialty"] == null) ViewState["HasActiveDODDSpecialty"] = false;
            return (bool)ViewState["HasActiveDODDSpecialty"];
        }
        set { ViewState["HasActiveDODDSpecialty"] = value; }
    }
    public bool HasODASpecialty
    {
        get
        {
            if (ViewState["HasODASpecialty"] == null) ViewState["HasODASpecialty"] = false;
            return (bool)ViewState["HasODASpecialty"];
        }
        set { ViewState["HasODASpecialty"] = value; }
    }
    public bool HasActiveODASpecialty
    {
        get
        {
            if (ViewState["HasActiveODASpecialty"] == null) ViewState["HasActiveODASpecialty"] = false;
            return (bool)ViewState["HasActiveODASpecialty"];
        }
        set { ViewState["HasActiveODASpecialty"] = value; }
    }
    public bool IsAddODMorODAMedSvc
    {
        get
        {
            if (ViewState["IsAddODMorODAMedSvc"] == null) ViewState["IsAddODMorODAMedSvc"] = false;
            return (bool)ViewState["IsAddODMorODAMedSvc"];
        }
        set { ViewState["IsAddODMorODAMedSvc"] = value; }
    }

    public bool IsCredentialingProvider
    {
        get
        {
            if (ViewState["IsCredentialingProvider"] == null) ViewState["IsCredentialingProvider"] = false;
            return (bool)ViewState["IsCredentialingProvider"];
        }
        set { ViewState["IsCredentialingProvider"] = value; }
    }
    public int BumpStep
    {
        get
        {
            if (ViewState["BumpStep"] == null) ViewState["BumpStep"] = 0;
            return Convert.ToInt32(ViewState["BumpStep"]);
        }
        set { ViewState["BumpStep"] = value; }
    }

    public int RegistrationSequence
    {
        get
        {
            if (ViewState["RegistrationSequence"] == null) ViewState["RegistrationSequence"] = 0;
            return Convert.ToInt32(ViewState["RegistrationSequence"]);
        }
        set { ViewState["RegistrationSequence"] = value; }
    }

    public int ProviderScreeningID
    {
        get
        {
            if (ViewState["ProviderScreeningID"] == null) ViewState["ProviderScreeningID"] = -1;
            return Convert.ToInt32(ViewState["ProviderScreeningID"]);
        }
        set { ViewState["ProviderScreeningID"] = value; }
    }

    public int OwnerScreeningID
    {
        get
        {
            if (ViewState["OwnerScreeningID"] == null) ViewState["OwnerScreeningID"] = -1;
            return Convert.ToInt32(ViewState["OwnerScreeningID"]);
        }
        set { ViewState["OwnerScreeningID"] = value; }
    }

    public new int RegistrationStep
    {
        get
        {
            if (ViewState["RegistrationStep"] == null)
                ViewState["RegistrationStep"] = MAXIMUS.Core.Libraries.Constants.RegistrationPageType.Identification;
            return (int)ViewState["RegistrationStep"];

        }
        set { ViewState["RegistrationStep"] = value; }
    }

    // TODO: EDV Do we need this variable at all??? Cant we jsut invalidate agreements in teh database and that would do it?
    public bool InvalidateAgreements
    {
        get
        {
            if (ViewState["InvalidateAgreements"] == null) ViewState["InvalidateAgreements"] = false;
            return (bool)ViewState["InvalidateAgreements"];
        }
        set { ViewState["InvalidateAgreements"] = value; }
    }

    public Dictionary<int, RegistrationNode> RegistrationNodes
    {
        get
        {
            if (ViewState["RegistrationNodes"] == null) ViewState["RegistrationNodes"] = new Dictionary<int, RegistrationNode>();
            return ViewState["RegistrationNodes"] as Dictionary<int, RegistrationNode>;
        }
        set { ViewState["RegistrationNodes"] = value; }
    }

    public int RegAffiliationID
    {
        get
        {
            if (ViewState["RegAffiliationID"] == null) ViewState["RegAffiliationID"] = 0;
            return Convert.ToInt32(ViewState["RegAffiliationID"]);
        }
        set { ViewState["RegAffiliationID"] = value; }
    }

    public int RegOwnerID
    {
        get
        {
            if (ViewState["RegOwnerID"] == null) ViewState["RegOwnerID"] = 0;
            return Convert.ToInt32(ViewState["RegOwnerID"]);
        }
        set { ViewState["RegOwnerID"] = value; }
    }
    public int ActiveScreeningActivityID
    {
        get
        {
            if (ViewState["ActiveScreeningActivityID"] == null) ViewState["ActiveScreeningActivityID"] = 0;
            return Convert.ToInt32(ViewState["ActiveScreeningActivityID"]);
        }
        set { ViewState["ActiveScreeningActivityID"] = value; }
    }
    public int ActiveScreeningID
    {
        get
        {
            if (ViewState["ActiveScreeningID"] == null) ViewState["ActiveScreeningID"] = 0;
            return Convert.ToInt32(ViewState["ActiveScreeningID"]);
        }
        set { ViewState["ActiveScreeningID"] = value; }
    }

    // TODO: EDV Do we need the owners list in view state? Cant we get them as required?
    public DataTable RegistrationOwnersList
    {
        get
        {
            if (ViewState["RegistrationOwnersList"] == null) ViewState["RegistrationOwnersList"] = new DataTable();
            return (DataTable)ViewState["RegistrationOwnersList"];
        }
        set { ViewState["RegistrationOwnersList"] = value; }
    }
	
	// this is a similiar list to the list above; except it preserves loading the data via the SPC that grabs the address from REG_ADDRESS instead of from REG_OWNER
	public DataTable RegistrationOwnerAddressList
    {
        get
        {
            if (ViewState["RegistrationOwnerAddressList"] == null) ViewState["RegistrationOwnerAddressList"] = new DataTable();
            return (DataTable)ViewState["RegistrationOwnerAddressList"];
        }
        set { ViewState["RegistrationOwnerAddressList"] = value; }
    }

    public DataTable RealEstateOwnersList
    {
        get
        {
            if (ViewState["RealEstateOwnersList"] == null) ViewState["RealEstateOwnersList"] = new DataTable();
            return (DataTable)ViewState["RealEstateOwnersList"];
        }
        set { ViewState["RealEstateOwnersList"] = value; }
    }

    public DataTable AdditionalDisclosureList
    {
        get
        {
            if (ViewState["AdditionalDisclosureList"] == null) ViewState["AdditionalDisclosureList"] = new DataTable();
            return (DataTable)ViewState["AdditionalDisclosureList"];
        }
        set { ViewState["AdditionalDisclosureList"] = value; }
    }
    public string WorkingState
    {
        get
        {
            if (ViewState["WorkingState"] == null) ViewState["WorkingState"] = string.Empty;
            return (string)ViewState["WorkingState"];
        }
        set { ViewState["WorkingState"] = value; }
    }
    public string MyQueueSelectedRoleName
    {
        get
        {
            if (ViewState["MyQueueSelectedRoleName"] == null) ViewState["MyQueueSelectedRoleName"] = string.Empty;
            return (string)ViewState["MyQueueSelectedRoleName"];
        }
        set { ViewState["MyQueueSelectedRoleName"] = value; }
    }

    public string MyQueueSelectedRoleValue
    {
        get
        {
            if (ViewState["MyQueueSelectedRoleValue"] == null) ViewState["MyQueueSelectedRoleValue"] = string.Empty;
            return (string)ViewState["MyQueueSelectedRoleValue"];
        }
        set { ViewState["MyQueueSelectedRoleValue"] = value; }
    }
    public Dictionary<int, SendAttachment> HospicAttachments
    {
        get
        {
            if (ViewState["HospicAttachments"] == null) ViewState["HospicAttachments"] = new Dictionary<int, SendAttachment>();
            return (Dictionary<int, SendAttachment>)ViewState["HospicAttachments"];
        }
        set { ViewState["HospicAttachments"] = value; }
    }
    public DataTable HospicDocuments
    {
        get
        {
            if (ViewState["HospicDocuments"] == null) ViewState["HospicDocuments"] = null;
            return (DataTable)ViewState["HospicDocuments"];
        }
        set { ViewState["HospicDocuments"] = value; }
    }

    public DataTable HospiceBenifitSegmentIndicatorType
    {
        get
        {
            if (ViewState["HospiceBenifitSegmentIndicatorType"] == null) ViewState["HospiceBenifitSegmentIndicatorType"] = null;
            return (DataTable)ViewState["HospiceBenifitSegmentIndicatorType"];
        }
        set { ViewState["HospiceBenifitSegmentIndicatorType"] = value; }
    }
    public bool IsSaved
    { get
        {
            if (ViewState["IsSaved"] == null) return false;
            return Convert.ToBoolean(ViewState["IsSaved"]);
        }
        set { ViewState["IsSaved"] = value; }
    }
    public string ClaimRecipientId
    {
        get
        {
            if (ViewState["ClaimRecipientId"] == null) ViewState["ClaimRecipientId"] = null;
            return (string)ViewState["ClaimRecipientId"];
        }
        set { ViewState["ClaimRecipientId"] = value; }
    }
    public Dictionary<string,string> RecipientInfo
    {
        get
        {
            if (ViewState["RecipientInfo"] == null) ViewState["RecipientInfo"] = null;
            return (Dictionary<string,string>)ViewState["RecipientInfo"];
        }
        set { ViewState["RecipientInfo"] = value; }
    }
    protected virtual void Page_PreRender(object sender, EventArgs e)
    {
    }

    public virtual bool OnTaskExit(string actionName)
    {
        return true;
    }
    public DataSet OtherPayerSequenceTable
    {
        get
        {
            if (ViewState["OtherPayerSequenceTable"] == null) ViewState["OtherPayerSequenceTable"] = null;
            return (DataSet)ViewState["OtherPayerSequenceTable"];
        }
        set { ViewState["OtherPayerSequenceTable"] = value; }
    }
    public DataSet NoteReferenceCodeTable
    {
        get
        {
            if (ViewState["NoteReferenceCodeTable"] == null) ViewState["NoteReferenceCodeTable"] = null;
            return (DataSet)ViewState["NoteReferenceCodeTable"];
        }
        set { ViewState["NoteReferenceCodeTable"] = value; }
    }
    public DataSet ToothSurfaceNumber
    {
        get
        {
            if (ViewState["ToothSurfaceNumber"] == null) ViewState["ToothSurfaceNumber"] = null;
            return (DataSet)ViewState["ToothSurfaceNumber"];
        }
        set { ViewState["ToothSurfaceNumber"] = value; }
    }
    public DataSet ClaimFilingIndicator
    {
        get
        {
            if (ViewState["ClaimFilingIndicator"] == null) ViewState["ClaimFilingIndicator"] = null;
            return (DataSet)ViewState["ClaimFilingIndicator"];
        }
        set { ViewState["ClaimFilingIndicator"] = value; }
    }
    public DataSet PatientRelationShipSubscriber
    {
        get
        {
            if (ViewState["PatientRelationShipSubscriber"] == null) ViewState["PatientRelationShipSubscriber"] = null;
            return (DataSet)ViewState["PatientRelationShipSubscriber"];
        }
        set { ViewState["PatientRelationShipSubscriber"] = value; }
    }
    public DataSet NDCunitOfMeasure
    {
        get
        {
            if (ViewState["NDCunitOfMeasure"] == null) ViewState["NDCunitOfMeasure"] = null;
            return (DataSet)ViewState["NDCunitOfMeasure"];
        }
        set { ViewState["NDCunitOfMeasure"] = value; }
    }
	
    // OHPNM-7214 - return true if this workflow is just for updating the CPC Contact Info
    public bool IsUpdateCPCContactOnly
    {
        get
        {
            if (ProcessParameter != null && ProcessParameter.ContainsKey(CON.ProcessParameter.UpdateCpcContact))                
            {
                if (ProcessParameter[CON.ProcessParameter.UpdateCpcContact] != null && (ProcessParameter[CON.ProcessParameter.UpdateCpcContact] == "true" || ProcessParameter[CON.ProcessParameter.UpdateCpcContact] == "True"))
                return true;
            }

            return false;
        }
    }
	
    public int WaiverServiceUpdateTypeID
    {
        get
        {
            if (ViewState["WaiverServiceUpdateTypeID"] == null) ViewState["WaiverServiceUpdateTypeID"] = 0;
            return (int)ViewState["WaiverServiceUpdateTypeID"];
        }
        set { ViewState["WaiverServiceUpdateTypeID"] = value; }
    }
    public int WaiverTypeID
    {
        get
        {
            if (ViewState["WaiverTypeID"] == null) ViewState["WaiverTypeID"] = 0;
            return (int)ViewState["WaiverTypeID"];
        }
        set { ViewState["WaiverTypeID"] = value; }
    }
	
    public bool BillingPageIsReadOnly
    {
        get
        {
            return ViewState["BillingPageIsReadOnly"] == null ? false : Convert.ToBoolean(ViewState["BillingPageIsReadOnly"].ToString());
        }
        set { ViewState["BillingPageIsReadOnly"] = value; }
    }
	
    public bool OwnerInfoPageIsReadOnly
    {
        get
        {
            return ViewState["OwnerInfoPageIsReadOnly"] == null ? false : Convert.ToBoolean(ViewState["OwnerInfoPageIsReadOnly"].ToString());
        }
        set { ViewState["OwnerInfoPageIsReadOnly"] = value; }
    }
	
    public bool IsReactivation
    {
        get
        {
            if (ViewState["IsReactivation"] == null) ViewState["IsReactivation"] = false;
            return (bool)ViewState["IsReactivation"];
        }
        set { ViewState["IsReactivation"] = value; }
    }

    public bool IsSuspendedProvider
    {
        get
        {
            if (ViewState["IsSuspendedProvider"] == null) ViewState["IsSuspendedProvider"] = false;
            return (bool)ViewState["IsSuspendedProvider"];
        }
        set { ViewState["IsSuspendedProvider"] = value; }
    }
    public int SiteVisitAttemptID
    {
        get
        {
            if (ViewState["SiteVisitAttemptID"] == null) ViewState["SiteVisitAttemptID"] = 0;
            return (int)ViewState["SiteVisitAttemptID"];
        }
        set { ViewState["SiteVisitAttemptID"] = value; }
    }
    public int SiteVisitScreeningID
    {
        get
        {
            if (ViewState["SiteVisitScreeningID"] == null) ViewState["SiteVisitScreeningID"] = 0;
            return (int)ViewState["SiteVisitScreeningID"];
        }
        set { ViewState["SiteVisitScreeningID"] = value; }
    }

    protected void SetBreadcrumb(string html)
    {
        var lit = Master.FindControl("litBreadcrumb") as Literal;

        if (lit != null)
        {
            lit.Text = html;
        }
    }

}
