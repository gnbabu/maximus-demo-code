using System;
using System.Collections.Generic;
using System.Data;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_NursingFacilityAddressHistory : BaseSectionControl
{

	public WorkflowPage WorkflowPage
	{
		get { return (WorkflowPage) this.Page; }
	}

	protected override void OnLoad(EventArgs e)
	{
		LoadControlData();
		base.OnLoad(e);
	}

	public override void LoadControlData()
	{
		LoadData();
	}

	public void LoadData()
	{
		PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
		Dictionary<string, string> parms = new Dictionary<string, string>();
		parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
		parms.Add("ADDRESS_TYPE_ID", CON.SectionTypeID.NursingFacilityAddress.ToString());
		DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_ADDRESS_SECTION_HISTORY", parms);
		if (Helper.HasRows(ds))
		{
			grd.DataSource = ds.Tables[0];
			grd.DataBind();
		}
	}

	public override bool ValidateData()
	{
		return true;
	}

	public override bool SaveData()
	{
		return true;
	}

	public override void LoadData(DataRow row)
	{

	}

	public override string ValidationGroup
	{
		get { return "NursingFacilityHistory"; }
	}

	public override string Title
	{
		get { return "Long Term care History"; }
	}

	public override string IdText
	{
		get { return "ucNursingFacilityHistory_" + this.WorkflowPage.RegistrationId; }
	}
}