using Corp.Core.Libraries.AttachmentServiceReference;
using Corp.Core.Libraries.HospiceReference;
using MAXIMUS.Controllers.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

public partial class PopupControls_HospiceAttachment : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.WorkflowPage.HospiceRequestResponse != null)
        {
            BindGrid();
        }
    }

    public void BindGrid()
    {
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
            && inquireResponse.Payload != null
            && inquireResponse.Payload.Attachments != null
            && inquireResponse.Payload.Attachments.Count() > 0 && this.WorkflowPage.HospicDocuments == null)
        {
            var dt = CreateDocumentDataTable();
            var dtDocumentTypes = GetSelectHospiceDocumentType().Tables[0];
            foreach (var att in inquireResponse.Payload.Attachments)
            {
                DataRow destRow = dt.NewRow();
                destRow["LineItem"] = att.BenPeriod;
                var ds = RegistrationController.GetDocumentIndex(att.AttachID);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    destRow["DocumentID"] = ds.Tables[0].Rows[0]["DocumentID"];
                }
                var attachType = dtDocumentTypes.AsEnumerable().Where(x => x.Field<string>("DOCUMENT_TYPE_CODE") == att.AttachType).Select(y => y.Field<string>("DOCUMENT_TYPE_DESC")).FirstOrDefault();
                destRow["AttachType"] = attachType;
                destRow["IsFromInquiry"] = true;
                dt.Rows.Add(destRow);
            }
            this.WorkflowPage.HospicDocuments = dt;
        }
        if (this.WorkflowPage.HospicDocuments != null && this.WorkflowPage.HospicDocuments.Rows.Count > 0)
        {
            gvHospiceAttachment.DataSource = this.WorkflowPage.HospicDocuments;
            gvHospiceAttachment.DataBind();
        }
        else
        {
            var sources = new List<HospiceRequestResponsePayloadAttachments>
            {
               new HospiceRequestResponsePayloadAttachments()
            };
            GridviewShowNoResultFound<HospiceRequestResponsePayloadAttachments>(sources, gvHospiceAttachment);
        }
        BindBenefitLinenoOnFooter();
        BindHospiceDocumentTypeFooter();
    }
    private void BindBenefitLinenoOnFooter()
    {
        DropDownList fddlBenefitLineNo = gvHospiceAttachment.FooterRow.FindControl("fddlBenefitLineNo") as DropDownList;
        LoadBenefitLinenoOnFooter(fddlBenefitLineNo);
    }
    private void LoadBenefitLinenoOnFooter(DropDownList ddlType)
    {
        Helper.LoadDropDown(ddlType, this.WorkflowPage.GetHospiceBenifitLineNo(), "LineNo", "BenefitPeriod", true);
    }
    private void BindHospiceDocumentTypeFooter()
    {
        DropDownList fddlDocumentType = gvHospiceAttachment.FooterRow.FindControl("fddlDocumentType") as DropDownList;
        LoadHospiceDocumentTypeDropDown(fddlDocumentType);
    }
    private void LoadHospiceDocumentTypeDropDown(DropDownList ddlType)
    {
        Helper.LoadDropDown(ddlType, GetSelectHospiceDocumentType().Tables[0], "DOCUMENT_TYPE_DESC", "DOCUMENT_TYPE_CODE", true);
    }


    protected void fbtnAdd_Click(object sender, EventArgs e)
    {
        DropDownList fddlBenefitLineNo = gvHospiceAttachment.FooterRow.FindControl("fddlBenefitLineNo") as DropDownList;
        DropDownList fddlDocumentType = gvHospiceAttachment.FooterRow.FindControl("fddlDocumentType") as DropDownList;
        FileUpload fuAttachmentUpload = gvHospiceAttachment.FooterRow.FindControl("fuAttachmentUpload") as FileUpload;
        HiddenField hdnBenefitLineNo = (HiddenField)gvHospiceAttachment.FooterRow.FindControl("hdnBenefitLineNo");
        var fileBytes = fuAttachmentUpload.FileBytes;
        var docType = fddlDocumentType.SelectedValue;
        AttachmentErrorMessage.InnerText = "";
        if (fuAttachmentUpload.PostedFile.ContentLength > 10364325)
        {
            AttachmentErrorMessage.InnerText = "File size exceeds maximum limit 10 MB";
            return;
        }
        if (this.WorkflowPage.HospiceRequestResponse != null
          && this.WorkflowPage.HospiceRequestResponse.Payload != null
          && this.WorkflowPage.HospiceRequestResponse.Payload.Attachments != null
          && this.WorkflowPage.HospiceRequestResponse.Payload.Attachments.Count() > 0)
        {
            var attachmentExists = this.WorkflowPage.HospiceRequestResponse.Payload.Attachments.ToList().Where(x => x.BenPeriod == Convert.ToInt32(fddlBenefitLineNo.SelectedItem.Text)
            && x.AttachType == fddlDocumentType.SelectedItem.Value).FirstOrDefault();
            if (attachmentExists != null)
            {
                AttachmentErrorMessage.InnerText = "The attachements of same document type already uploaded for benefit line number. Please select different type.";
                return;
            }
        }
        //var note = txtNote.Text.Trim();
        PDMSService.PDMSServiceClient client = new PDMSService.PDMSServiceClient();
        // Db call to store document and it will return docId
        int docID = client.InsertRegDocument(this.WorkflowPage.RegistrationId, 0, null,
                       Path.GetFileNameWithoutExtension(fuAttachmentUpload.FileName), string.Empty, Path.GetFileName(fuAttachmentUpload.FileName)
                       , Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, null);
        SendAttachment attachment = new SendAttachment
        {
            DocumentType = docType,
            DocumentName = Path.GetFileName(fuAttachmentUpload.FileName),
            DocumentExtension = Path.GetExtension(fuAttachmentUpload.FileName).Replace(@".", ""),
            AttachmentData64Binary = fileBytes,
            BenPeriod = Convert.ToInt32(fddlBenefitLineNo.SelectedItem.Text)
            //Identifiers = new SendAttachmentData // I will assign this property in sumbmit button click event when final response comes
        };
        // I am maintaing this attachment data in viewstate until send attachment to SI.
        // I created one dictionary variable  like Dictionary<int, List<SendAttachment>>. here key is docID.
        // Here i am adding docid and attachment data to dictionary variable.
        if (this.WorkflowPage.HospicAttachments != null)
        {
            var vsAttachments = this.WorkflowPage.HospicAttachments;
            if (vsAttachments.ContainsKey(docID))
            {
                vsAttachments[docID] = attachment;
            }
            else
            {
                vsAttachments.Add(docID, attachment);
                this.WorkflowPage.HospicAttachments = vsAttachments;
            }
        }
        else
        {
            Dictionary<int, SendAttachment> dicAttachments = new Dictionary<int, SendAttachment>();
            dicAttachments.Add(docID, attachment);
            this.WorkflowPage.HospicAttachments = dicAttachments;
        }

        var dt = this.WorkflowPage.HospicDocuments;
        if (dt == null) dt = CreateDocumentDataTable();
        var dr = dt.NewRow();
        dr["LineItem"] = fddlBenefitLineNo.SelectedItem; //dt.Rows.Count + 1;
        dr["DocumentID"] = docID;
        dr["AttachType"] = fddlDocumentType.SelectedItem;
        dr["Note"] = string.Empty;
        dr["IsFromInquiry"] = false;
        dt.Rows.Add(dr);
        this.WorkflowPage.HospicDocuments = dt;

        HospiceRequestResponsePayloadAttachments payloadAttachments = new HospiceRequestResponsePayloadAttachments
        {
            AttachType = fddlDocumentType.SelectedItem.Value,
            AttachDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")),
            AttachDateSpecified = true,
            BenPeriod = Convert.ToInt32(fddlBenefitLineNo.SelectedItem.Text),
        };
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
           && inquireResponse.Payload != null
           && inquireResponse.Payload.Attachments != null)
        {
            var attachments = this.WorkflowPage.HospiceRequestResponse.Payload.Attachments.ToList();
            attachments.Add(payloadAttachments);
            this.WorkflowPage.HospiceRequestResponse.Payload.Attachments = attachments.ToArray();
        }
        else
        {
            var attachments = new List<HospiceRequestResponsePayloadAttachments>();
            attachments.Add(payloadAttachments);
            this.WorkflowPage.HospiceRequestResponse.Payload.Attachments = attachments.ToArray();
        }

        BindGrid();
    }
    private DataTable CreateDocumentDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("LineItem");
        dt.Columns.Add("DocumentID");
        dt.Columns.Add("AttachType");
        dt.Columns.Add("Note");
        dt.Columns.Add("IsFromInquiry");
        return dt;
    }
    protected void gvHospiceAttachment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHospiceAttachment.PageIndex = e.NewPageIndex;
        BindGrid();
        gvHospiceAttachment.EditIndex = -1;
    }

    protected void gvHospiceAttachment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var dt = this.WorkflowPage.HospicDocuments;
        var documentId = dt.Rows[e.RowIndex]["DocumentId"];

        var attachments = this.WorkflowPage.HospiceRequestResponse.Payload.Attachments.ToList();

        //Delete same document Id from attachment View state 
        if (this.WorkflowPage.HospicAttachments != null && this.WorkflowPage.HospicAttachments.ContainsKey(Convert.ToInt32(documentId)))
        {
            this.WorkflowPage.HospicAttachments.Remove(Convert.ToInt32(documentId));
        }

        //Delete from Viewstate
        dt.Rows[e.RowIndex].Delete();
        //Delete Document from Document Table
        ProviderController.DeleteDocument(Convert.ToInt32(documentId));

        //Delete from hospice attachments
        attachments.Remove(attachments[e.RowIndex]);
        BindGrid();
    }

    protected void gvHospiceAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Label lblBenefitLineNo = ((Label)e.Row.FindControl("lblBenefitLineNo"));
            if (lblBenefitLineNo != null && lblBenefitLineNo.Text != "0" && lblBenefitLineNo.Text != "")
            {
                var benPeriod = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(lblBenefitLineNo.Text)).Select(y => this.WorkflowPage.GetSegmentIndicatorByValue(y.BenPeriodType.ToString())
                                           + ";" + y.BenPeriodEffDate.ToString("MM/dd/yyyy")
                                           + "-" + y.BenPeriodEndDate.ToString("MM/dd/yyyy")).FirstOrDefault();
                Label lblBenefitPeriodType = ((Label)e.Row.FindControl("lblBenefitPeriodType"));
                Label lblBenefitPeriod = ((Label)e.Row.FindControl("lblBenefitPeriod"));
                if (!string.IsNullOrEmpty(benPeriod))
                {
                    var sigmentIndicatorText = benPeriod.Split(';');
                    lblBenefitPeriodType.Text = sigmentIndicatorText[0];
                    lblBenefitPeriod.Text = sigmentIndicatorText[1];
                }
            }
        }
    }
    protected void gvHospiceAttachment_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Label lblAttachType = (Label)gvHospiceAttachment.Rows[e.NewEditIndex].FindControl("lblAttachType");
        Label lblBenefitLineNo = (Label)gvHospiceAttachment.Rows[e.NewEditIndex].FindControl("lblBenefitLineNo");

        gvHospiceAttachment.EditIndex = e.NewEditIndex;
        BindGrid();

        ////find the Country DropDownList of EditItemTemplate
        DropDownList eddlDocumentType = gvHospiceAttachment.Rows[gvHospiceAttachment.EditIndex].FindControl("eddlDocumentType") as DropDownList;

        //// assigning the Country DataTable to DropDownList
        LoadHospiceDocumentTypeDropDown(eddlDocumentType);

        if (eddlDocumentType.Items.FindByText(lblAttachType.Text) != null)
        {
            eddlDocumentType.Items.FindByText(lblAttachType.Text).Selected = true;
        }

        DropDownList eddlBenefitLineNo = (DropDownList)gvHospiceAttachment.Rows[gvHospiceAttachment.EditIndex].FindControl("eddlBenefitLineNo");
        LoadBenefitLinenoOnFooter(eddlBenefitLineNo);
        HiddenField hdnDocumentType = (HiddenField)gvHospiceAttachment.Rows[gvHospiceAttachment.EditIndex].FindControl("hdnDocumentType");
        var dtDocumentTypes = GetSelectHospiceDocumentType().Tables[0];

        var attachType = dtDocumentTypes.AsEnumerable().Where(x => x.Field<string>("DOCUMENT_TYPE_DESC") == hdnDocumentType.Value).Select(y => y.Field<string>("DOCUMENT_TYPE_CODE")).FirstOrDefault();
        this.WorkflowPage.PreviousSelectedHospiceAttachmentDocType = attachType;
        if (eddlBenefitLineNo.Items.FindByText(lblBenefitLineNo.Text) != null)
        {
            eddlBenefitLineNo.Items.FindByText(lblBenefitLineNo.Text).Selected = true;
            eddlBenefitLineNo.Enabled = false;
            var benPeriod = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(lblBenefitLineNo.Text)).Select(y => this.WorkflowPage.GetSegmentIndicatorByValue(y.BenPeriodType.ToString())
                                          + ";" + y.BenPeriodEffDate.ToString("MM/dd/yyyy")
                                          + "-" + y.BenPeriodEndDate.ToString("MM/dd/yyyy")).FirstOrDefault();
            Label lblBenefitPeriodType = ((Label)gvHospiceAttachment.Rows[e.NewEditIndex].FindControl("elblSegmentBenefitType"));
            Label lblBenefitPeriod = ((Label)gvHospiceAttachment.Rows[e.NewEditIndex].FindControl("elblDateBenefitPeriod"));
            if (!string.IsNullOrEmpty(benPeriod))
            {
                var sigmentIndicatorText = benPeriod.Split(';');
                lblBenefitPeriodType.Text = sigmentIndicatorText[0];
                lblBenefitPeriod.Text = sigmentIndicatorText[1];
            }
        }

    }
    protected void gvHospiceAttachment_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        HiddenField hdnBenefitLineNo = (HiddenField)gvHospiceAttachment.Rows[e.RowIndex].FindControl("hdnBenefitLineNo");
        HiddenField hdnDocumentType = (HiddenField)gvHospiceAttachment.Rows[e.RowIndex].FindControl("hdnDocumentType");
        FileUpload euAttachmentUpload = gvHospiceAttachment.Rows[e.RowIndex].FindControl("euAttachmentUpload") as FileUpload;
        DropDownList eddlDocumentType = gvHospiceAttachment.Rows[e.RowIndex].FindControl("eddlDocumentType") as DropDownList;
        var fileBytes = euAttachmentUpload.FileBytes;
        var docType = hdnDocumentType.Value;
        var dtDocumentTypes = GetSelectHospiceDocumentType().Tables[0];
        var attachTypeId = dtDocumentTypes.AsEnumerable().Where(x => x.Field<string>("DOCUMENT_TYPE_DESC") == docType).Select(y => y.Field<string>("DOCUMENT_TYPE_CODE")).FirstOrDefault();
        AttachmentErrorMessage.InnerText = "";

        //// assigning the Country DataTable to DropDownList
        LoadHospiceDocumentTypeDropDown(eddlDocumentType);

        if (eddlDocumentType.Items.FindByText(docType) != null)
        {
            eddlDocumentType.Items.FindByText(docType).Selected = true;
        }
        DropDownList eddlBenefitLineNo = (DropDownList)gvHospiceAttachment.Rows[gvHospiceAttachment.EditIndex].FindControl("eddlBenefitLineNo");
        LoadBenefitLinenoOnFooter(eddlBenefitLineNo);
        if (eddlBenefitLineNo.Items.FindByText(hdnBenefitLineNo.Value) != null)
        {
            eddlBenefitLineNo.Items.FindByText(hdnBenefitLineNo.Value).Selected = true;
            eddlBenefitLineNo.Enabled = false;
            var benPeriod = this.WorkflowPage.HospiceRequestResponse.Payload.BenefitPeriods.Where(x => x.BenPeriod == Convert.ToInt32(hdnBenefitLineNo.Value)).Select(y => this.WorkflowPage.GetSegmentIndicatorByValue(y.BenPeriodType.ToString())
                                          + ";" + y.BenPeriodEffDate.ToString("MM/dd/yyyy")
                                          + "-" + y.BenPeriodEndDate.ToString("MM/dd/yyyy")).FirstOrDefault();
            Label lblBenefitPeriodType = ((Label)gvHospiceAttachment.Rows[e.RowIndex].FindControl("elblSegmentBenefitType"));
            Label lblBenefitPeriod = ((Label)gvHospiceAttachment.Rows[e.RowIndex].FindControl("elblDateBenefitPeriod"));
            if (!string.IsNullOrEmpty(benPeriod))
            {
                var sigmentIndicatorText = benPeriod.Split(';');
                lblBenefitPeriodType.Text = sigmentIndicatorText[0];
                lblBenefitPeriod.Text = sigmentIndicatorText[1];
            }
        }
        if (euAttachmentUpload.PostedFile.ContentLength > 10364325)
        {
            AttachmentErrorMessage.InnerText = "File size exceeds maximum limit 10 MB";
            return;
        }
        if (this.WorkflowPage.HospiceRequestResponse != null
          && this.WorkflowPage.HospiceRequestResponse.Payload != null
          && this.WorkflowPage.HospiceRequestResponse.Payload.Attachments != null
          && this.WorkflowPage.HospiceRequestResponse.Payload.Attachments.Count() > 0 && attachTypeId != this.WorkflowPage.PreviousSelectedHospiceAttachmentDocType)
        {
            var attachmentExists = this.WorkflowPage.HospiceRequestResponse.Payload.Attachments.ToList().Where(x => x.BenPeriod == Convert.ToInt32(hdnBenefitLineNo.Value)
            && x.AttachType == attachTypeId).FirstOrDefault();
            if (attachmentExists != null)
            {
                AttachmentErrorMessage.InnerText = "The attachements of same document type already uploaded for benefit line number. Please select different type.";
                return;
            }
        }
        //var note = txtNote.Text.Trim();
        PDMSService.PDMSServiceClient client = new PDMSService.PDMSServiceClient();
        // Db call to store document and it will return docId
        int docID = client.InsertRegDocument(this.WorkflowPage.RegistrationId, 0, null,
                       Path.GetFileNameWithoutExtension(euAttachmentUpload.FileName), string.Empty, Path.GetFileName(euAttachmentUpload.FileName)
                       , Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, null);
        SendAttachment attachment = new SendAttachment
        {
            DocumentType = attachTypeId,
            DocumentName = Path.GetFileName(euAttachmentUpload.FileName),
            DocumentExtension = Path.GetExtension(euAttachmentUpload.FileName).Replace(@".", ""),
            AttachmentData64Binary = fileBytes,
            BenPeriod = Convert.ToInt32(hdnBenefitLineNo.Value)
        };
        int preDocId = 0;
        var dt = this.WorkflowPage.HospicDocuments;
        var dr = dt.Rows[e.RowIndex];
        preDocId = Convert.ToInt32(dr.Field<object>("DocumentID"));
        dr["LineItem"] = hdnBenefitLineNo.Value;
        dr["DocumentID"] = docID;
        dr["AttachType"] = hdnDocumentType.Value;
        dr["Note"] = string.Empty;
        dr["IsFromInquiry"] = false;
        this.WorkflowPage.HospicDocuments = dt;
        if (this.WorkflowPage.HospicAttachments != null)
        {
            var vsAttachments = this.WorkflowPage.HospicAttachments;

            if (vsAttachments.ContainsKey(preDocId))
            {
                vsAttachments.Remove(preDocId);
            }

            vsAttachments.Add(docID, attachment);
            this.WorkflowPage.HospicAttachments = vsAttachments;

        }

        HospiceRequestResponsePayloadAttachments payloadAttachments = new HospiceRequestResponsePayloadAttachments
        {
            AttachType = attachTypeId,
            AttachDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")),
            BenPeriod = Convert.ToInt32(hdnBenefitLineNo.Value)
        };
        var inquireResponse = this.WorkflowPage.HospiceRequestResponse;
        if (inquireResponse != null
           && inquireResponse.Payload != null
           && inquireResponse.Payload.Attachments != null)
        {
            var attachments = this.WorkflowPage.HospiceRequestResponse.Payload.Attachments.ToList();
            attachments[e.RowIndex].AttachType = attachTypeId;
            attachments[e.RowIndex].AttachDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            attachments[e.RowIndex].BenPeriod = Convert.ToInt32(hdnBenefitLineNo.Value);
            this.WorkflowPage.HospiceRequestResponse.Payload.Attachments = attachments.ToArray();
        }
        ProviderController.DeleteDocument(preDocId);
        gvHospiceAttachment.EditIndex = -1;
        BindGrid();
    }
    protected void gvHospiceAttachment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvHospiceAttachment.EditIndex = -1;
        BindGrid();
    }

    private DataSet GetSelectHospiceDocumentType()
    {
        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            return psc.SelectHospiceDocumentType();
        }
    }
}