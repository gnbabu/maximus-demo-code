using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Services3.Security;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using PdfSharp.Snippets.Font;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using PDMSRestServices.Models;
using System.Linq.Expressions;
using System.Web;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace PDMSRestServices.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[EnableCors("NewPolicy")]
    [Route("[controller]")]
    public class PDFController : ControllerBase
    {

        [HttpPost("GenerateMemberEligibilityPdfJSON")]
        public IActionResult GenerateMemberEligibilityPdfJSON([FromBody] MemberEligibilityPDFRequest pdfReq)
        {
            try
            {

                string fsxPathLogo = AppSettings.Get("ImagesFSXPath") + "ODMLogo.jpg";
                //Search Parameters
                string spMBN = string.Empty;
                string spDOB = string.Empty;
                string spSSN = string.Empty;
                string spFDOS = string.Empty;
                string spTDOS = string.Empty;
                string spPC = string.Empty;
                string spSDT = string.Empty;

                //Recipient Information
                string riMBN = string.Empty;
                string riDOB = string.Empty;
                string riDOD = string.Empty;
                string riLN = string.Empty;
                string riFN = string.Empty;
                string riSSN = string.Empty;
                string riGender = string.Empty;
                string riCOR = string.Empty;
                string riCOE = string.Empty;
                string riCOI = string.Empty;

                //Search Parameters
                if (pdfReq.RecipientSearchParameters.MEmbn != null)
                {
                    spMBN = pdfReq.RecipientSearchParameters.MEmbn;
                }
                if (pdfReq.RecipientSearchParameters.MEdob != null)
                {
                    spDOB = GetDateString(pdfReq.RecipientSearchParameters.MEdob);
                }
                if (pdfReq.RecipientSearchParameters.MEssn != null)
                {
                    spSSN = pdfReq.RecipientSearchParameters.MEssn;
                }
                if (pdfReq.RecipientSearchParameters.MEfdos != null)
                {
                    spFDOS = GetDateString(pdfReq.RecipientSearchParameters.MEfdos);
                }
                if (pdfReq.RecipientSearchParameters.MEtdos != null)
                {
                    spTDOS = GetDateString(pdfReq.RecipientSearchParameters.MEtdos);
                }
                if (pdfReq.RecipientSearchParameters.MEpc != null)
                {
                    spPC = pdfReq.RecipientSearchParameters.MEpc;
                }
                if (pdfReq.RecipientSearchParameters.MEsdt != null)
                {
                    spSDT = pdfReq.RecipientSearchParameters.MEsdt;
                }

                //Recipient Information
                if (pdfReq.RecipientInfo.MedicaidId != null)
                {
                    riMBN = pdfReq.RecipientInfo.MedicaidId;
                }
                if (pdfReq.RecipientInfo.DateOfBirth != null)
                {
                    riDOB = GetDateString(pdfReq.RecipientInfo.DateOfBirth.Split("T")[0]);
                }
                if (pdfReq.RecipientInfo.DateOfDeath != null)
                {
                    riDOD = GetDateString(pdfReq.RecipientInfo.DateOfDeath.Split("T")[0]);
                }
                if (pdfReq.RecipientInfo.LastName != null)
                {
                    riLN = pdfReq.RecipientInfo.LastName;
                }
                if (pdfReq.RecipientInfo.FirstName != null)
                {
                    riFN = pdfReq.RecipientInfo.FirstName;
                }
                if (pdfReq.RecipientInfo.SSN != null)
                {
                    riSSN = pdfReq.RecipientInfo.SSN;
                }
                if (pdfReq.RecipientInfo.Gender != null)
                {
                    riGender = pdfReq.RecipientInfo.Gender;
                }
                if (pdfReq.RecipientInfo.CountyOfResidence != null)
                {
                    riCOR = pdfReq.RecipientInfo.CountyOfResidence;
                }
                if (pdfReq.RecipientInfo.CountyOfEligibility != null)
                {
                    riCOE = pdfReq.RecipientInfo.CountyOfEligibility;
                }

                Document document = new Document();
                Section section = document.AddSection();

                // Put a logo in the header
                Image image = section.Headers.Primary.AddImage(fsxPathLogo);
                image.Height = "1cm";
                image.LockAspectRatio = true;
                image.RelativeVertical = RelativeVertical.Line;
                image.RelativeHorizontal = RelativeHorizontal.Margin;
                image.Top = ShapePosition.Top;
                image.Left = ShapePosition.Left;
                image.WrapFormat.Style = WrapStyle.Through;


                // Create footer
                Paragraph paragraph = section.Footers.Primary.AddParagraph();
                paragraph.AddText(spSDT);
                paragraph.AddText("                              Member Eligibility Verification      Page ");
                paragraph.AddPageField();
                paragraph.AddText(" of ");
                paragraph.AddNumPagesField();
                paragraph.Format.Font.Size = 9;
                paragraph.Format.Alignment = ParagraphAlignment.Center;

                paragraph = section.AddParagraph();
                FormattedText ftext = new FormattedText();
                ftext.Size = 18;
                ftext.AddText("Member Eligibility Verification");
                ftext.Color = Colors.DarkBlue;
                ftext.Bold = true;
                paragraph.Add(ftext);

                document.LastSection.AddParagraph("", "Heading2");
                paragraph = section.AddParagraph();
                FormattedText ftext2 = new FormattedText();
                ftext2.Size = 12;
                ftext2.AddText("Recipient Eligibility Search Parameters");
                ftext2.Color = Colors.DarkBlue;
                ftext2.Bold = true;
                paragraph.Add(ftext2);

                // Create the item table
                Table table = section.AddTable();
                table.Style = "Table";
                table.Borders.Color = Color.FromRgb(1, 1, 1);
                table.Borders.Width = 0.25;
                table.Borders.Left.Width = 0.5;
                table.Borders.Right.Width = 0.5;
                table.Rows.LeftIndent = 0;

                // Before you can add a row, you must define the columns
                Column column = table.AddColumn("5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = table.AddColumn("5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                Row row = table.AddRow();
                row.HeadingFormat = true;
                row.Format.Alignment = ParagraphAlignment.Center;
                row.Format.Font.Bold = true;
                row.Format.Font.Size = 8;
                //row.Shading.Color = Color.FromRgb(1, 1, 1);
                row.Cells[0].AddParagraph("Medicaid Billing Number");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[1].AddParagraph(spMBN);
                row.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row = table.AddRow();
                row.HeadingFormat = true;
                row.Format.Alignment = ParagraphAlignment.Center;
                row.Format.Font.Bold = true;
                row.Format.Font.Size = 8;
                row.Cells[0].AddParagraph("Date of Birth");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[1].AddParagraph(spDOB);
                row.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row = table.AddRow();
                row.HeadingFormat = true;
                row.Format.Alignment = ParagraphAlignment.Center;
                row.Format.Font.Bold = true;
                row.Format.Font.Size = 8;
                row.Cells[0].AddParagraph("SSN");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[1].AddParagraph(spSSN);
                row.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row = table.AddRow();
                row.HeadingFormat = true;
                row.Format.Alignment = ParagraphAlignment.Center;
                row.Format.Font.Bold = true;
                row.Format.Font.Size = 8;
                row.Cells[0].AddParagraph("From DOS");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[1].AddParagraph(spFDOS);
                row.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row = table.AddRow();
                row.HeadingFormat = true;
                row.Format.Alignment = ParagraphAlignment.Center;
                row.Format.Font.Bold = true;
                row.Format.Font.Size = 8;
                row.Cells[0].AddParagraph("To DOS");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[1].AddParagraph(spTDOS);
                row.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row = table.AddRow();
                row.HeadingFormat = true;
                row.Format.Alignment = ParagraphAlignment.Center;
                row.Format.Font.Bold = true;
                row.Format.Font.Size = 8;
                row.Cells[0].AddParagraph("Procedure Code");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[1].AddParagraph(spPC);
                row.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                document.LastSection.AddParagraph("", "Heading2");
                paragraph = section.AddParagraph();
                FormattedText ftext3 = new FormattedText();
                ftext3.Size = 12;
                ftext3.AddText("Search Result");
                ftext3.Color = Colors.DarkBlue;
                ftext3.Bold = true;
                paragraph.Add(ftext3);

                paragraph = section.AddParagraph();
                FormattedText ftext4 = new FormattedText();
                ftext4.Size = 12;
                ftext4.AddText("Recipient Information");
                ftext4.Color = Colors.Black;
                ftext4.Bold = true;
                paragraph.Add(ftext4);

                // Create the item table
                Table table2 = section.AddTable();
                table2.Style = "Table";
                table2.Borders.Color = Color.FromRgb(1, 1, 1);
                table2.Borders.Width = 0.25;
                table2.Borders.Left.Width = 0.5;
                table2.Borders.Right.Width = 0.5;
                table2.Rows.LeftIndent = 0;

                // Before you can add a row, you must define the columns
                Column column2 = table2.AddColumn("5cm");
                column2.Format.Alignment = ParagraphAlignment.Center;

                column2 = table2.AddColumn("10cm");
                column2.Format.Alignment = ParagraphAlignment.Center;

                Row row2 = table2.AddRow();
                row2.HeadingFormat = true;
                row2.Format.Alignment = ParagraphAlignment.Center;
                row2.Format.Font.Bold = true;
                row2.Format.Font.Size = 8;
                row2.Cells[0].AddParagraph("Medicaid Billing Number");
                row2.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row2.Cells[1].AddParagraph(riMBN);
                row2.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row2 = table2.AddRow();
                row2.HeadingFormat = true;
                row2.Format.Alignment = ParagraphAlignment.Center;
                row2.Format.Font.Bold = true;
                row2.Format.Font.Size = 8;
                row2.Cells[0].AddParagraph("Date of Birth");
                row2.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row2.Cells[1].AddParagraph(riDOB);
                row2.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row2 = table2.AddRow();
                row2.HeadingFormat = true;
                row2.Format.Alignment = ParagraphAlignment.Center;
                row2.Format.Font.Bold = true;
                row2.Format.Font.Size = 8;
                row2.Cells[0].AddParagraph("Date of Death");
                row2.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row2.Cells[1].AddParagraph(riDOD);
                row2.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row2 = table2.AddRow();
                row2.HeadingFormat = true;
                row2.Format.Alignment = ParagraphAlignment.Center;
                row2.Format.Font.Bold = true;
                row2.Format.Font.Size = 8;
                row2.Cells[0].AddParagraph("Last Name");
                row2.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row2.Cells[1].AddParagraph(riLN);
                row2.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row2 = table2.AddRow();
                row2.HeadingFormat = true;
                row2.Format.Alignment = ParagraphAlignment.Center;
                row2.Format.Font.Bold = true;
                row2.Format.Font.Size = 8;
                row2.Cells[0].AddParagraph("First Name");
                row2.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row2.Cells[1].AddParagraph(riFN);
                row2.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row2 = table2.AddRow();
                row2.HeadingFormat = true;
                row2.Format.Alignment = ParagraphAlignment.Center;
                row2.Format.Font.Bold = true;
                row2.Format.Font.Size = 8;
                row2.Cells[0].AddParagraph("SSN");
                row2.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row2.Cells[1].AddParagraph(riSSN);
                row2.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row2 = table2.AddRow();
                row2.HeadingFormat = true;
                row2.Format.Alignment = ParagraphAlignment.Center;
                row2.Format.Font.Bold = true;
                row2.Format.Font.Size = 8;
                row2.Cells[0].AddParagraph("Gender");
                row2.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row2.Cells[1].AddParagraph(riGender);
                row2.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row2 = table2.AddRow();
                row2.HeadingFormat = true;
                row2.Format.Alignment = ParagraphAlignment.Center;
                row2.Format.Font.Bold = true;
                row2.Format.Font.Size = 8;
                row2.Cells[0].AddParagraph("County of Residence");
                row2.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row2.Cells[1].AddParagraph(riCOR);
                row2.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row2 = table2.AddRow();
                row2.HeadingFormat = true;
                row2.Format.Alignment = ParagraphAlignment.Center;
                row2.Format.Font.Bold = true;
                row2.Format.Font.Size = 8;
                row2.Cells[0].AddParagraph("County of Eligibility");
                row2.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row2.Cells[1].AddParagraph(riCOE);
                row2.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                row2 = table2.AddRow();
                row2.HeadingFormat = true;
                row2.Format.Alignment = ParagraphAlignment.Center;
                row2.Format.Font.Bold = true;
                row2.Format.Font.Size = 8;
                row2.Cells[0].AddParagraph("County Office Information");
                row2.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                row2.Cells[1].AddParagraph("https://jfs.ohio.gov/about/local-agencies-directory");
                row2.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                row2.Cells[1].Format.Font.Color = Colors.DarkBlue;

                try
                {
                    if (pdfReq.BenifitAssignmentPlans.Count > 0)
                    {
                        document.LastSection.AddParagraph("", "Heading2");

                        // Create the item table
                        Table table3 = section.AddTable();
                        table3.Style = "Table";
                        table3.Borders.Color = Color.FromRgb(1, 1, 1);
                        table3.Borders.Width = 0.25;
                        table3.Borders.Left.Width = 0.5;
                        table3.Borders.Right.Width = 0.5;
                        table3.Rows.LeftIndent = 0;

                        // Before you can add a row, you must define the columns
                        Column column3 = table3.AddColumn("5cm");
                        column3.Format.Alignment = ParagraphAlignment.Center;

                        column3 = table3.AddColumn("4cm");
                        column3.Format.Alignment = ParagraphAlignment.Center;

                        column3 = table3.AddColumn("4cm");
                        column3.Format.Alignment = ParagraphAlignment.Center;

                        Row row30 = table3.AddRow();
                        row30.HeadingFormat = true;
                        row30.Format.Alignment = ParagraphAlignment.Center;
                        row30.Format.Font.Bold = true;
                        row30.Format.Font.Size = 12;
                        row30.Cells[0].AddParagraph("Benefit/Assignment Plan(s)");
                        row30.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row30.Cells[0].MergeRight = 2;
                        row30.Borders.Visible = false;

                        Row row3 = table3.AddRow();
                        row3.HeadingFormat = true;
                        row3.Format.Alignment = ParagraphAlignment.Center;
                        row3.Format.Font.Bold = true;
                        row3.Format.Font.Size = 8;
                        row3.Cells[0].AddParagraph("Benefit/Assignment Plan");
                        row3.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row3.Cells[1].AddParagraph("Effective Date");
                        row3.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                        row3.Cells[2].AddParagraph("End Date");
                        row3.Cells[2].Format.Alignment = ParagraphAlignment.Left;

                        string baPlan = string.Empty;
                        string baEffD = string.Empty;
                        string baEndD = string.Empty;

                        for (int i = 0; i < pdfReq.BenifitAssignmentPlans.Count; i++)
                        {
                            if (pdfReq.BenifitAssignmentPlans[i].AssignmentPlan != null)
                            {
                                baPlan = pdfReq.BenifitAssignmentPlans[i].AssignmentPlan;
                            }
                            if (pdfReq.BenifitAssignmentPlans[i].EffectiveDate != null)
                            {
                                baEffD = GetDateString(pdfReq.BenifitAssignmentPlans[i].EffectiveDate.Split("T")[0]);
                            }
                            if (pdfReq.BenifitAssignmentPlans[i].EndDate != null)
                            {
                                baEndD = GetDateString(pdfReq.BenifitAssignmentPlans[i].EndDate.Split("T")[0]);
                            }

                            row3 = table3.AddRow();
                            row3.HeadingFormat = false;
                            row3.Format.Alignment = ParagraphAlignment.Center;
                            row3.Format.Font.Bold = false;
                            row3.Format.Font.Size = 8;
                            row3.Cells[0].AddParagraph(baPlan);
                            row3.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                            row3.Cells[1].AddParagraph(baEffD);
                            row3.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                            row3.Cells[2].AddParagraph(baEndD);
                            row3.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                        }
                    }
                }
                catch (Exception ex)
                { }

                try
                {
                    if (pdfReq.ManagedCarePlans.Count > 0)
                    {
                        document.LastSection.AddParagraph("", "Heading2");

                        // Create the item table
                        Table table4 = section.AddTable();
                        table4.Style = "Table";
                        table4.Borders.Color = Color.FromRgb(1, 1, 1);
                        table4.Borders.Width = 0.25;
                        table4.Borders.Left.Width = 0.5;
                        table4.Borders.Right.Width = 0.5;
                        table4.Rows.LeftIndent = 0;

                        // Before you can add a row, you must define the columns
                        Column column4 = table4.AddColumn("3cm");
                        column4.Format.Font.Size = 8;
                        column4.Format.Alignment = ParagraphAlignment.Center;

                        column4 = table4.AddColumn("2cm");
                        column4.Format.Font.Size = 8;
                        column4.Format.Alignment = ParagraphAlignment.Center;

                        column4 = table4.AddColumn("4cm");
                        column4.Format.Font.Size = 8;
                        column4.Format.Alignment = ParagraphAlignment.Center;

                        column4 = table4.AddColumn("2.2cm");
                        column4.Format.Font.Size = 8;
                        column4.Format.Alignment = ParagraphAlignment.Center;

                        column4 = table4.AddColumn("2.2cm");
                        column4.Format.Font.Size = 8;
                        column4.Format.Alignment = ParagraphAlignment.Center;

                        column4 = table4.AddColumn("4cm");
                        column4.Format.Font.Size = 8;
                        column4.Format.Alignment = ParagraphAlignment.Center;

                        Row row40 = table4.AddRow();
                        row40.HeadingFormat = true;
                        row40.Format.Alignment = ParagraphAlignment.Center;
                        row40.Format.Font.Bold = true;
                        row40.Format.Font.Size = 12;
                        row40.Cells[0].AddParagraph("Managed Care Plans");
                        row40.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row40.Cells[0].MergeRight = 5;
                        row40.Borders.Visible = false;

                        Row row4 = table4.AddRow();
                        row4.HeadingFormat = true;
                        row4.Format.Alignment = ParagraphAlignment.Center;
                        row4.Format.Font.Bold = true;
                        row4.Cells[0].AddParagraph("Plan Name");
                        row4.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row4.Cells[1].AddParagraph("Payer ID");
                        row4.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                        row4.Cells[2].AddParagraph("Plan Description");
                        row4.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                        row4.Cells[3].AddParagraph("Effective Date");
                        row4.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                        row4.Cells[4].AddParagraph("End Date");
                        row4.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                        row4.Cells[5].AddParagraph("Managed Care Benefits");
                        row4.Cells[5].Format.Alignment = ParagraphAlignment.Left;

                        string mcpPN = string.Empty;
                        string mcpPaD = string.Empty;
                        string mcpPlD = string.Empty;
                        string mcpEffD = string.Empty;
                        string mcpEndD = string.Empty;
                        string mcpMCB = string.Empty;

                        for (int i = 0; i < pdfReq.ManagedCarePlans.Count; i++)
                        {
                            if (pdfReq.ManagedCarePlans[i].PlanName != null)
                            {
                                mcpPN = pdfReq.ManagedCarePlans[i].PlanName;
                            }
                            if (pdfReq.ManagedCarePlans[i].PlanId != null)
                            {
                                mcpPaD = pdfReq.ManagedCarePlans[i].PlanId;
                            }
                            if (pdfReq.ManagedCarePlans[i].PlanDescription != null)
                            {
                                mcpPlD = pdfReq.ManagedCarePlans[i].PlanDescription;
                            }
                            if (pdfReq.ManagedCarePlans[i].EffectiveDate != null)
                            {
                                mcpEffD = GetDateString(pdfReq.ManagedCarePlans[i].EffectiveDate.Split("T")[0]);
                            }
                            if (pdfReq.ManagedCarePlans[i].EndDate != null)
                            {
                                mcpEndD = GetDateString(pdfReq.ManagedCarePlans[i].EndDate.Split("T")[0]);
                            }
                            if (pdfReq.ManagedCarePlans[i].ManagedCareBenefits != null)
                            {
                                mcpMCB = pdfReq.ManagedCarePlans[i].ManagedCareBenefits;
                            }

                            row4 = table4.AddRow();
                            row4.HeadingFormat = false;
                            row4.Format.Alignment = ParagraphAlignment.Center;
                            row4.Format.Font.Bold = false;
                            row4.Cells[0].AddParagraph(mcpPN);
                            row4.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                            row4.Cells[1].AddParagraph(mcpPaD);
                            row4.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                            row4.Cells[2].AddParagraph(mcpPlD);
                            row4.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                            row4.Cells[3].AddParagraph(mcpEffD);
                            row4.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                            row4.Cells[4].AddParagraph(mcpEndD);
                            row4.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                            row4.Cells[5].AddParagraph(mcpMCB);
                            row4.Cells[5].Format.Alignment = ParagraphAlignment.Left;
                        }
                    }
                }
                catch (Exception ex)
                { }


                try
                {
                    if (pdfReq.ThirdPartyLiabilities.Count > 0)
                    {
                        document.LastSection.AddParagraph("", "Heading2");

                        // Create the item table
                        Table table5 = section.AddTable();
                        table5.Style = "Table";
                        table5.Borders.Color = Color.FromRgb(1, 1, 1);
                        table5.Borders.Width = 0.25;
                        table5.Borders.Left.Width = 0.5;
                        table5.Borders.Right.Width = 0.5;
                        table5.Rows.LeftIndent = 0;

                        // Before you can add a row, you must define the columns
                        Column column5 = table5.AddColumn("1.5cm");
                        column5.Format.Font.Size = 8;
                        column5.Format.Alignment = ParagraphAlignment.Center;

                        column5 = table5.AddColumn("1.5cm");
                        column5.Format.Font.Size = 8;
                        column5.Format.Alignment = ParagraphAlignment.Center;

                        column5 = table5.AddColumn("2.5cm");
                        column5.Format.Font.Size = 8;
                        column5.Format.Alignment = ParagraphAlignment.Center;

                        column5 = table5.AddColumn("3cm");
                        column5.Format.Font.Size = 8;
                        column5.Format.Alignment = ParagraphAlignment.Center;

                        column5 = table5.AddColumn("2cm");
                        column5.Format.Font.Size = 8;
                        column5.Format.Alignment = ParagraphAlignment.Center;

                        column5 = table5.AddColumn("2cm");
                        column5.Format.Font.Size = 8;
                        column5.Format.Alignment = ParagraphAlignment.Center;

                        column5 = table5.AddColumn("2cm");
                        column5.Format.Font.Size = 8;
                        column5.Format.Alignment = ParagraphAlignment.Center;

                        column5 = table5.AddColumn("2cm");
                        column5.Format.Font.Size = 8;
                        column5.Format.Alignment = ParagraphAlignment.Center;

                        column5 = table5.AddColumn("1.7cm");
                        column5.Format.Font.Size = 8;
                        column5.Format.Alignment = ParagraphAlignment.Center;

                        Row row50 = table5.AddRow();
                        row50.HeadingFormat = true;
                        row50.Format.Alignment = ParagraphAlignment.Center;
                        row50.Format.Font.Bold = true;
                        row50.Format.Font.Size = 12;
                        row50.Cells[0].AddParagraph("Third Party Liability");
                        row50.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row50.Cells[0].MergeRight = 8;
                        row50.Borders.Visible = false;

                        Row row5 = table5.AddRow();
                        row5.HeadingFormat = true;
                        row5.Format.Alignment = ParagraphAlignment.Center;
                        row5.Format.Font.Bold = true;
                        row5.Format.Font.Size = 8;
                        row5.Cells[0].AddParagraph("Carrier Name");
                        row5.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row5.Cells[1].AddParagraph("Carrier Number");
                        row5.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                        row5.Cells[2].AddParagraph("Policy Number");
                        row5.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                        row5.Cells[3].AddParagraph("Policy Holder");
                        row5.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                        row5.Cells[4].AddParagraph("Coverage Type");
                        row5.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                        row5.Cells[5].AddParagraph("Coverage");
                        row5.Cells[5].Format.Alignment = ParagraphAlignment.Left;
                        row5.Cells[6].AddParagraph("Effective Date");
                        row5.Cells[6].Format.Alignment = ParagraphAlignment.Left;
                        row5.Cells[7].AddParagraph("End Date");
                        row5.Cells[7].Format.Alignment = ParagraphAlignment.Left;
                        row5.Cells[8].AddParagraph("Group Number");
                        row5.Cells[8].Format.Alignment = ParagraphAlignment.Left;

                        string MECarrierName = string.Empty;
                        string MECarrierNumber = string.Empty;
                        string MEPolicyNumber = string.Empty;
                        string MEPolicyHolder = string.Empty;
                        string MECoverageType = string.Empty;
                        string MECoverage = string.Empty;
                        string MEEffectiveDate = string.Empty;
                        string MEEndDate = string.Empty;
                        string MEGroupNumber = string.Empty;

                        for (int i = 0; i < pdfReq.ThirdPartyLiabilities.Count; i++)
                        {
                            if (pdfReq.ThirdPartyLiabilities[i].CarrierNumber != null)
                            {
                                MECarrierName = pdfReq.ThirdPartyLiabilities[i].CarrierNumber;
                            }
                            if (pdfReq.ThirdPartyLiabilities[i].CarrierNumber != null)
                            {
                                MECarrierNumber = pdfReq.ThirdPartyLiabilities[i].CarrierNumber;
                            }
                            if (pdfReq.ThirdPartyLiabilities[i].PolicyNumber != null)
                            {
                                MEPolicyNumber = pdfReq.ThirdPartyLiabilities[i].PolicyNumber;
                            }
                            if (pdfReq.ThirdPartyLiabilities[i].PolicyHolder != null)
                            {
                                MEPolicyHolder = pdfReq.ThirdPartyLiabilities[i].PolicyHolder;
                            }
                            if (pdfReq.ThirdPartyLiabilities[i].CoverageType != null)
                            {
                                MECoverageType = pdfReq.ThirdPartyLiabilities[i].CoverageType;
                            }
                            if (pdfReq.ThirdPartyLiabilities[i].Coverage != null)
                            {
                                MECoverage = pdfReq.ThirdPartyLiabilities[i].Coverage;
                            }
                            if (pdfReq.ThirdPartyLiabilities[i].EffectiveDate != null)
                            {
                                MEEffectiveDate = GetDateString(pdfReq.ThirdPartyLiabilities[i].EffectiveDate.Split("T")[0]);
                            }
                            if (pdfReq.ThirdPartyLiabilities[i].EndDate != null)
                            {
                                MEEndDate = GetDateString(pdfReq.ThirdPartyLiabilities[i].EndDate.Split("T")[0]);
                            }
                            if (pdfReq.ThirdPartyLiabilities[i].GroupNumber != null)
                            {
                                MEGroupNumber = pdfReq.ThirdPartyLiabilities[i].GroupNumber;
                            }

                            row5 = table5.AddRow();
                            row5.HeadingFormat = false;
                            row5.Format.Alignment = ParagraphAlignment.Center;
                            row5.Format.Font.Bold = false;
                            row5.Cells[0].AddParagraph(MECarrierName);
                            row5.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                            row5.Cells[1].AddParagraph(MECarrierNumber);
                            row5.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                            row5.Cells[2].AddParagraph(MEPolicyNumber);
                            row5.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                            row5.Cells[3].AddParagraph(MEPolicyHolder);
                            row5.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                            row5.Cells[4].AddParagraph(MECoverageType);
                            row5.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                            row5.Cells[5].AddParagraph(MECoverage);
                            row5.Cells[5].Format.Alignment = ParagraphAlignment.Left;
                            row5.Cells[6].AddParagraph(MEEffectiveDate);
                            row5.Cells[6].Format.Alignment = ParagraphAlignment.Left;
                            row5.Cells[7].AddParagraph(MEEndDate);
                            row5.Cells[7].Format.Alignment = ParagraphAlignment.Left;
                            row5.Cells[8].AddParagraph(MEGroupNumber);
                            row5.Cells[8].Format.Alignment = ParagraphAlignment.Left;
                        }
                    }
                }
                catch (Exception ex)
                { }


                try
                {
                    if (pdfReq.PatientLiabilities.Count > 0)
                    {
                        document.LastSection.AddParagraph("", "Heading2");

                        // Create the item table
                        Table table6 = section.AddTable();
                        table6.Style = "Table";
                        table6.Borders.Color = Color.FromRgb(1, 1, 1);
                        table6.Borders.Width = 0.25;
                        table6.Borders.Left.Width = 0.5;
                        table6.Borders.Right.Width = 0.5;
                        table6.Rows.LeftIndent = 0;

                        // Before you can add a row, you must define the columns
                        Column column6 = table6.AddColumn("3cm");
                        column6.Format.Font.Size = 8;
                        column6.Format.Alignment = ParagraphAlignment.Center;

                        column6 = table6.AddColumn("2cm");
                        column6.Format.Font.Size = 8;
                        column6.Format.Alignment = ParagraphAlignment.Center;

                        column6 = table6.AddColumn("4cm");
                        column6.Format.Font.Size = 8;
                        column6.Format.Alignment = ParagraphAlignment.Center;

                        column6 = table6.AddColumn("2.2cm");
                        column6.Format.Font.Size = 8;
                        column6.Format.Alignment = ParagraphAlignment.Center;

                        column6 = table6.AddColumn("2.2cm");
                        column6.Format.Font.Size = 8;
                        column6.Format.Alignment = ParagraphAlignment.Center;

                        Row row60 = table6.AddRow();
                        row60.HeadingFormat = true;
                        row60.Format.Alignment = ParagraphAlignment.Center;
                        row60.Format.Font.Bold = true;
                        row60.Format.Font.Size = 12;
                        row60.Cells[0].AddParagraph("Patient Liability");
                        row60.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row60.Cells[0].MergeRight = 4;
                        row60.Borders.Visible = false;

                        Row row6 = table6.AddRow();
                        row6.HeadingFormat = true;
                        row6.Format.Alignment = ParagraphAlignment.Center;
                        row6.Format.Font.Bold = true;
                        row6.Cells[0].AddParagraph("Financial Payer");
                        row6.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row6.Cells[1].AddParagraph("Monthly Amount");
                        row6.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                        row6.Cells[2].AddParagraph("Type");
                        row6.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                        row6.Cells[3].AddParagraph("Effective Date");
                        row6.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                        row6.Cells[4].AddParagraph("End Date");
                        row6.Cells[4].Format.Alignment = ParagraphAlignment.Left;

                        string MEPLFinancialPayer = string.Empty;
                        string MEPLMonthlyAmount = string.Empty;
                        string MEPLType = string.Empty;
                        string MEPLEffectiveDate = string.Empty;
                        string MEPLEndDate = string.Empty;

                        for (int i = 0; i < pdfReq.PatientLiabilities.Count; i++)
                        {
                            if (pdfReq.PatientLiabilities[i].FinancialPayer != null)
                            {
                                MEPLFinancialPayer = pdfReq.PatientLiabilities[i].FinancialPayer;
                            }
                            if (pdfReq.PatientLiabilities[i].MonthlyAmount != null)
                            {
                                MEPLMonthlyAmount = pdfReq.PatientLiabilities[i].MonthlyAmount.ToString();
                            }
                            if (pdfReq.PatientLiabilities[i].Type != null)
                            {
                                MEPLType = pdfReq.PatientLiabilities[i].Type;
                            }
                            if (pdfReq.PatientLiabilities[i].EffectiveDate != null)
                            {
                                MEPLEffectiveDate = GetDateString(pdfReq.PatientLiabilities[i].EffectiveDate.Split("T")[0]);
                            }
                            if (pdfReq.PatientLiabilities[i].EndDate != null)
                            {
                                MEPLEndDate = GetDateString(pdfReq.PatientLiabilities[i].EndDate.Split("T")[0]);
                            }

                            row6 = table6.AddRow();
                            row6.HeadingFormat = false;
                            row6.Format.Alignment = ParagraphAlignment.Center;
                            row6.Format.Font.Bold = false;
                            row6.Cells[0].AddParagraph(MEPLFinancialPayer);
                            row6.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                            row6.Cells[1].AddParagraph(MEPLMonthlyAmount);
                            row6.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                            row6.Cells[2].AddParagraph(MEPLType);
                            row6.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                            row6.Cells[3].AddParagraph(MEPLEffectiveDate);
                            row6.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                            row6.Cells[4].AddParagraph(MEPLEndDate);
                            row6.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                        }
                    }
                }
                catch (Exception ex)
                { }

                try
                {
                    if (pdfReq.LTCFPlacements.Count > 0)
                    {
                        document.LastSection.AddParagraph("", "Heading2");

                        // Create the item table
                        Table table7 = section.AddTable();
                        table7.Style = "Table";
                        table7.Borders.Color = Color.FromRgb(1, 1, 1);
                        table7.Borders.Width = 0.25;
                        table7.Borders.Left.Width = 0.5;
                        table7.Borders.Right.Width = 0.5;
                        table7.Rows.LeftIndent = 0;

                        // Before you can add a row, you must define the columns
                        Column column7 = table7.AddColumn("4cm");
                        column7.Format.Font.Size = 8;
                        column7.Format.Alignment = ParagraphAlignment.Center;

                        column7 = table7.AddColumn("3cm");
                        column7.Format.Font.Size = 8;
                        column7.Format.Alignment = ParagraphAlignment.Center;

                        column7 = table7.AddColumn("3cm");
                        column7.Format.Font.Size = 8;
                        column7.Format.Alignment = ParagraphAlignment.Center;

                        column7 = table7.AddColumn("3cm");
                        column7.Format.Font.Size = 8;
                        column7.Format.Alignment = ParagraphAlignment.Center;

                        column7 = table7.AddColumn("3cm");
                        column7.Format.Font.Size = 8;
                        column7.Format.Alignment = ParagraphAlignment.Center;

                        Row row70 = table7.AddRow();
                        row70.HeadingFormat = true;
                        row70.Format.Alignment = ParagraphAlignment.Center;
                        row70.Format.Font.Bold = true;
                        row70.Format.Font.Size = 12;
                        row70.Cells[0].AddParagraph("Long Term Care Facility Placements");
                        row70.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row70.Cells[0].MergeRight = 4;
                        row70.Borders.Visible = false;

                        Row row7 = table7.AddRow();
                        row7.HeadingFormat = true;
                        row7.Format.Alignment = ParagraphAlignment.Center;
                        row7.Format.Font.Bold = true;
                        row7.Cells[0].AddParagraph("Facility Type");
                        row7.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row7.Cells[1].AddParagraph("Date of Admission");
                        row7.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                        row7.Cells[2].AddParagraph("Discharge Date");
                        row7.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                        row7.Cells[3].AddParagraph("Effective Date of Medicaid Coverage");
                        row7.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                        row7.Cells[4].AddParagraph("End Date of Medicaid Coverage");
                        row7.Cells[4].Format.Alignment = ParagraphAlignment.Left;

                        string MELTCFacilityType = string.Empty;
                        string MELTCEffectiveDate = string.Empty;
                        string MELTCEndDate = string.Empty;
                        string MELTCEffectiveDateMedicaidCoverage = string.Empty;
                        string MELTCEndDateMedicaidCoverage = string.Empty;

                        for (int i = 0; i < pdfReq.LTCFPlacements.Count; i++)
                        {
                            if (pdfReq.LTCFPlacements[i].FacilityType != null)
                            {
                                MELTCFacilityType = HttpUtility.HtmlDecode(pdfReq.LTCFPlacements[i].FacilityType);
                            }
                            if (pdfReq.LTCFPlacements[i].EffectiveDate != null)
                            {
                                MELTCEffectiveDate = GetDateString(pdfReq.LTCFPlacements[i].EffectiveDate.Split("T")[0]);
                            }
                            if (pdfReq.LTCFPlacements[i].EndDate != null)
                            {
                                MELTCEndDate = GetDateString(pdfReq.LTCFPlacements[i].EndDate.Split("T")[0]);
                            }
                            if (pdfReq.LTCFPlacements[i].EffectiveDateMedicaidCoverage != null)
                            {
                                MELTCEffectiveDateMedicaidCoverage = GetDateString(pdfReq.LTCFPlacements[i].EffectiveDateMedicaidCoverage.Split("T")[0]);
                            }
                            if (pdfReq.LTCFPlacements[i].EndDateMedicaidCoverage != null)
                            {
                                MELTCEndDateMedicaidCoverage = GetDateString(pdfReq.LTCFPlacements[i].EndDateMedicaidCoverage.Split("T")[0]);
                            }

                            row7 = table7.AddRow();
                            row7.HeadingFormat = false;
                            row7.Format.Alignment = ParagraphAlignment.Center;
                            row7.Format.Font.Bold = false;
                            row7.Cells[0].AddParagraph(MELTCFacilityType);
                            row7.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                            row7.Cells[1].AddParagraph(MELTCEffectiveDate);
                            row7.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                            row7.Cells[2].AddParagraph(MELTCEndDate);
                            row7.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                            row7.Cells[3].AddParagraph(MELTCEffectiveDateMedicaidCoverage);
                            row7.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                            row7.Cells[4].AddParagraph(MELTCEndDateMedicaidCoverage);
                            row7.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                        }
                    }
                }
                catch (Exception ex)
                { }


                try
                {
                    if (pdfReq.Lockins.Count > 0)
                    {
                        document.LastSection.AddParagraph("", "Heading2");

                        // Create the item table
                        Table table8 = section.AddTable();
                        table8.Style = "Table";
                        table8.Borders.Color = Color.FromRgb(1, 1, 1);
                        table8.Borders.Width = 0.25;
                        table8.Borders.Left.Width = 0.5;
                        table8.Borders.Right.Width = 0.5;
                        table8.Rows.LeftIndent = 0;

                        // Before you can add a row, you must define the columns
                        Column column8 = table8.AddColumn("2.5cm");
                        column8.Format.Font.Size = 8;
                        column8.Format.Alignment = ParagraphAlignment.Center;

                        column8 = table8.AddColumn("2cm");
                        column8.Format.Font.Size = 8;
                        column8.Format.Alignment = ParagraphAlignment.Center;

                        column8 = table8.AddColumn("2.2cm");
                        column8.Format.Font.Size = 8;
                        column8.Format.Alignment = ParagraphAlignment.Center;

                        column8 = table8.AddColumn("2.2cm");
                        column8.Format.Font.Size = 8;
                        column8.Format.Alignment = ParagraphAlignment.Center;

                        column8 = table8.AddColumn("2.5cm");
                        column8.Format.Font.Size = 8;
                        column8.Format.Alignment = ParagraphAlignment.Center;

                        column8 = table8.AddColumn("3cm");
                        column8.Format.Font.Size = 8;
                        column8.Format.Alignment = ParagraphAlignment.Center;

                        column8 = table8.AddColumn("2.5cm");
                        column8.Format.Font.Size = 8;
                        column8.Format.Alignment = ParagraphAlignment.Center;

                        Row row80 = table8.AddRow();
                        row80.HeadingFormat = true;
                        row80.Format.Alignment = ParagraphAlignment.Center;
                        row80.Format.Font.Bold = true;
                        row80.Format.Font.Size = 12;
                        row80.Cells[0].AddParagraph("Lock In");
                        row80.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row80.Cells[0].MergeRight = 6;
                        row80.Borders.Visible = false;

                        Row row8 = table8.AddRow();
                        row8.HeadingFormat = true;
                        row8.Format.Alignment = ParagraphAlignment.Center;
                        row8.Format.Font.Bold = true;
                        row8.Cells[0].AddParagraph("Lock-In Plan");
                        row8.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row8.Cells[1].AddParagraph("Lock-In Type");
                        row8.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                        row8.Cells[2].AddParagraph("Effective Date");
                        row8.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                        row8.Cells[3].AddParagraph("End Date");
                        row8.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                        row8.Cells[4].AddParagraph("Provider NPI");
                        row8.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                        row8.Cells[5].AddParagraph("Provider Name");
                        row8.Cells[5].Format.Alignment = ParagraphAlignment.Left;
                        row8.Cells[6].AddParagraph("Provider Phone");
                        row8.Cells[6].Format.Alignment = ParagraphAlignment.Left;

                        string MELLockinPlan = string.Empty;
                        string MELLockinType = string.Empty;
                        string MELEffectiveDate = string.Empty;
                        string MELEndDate = string.Empty;
                        string MELProviderNPI = string.Empty;
                        string MELProviderName = string.Empty;
                        string MELProviderPhoneNumber = string.Empty;

                        for (int i = 0; i < pdfReq.Lockins.Count; i++)
                        {
                            if (pdfReq.Lockins[i].LockinPlan != null)
                            {
                                MELLockinPlan = pdfReq.Lockins[i].LockinPlan;
                            }
                            if (pdfReq.Lockins[i].LockinType != null)
                            {
                                MELLockinType = pdfReq.Lockins[i].LockinType;
                            }
                            if (pdfReq.Lockins[i].EffectiveDate != null)
                            {
                                MELEffectiveDate = GetDateString(pdfReq.Lockins[i].EffectiveDate.Split("T")[0]);
                            }
                            if (pdfReq.Lockins[i].EndDate != null)
                            {
                                MELEndDate = GetDateString(pdfReq.Lockins[i].EndDate.Split("T")[0]);
                            }
                            if (pdfReq.Lockins[i].ProviderNPI != null)
                            {
                                MELProviderNPI = pdfReq.Lockins[i].ProviderNPI;
                            }
                            if (pdfReq.Lockins[i].ProviderName != null)
                            {
                                MELProviderName = pdfReq.Lockins[i].ProviderName;
                            }
                            if (pdfReq.Lockins[i].ProviderPhoneNumber != null)
                            {
                                MELProviderPhoneNumber = pdfReq.Lockins[i].ProviderPhoneNumber;
                            }

                            row8 = table8.AddRow();
                            row8.HeadingFormat = false;
                            row8.Format.Alignment = ParagraphAlignment.Center;
                            row8.Format.Font.Bold = false;
                            row8.Cells[0].AddParagraph(MELLockinPlan);
                            row8.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                            row8.Cells[1].AddParagraph(MELLockinType);
                            row8.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                            row8.Cells[2].AddParagraph(MELEffectiveDate);
                            row8.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                            row8.Cells[3].AddParagraph(MELEndDate);
                            row8.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                            row8.Cells[4].AddParagraph(MELProviderNPI);
                            row8.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                            row8.Cells[5].AddParagraph(MELProviderName);
                            row8.Cells[5].Format.Alignment = ParagraphAlignment.Left;
                            row8.Cells[6].AddParagraph(MELProviderPhoneNumber);
                            row8.Cells[6].Format.Alignment = ParagraphAlignment.Left;
                        }
                    }
                }
                catch (Exception ex)
                { }

                try
                {
                    if (pdfReq.MedicareCoverageDetails.Count > 0)
                    {
                        document.LastSection.AddParagraph("", "Heading2");

                        // Create the item table
                        Table table9 = section.AddTable();
                        table9.Style = "Table";
                        table9.Borders.Color = Color.FromRgb(1, 1, 1);
                        table9.Borders.Width = 0.25;
                        table9.Borders.Left.Width = 0.5;
                        table9.Borders.Right.Width = 0.5;
                        table9.Rows.LeftIndent = 0;

                        // Before you can add a row, you must define the columns
                        Column column9 = table9.AddColumn("3cm");
                        column9.Format.Font.Size = 8;
                        column9.Format.Alignment = ParagraphAlignment.Center;

                        column9 = table9.AddColumn("2cm");
                        column9.Format.Font.Size = 8;
                        column9.Format.Alignment = ParagraphAlignment.Center;

                        column9 = table9.AddColumn("4cm");
                        column9.Format.Font.Size = 8;
                        column9.Format.Alignment = ParagraphAlignment.Center;

                        column9 = table9.AddColumn("2.2cm");
                        column9.Format.Font.Size = 8;
                        column9.Format.Alignment = ParagraphAlignment.Center;

                        column9 = table9.AddColumn("2.2cm");
                        column9.Format.Font.Size = 8;
                        column9.Format.Alignment = ParagraphAlignment.Center;

                        column9 = table9.AddColumn("4cm");
                        column9.Format.Font.Size = 8;
                        column9.Format.Alignment = ParagraphAlignment.Center;

                        Row row90 = table9.AddRow();
                        row90.HeadingFormat = true;
                        row90.Format.Alignment = ParagraphAlignment.Center;
                        row90.Format.Font.Bold = true;
                        row90.Format.Font.Size = 12;
                        row90.Cells[0].AddParagraph("Medicare");
                        row90.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row90.Cells[0].MergeRight = 5;
                        row90.Borders.Visible = false;

                        Row row9 = table9.AddRow();
                        row9.HeadingFormat = true;
                        row9.Format.Alignment = ParagraphAlignment.Center;
                        row9.Format.Font.Bold = true;
                        row9.Cells[0].AddParagraph("Coverage");
                        row9.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row9.Cells[1].AddParagraph("Effective Date");
                        row9.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                        row9.Cells[2].AddParagraph("End Date");
                        row9.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                        row9.Cells[3].AddParagraph("Plan Name");
                        row9.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                        row9.Cells[4].AddParagraph("Plan ID");
                        row9.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                        row9.Cells[5].AddParagraph("Medicare ID");
                        row9.Cells[5].Format.Alignment = ParagraphAlignment.Left;

                        string MEMCDCoverage = string.Empty;
                        string MEMCDPlanId = string.Empty;
                        string MEMCDPlanName = string.Empty;
                        string MEMCDMedicareId = string.Empty;
                        string MEMCDEffectiveDate = string.Empty;
                        string MEMCDEndDate = string.Empty;

                        for (int i = 0; i < pdfReq.MedicareCoverageDetails.Count; i++)
                        {
                            if (pdfReq.MedicareCoverageDetails[i].Coverage != null)
                            {
                                MEMCDCoverage = pdfReq.MedicareCoverageDetails[i].Coverage;
                            }
                            if (pdfReq.MedicareCoverageDetails[i].PlanId != null)
                            {
                                MEMCDPlanId = pdfReq.MedicareCoverageDetails[i].PlanId;
                            }
                            if (pdfReq.MedicareCoverageDetails[i].PlanName != null)
                            {
                                MEMCDPlanName = pdfReq.MedicareCoverageDetails[i].PlanName;
                            }
                            if (pdfReq.MedicareCoverageDetails[i].MedicareId != null)
                            {
                                MEMCDMedicareId = pdfReq.MedicareCoverageDetails[i].MedicareId;
                            }
                            if (pdfReq.MedicareCoverageDetails[i].EffectiveDate != null)
                            {
                                MEMCDEffectiveDate = GetDateString(pdfReq.MedicareCoverageDetails[i].EffectiveDate.Split("T")[0]);
                            }
                            if (pdfReq.MedicareCoverageDetails[i].EndDate != null)
                            {
                                MEMCDEndDate = GetDateString(pdfReq.MedicareCoverageDetails[i].EndDate.Split("T")[0]);
                            }

                            row9 = table9.AddRow();
                            row9.HeadingFormat = false;
                            row9.Format.Alignment = ParagraphAlignment.Center;
                            row9.Format.Font.Bold = false;
                            row9.Cells[0].AddParagraph(MEMCDCoverage);
                            row9.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                            row9.Cells[1].AddParagraph(MEMCDEffectiveDate);
                            row9.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                            row9.Cells[2].AddParagraph(MEMCDEndDate);
                            row9.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                            row9.Cells[3].AddParagraph(MEMCDPlanName);
                            row9.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                            row9.Cells[4].AddParagraph(MEMCDPlanId);
                            row9.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                            row9.Cells[5].AddParagraph(MEMCDMedicareId);
                            row9.Cells[5].Format.Alignment = ParagraphAlignment.Left;
                        }
                    }
                }
                catch (Exception ex)
                { }

                try
                {
                    if (pdfReq.ServiceLimitations.Count > 0)
                    {
                        document.LastSection.AddParagraph("", "Heading2");

                        // Create the item table
                        Table table10 = section.AddTable();
                        table10.Style = "Table";
                        table10.Borders.Color = Color.FromRgb(1, 1, 1);
                        table10.Borders.Width = 0.25;
                        table10.Borders.Left.Width = 0.5;
                        table10.Borders.Right.Width = 0.5;
                        table10.Rows.LeftIndent = 0;

                        // Before you can add a row, you must define the columns
                        Column column10 = table10.AddColumn("2cm");
                        column10.Format.Font.Size = 8;
                        column10.Format.Alignment = ParagraphAlignment.Center;

                        column10 = table10.AddColumn("3cm");
                        column10.Format.Font.Size = 8;
                        column10.Format.Alignment = ParagraphAlignment.Center;

                        column10 = table10.AddColumn("3cm");
                        column10.Format.Font.Size = 8;
                        column10.Format.Alignment = ParagraphAlignment.Center;

                        column10 = table10.AddColumn("1.2cm");
                        column10.Format.Font.Size = 8;
                        column10.Format.Alignment = ParagraphAlignment.Center;

                        column10 = table10.AddColumn("1.2cm");
                        column10.Format.Font.Size = 8;
                        column10.Format.Alignment = ParagraphAlignment.Center;

                        column10 = table10.AddColumn("2cm");
                        column10.Format.Font.Size = 8;
                        column10.Format.Alignment = ParagraphAlignment.Center;

                        column10 = table10.AddColumn("2cm");
                        column10.Format.Font.Size = 8;
                        column10.Format.Alignment = ParagraphAlignment.Center;

                        column10 = table10.AddColumn("2cm");
                        column10.Format.Font.Size = 8;
                        column10.Format.Alignment = ParagraphAlignment.Center;

                        Row row100 = table10.AddRow();
                        row100.HeadingFormat = true;
                        row100.Format.Alignment = ParagraphAlignment.Center;
                        row100.Format.Font.Bold = true;
                        row100.Format.Font.Size = 12;
                        row100.Cells[0].AddParagraph("Service Limitation");
                        row100.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row100.Cells[0].MergeRight = 7;
                        row100.Borders.Visible = false;

                        Row row10 = table10.AddRow();
                        row10.HeadingFormat = true;
                        row10.Format.Alignment = ParagraphAlignment.Center;
                        row10.Format.Font.Bold = true;
                        row10.Cells[0].AddParagraph("Procedure Code");
                        row10.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row10.Cells[1].AddParagraph("Description");
                        row10.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                        row10.Cells[2].AddParagraph("Benefit Description");
                        row10.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                        row10.Cells[3].AddParagraph("Total Limits");
                        row10.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                        row10.Cells[4].AddParagraph("Used Limits");
                        row10.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                        row10.Cells[5].AddParagraph("Remaining Limits");
                        row10.Cells[5].Format.Alignment = ParagraphAlignment.Left;
                        row10.Cells[6].AddParagraph("Time Frame");
                        row10.Cells[6].Format.Alignment = ParagraphAlignment.Left;
                        row10.Cells[7].AddParagraph("Date of Next Service");
                        row10.Cells[7].Format.Alignment = ParagraphAlignment.Left;

                        string MESLProcedureCode = string.Empty;
                        string MESLServiceLimitDescription = string.Empty;
                        string MESLBenefitDescription = string.Empty;
                        string MESLTotalLimits = string.Empty;
                        string MESLUsedLimits = string.Empty;
                        string MESLRemainingLimits = string.Empty;
                        string MESLTimeframe = string.Empty;
                        string MESLDateOfNextService = string.Empty;

                        for (int i = 0; i < pdfReq.ServiceLimitations.Count; i++)
                        {
                            if (pdfReq.ServiceLimitations[i].ProcedureCode != null)
                            {
                                MESLProcedureCode = pdfReq.ServiceLimitations[i].ProcedureCode;
                            }
                            if (pdfReq.ServiceLimitations[i].ServiceLimitDescription != null)
                            {

                                MESLServiceLimitDescription = pdfReq.ServiceLimitations[i].ServiceLimitDescription.Replace('_', '-');
                            }
                            if (pdfReq.ServiceLimitations[i].BenefitDescription != null)
                            {
                                MESLBenefitDescription = pdfReq.ServiceLimitations[i].BenefitDescription;
                            }
                            if (pdfReq.ServiceLimitations[i].TotalLimits != null)
                            {
                                MESLTotalLimits = pdfReq.ServiceLimitations[i].TotalLimits.ToString();
                            }
                            if (pdfReq.ServiceLimitations[i].UsedLimits != null)
                            {
                                MESLUsedLimits = pdfReq.ServiceLimitations[i].UsedLimits.ToString();
                            }
                            if (pdfReq.ServiceLimitations[i].RemainingLimits != null)
                            {
                                MESLRemainingLimits = pdfReq.ServiceLimitations[i].RemainingLimits.ToString();
                            }
                            if (pdfReq.ServiceLimitations[i].Timeframe != null)
                            {
                                MESLTimeframe = pdfReq.ServiceLimitations[i].Timeframe;
                            }
                            if (pdfReq.ServiceLimitations[i].DateOfNextService != null)
                            {
                                MESLDateOfNextService = GetDateString(pdfReq.ServiceLimitations[i].DateOfNextService.Split("T")[0]);
                            }

                            row10 = table10.AddRow();
                            row10.HeadingFormat = false;
                            row10.Format.Alignment = ParagraphAlignment.Center;
                            row10.Format.Font.Bold = false;
                            row10.Cells[0].AddParagraph(MESLProcedureCode);
                            row10.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                            row10.Cells[1].AddParagraph(MESLServiceLimitDescription);
                            row10.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                            row10.Cells[2].AddParagraph(MESLBenefitDescription);
                            row10.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                            row10.Cells[3].AddParagraph(MESLTotalLimits);
                            row10.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                            row10.Cells[4].AddParagraph(MESLUsedLimits);
                            row10.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                            row10.Cells[5].AddParagraph(MESLRemainingLimits);
                            row10.Cells[5].Format.Alignment = ParagraphAlignment.Left;
                            row10.Cells[6].AddParagraph(MESLTimeframe);
                            row10.Cells[6].Format.Alignment = ParagraphAlignment.Left;
                            row10.Cells[7].AddParagraph(MESLDateOfNextService);
                            row10.Cells[7].Format.Alignment = ParagraphAlignment.Left;
                        }
                    }
                }
                catch (Exception ex)
                { }


                try
                {
                    if (pdfReq.RestrictedCoverages.Count > 0)
                    {
                        document.LastSection.AddParagraph("", "Heading2");

                        // Create the item table
                        Table table11 = section.AddTable();
                        table11.Style = "Table";
                        table11.Borders.Color = Color.FromRgb(1, 1, 1);
                        table11.Borders.Width = 0.25;
                        table11.Borders.Left.Width = 0.5;
                        table11.Borders.Right.Width = 0.5;
                        table11.Rows.LeftIndent = 0;

                        // Before you can add a row, you must define the columns
                        Column column11 = table11.AddColumn("5cm");
                        column11.Format.Font.Size = 8;
                        column11.Format.Alignment = ParagraphAlignment.Center;

                        column11 = table11.AddColumn("5cm");
                        column11.Format.Font.Size = 8;
                        column11.Format.Alignment = ParagraphAlignment.Center;

                        Row row110 = table11.AddRow();
                        row110.HeadingFormat = true;
                        row110.Format.Alignment = ParagraphAlignment.Center;
                        row110.Format.Font.Bold = true;
                        row110.Format.Font.Size = 12;
                        row110.Cells[0].AddParagraph("Restricted Coverage");
                        row110.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row110.Cells[0].MergeRight = 1;
                        row110.Borders.Visible = false;

                        Row row11 = table11.AddRow();
                        row11.HeadingFormat = true;
                        row11.Format.Alignment = ParagraphAlignment.Center;
                        row11.Format.Font.Bold = true;
                        row11.Cells[0].AddParagraph("Effective Date");
                        row11.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row11.Cells[1].AddParagraph("End Date");
                        row11.Cells[1].Format.Alignment = ParagraphAlignment.Left;

                        string MERCEffectiveDate = string.Empty;
                        string MERCEndDate = string.Empty;

                        for (int i = 0; i < pdfReq.RestrictedCoverages.Count; i++)
                        {
                            if (pdfReq.RestrictedCoverages[i].EffectiveDate != null)
                            {
                                MERCEffectiveDate = GetDateString(pdfReq.RestrictedCoverages[i].EffectiveDate.Split("T")[0]);
                            }
                            if (pdfReq.RestrictedCoverages[i].EndDate != null)
                            {
                                MERCEndDate = GetDateString(pdfReq.RestrictedCoverages[i].EndDate.Split("T")[0]);
                            }

                            row11 = table11.AddRow();
                            row11.HeadingFormat = false;
                            row11.Format.Alignment = ParagraphAlignment.Center;
                            row11.Format.Font.Bold = false;
                            row11.Cells[0].AddParagraph(MERCEffectiveDate);
                            row11.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                            row11.Cells[1].AddParagraph(MERCEndDate);
                            row11.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                        }
                    }
                }
                catch (Exception ex)
                { }

                try
                {
                    if (pdfReq.Under19FamilyMembers.Count > 0)
                    {
                        document.LastSection.AddParagraph("", "Heading2");

                        // Create the item table
                        Table table12 = section.AddTable();
                        table12.Style = "Table";
                        table12.Borders.Color = Color.FromRgb(1, 1, 1);
                        table12.Borders.Width = 0.25;
                        table12.Borders.Left.Width = 0.5;
                        table12.Borders.Right.Width = 0.5;
                        table12.Rows.LeftIndent = 0;

                        // Before you can add a row, you must define the columns
                        Column column12 = table12.AddColumn("4cm");
                        column12.Format.Font.Size = 8;
                        column12.Format.Alignment = ParagraphAlignment.Center;

                        column12 = table12.AddColumn("4cm");
                        column12.Format.Font.Size = 8;
                        column12.Format.Alignment = ParagraphAlignment.Center;

                        column12 = table12.AddColumn("2cm");
                        column12.Format.Font.Size = 8;
                        column12.Format.Alignment = ParagraphAlignment.Center;

                        column12 = table12.AddColumn("4cm");
                        column12.Format.Font.Size = 8;
                        column12.Format.Alignment = ParagraphAlignment.Center;

                        column12 = table12.AddColumn("2cm");
                        column12.Format.Font.Size = 8;
                        column12.Format.Alignment = ParagraphAlignment.Center;

                        column12 = table12.AddColumn("2.2cm");
                        column12.Format.Font.Size = 8;
                        column12.Format.Alignment = ParagraphAlignment.Center;

                        Row row120 = table12.AddRow();
                        row120.HeadingFormat = true;
                        row120.Format.Alignment = ParagraphAlignment.Center;
                        row120.Format.Font.Bold = true;
                        row120.Format.Font.Size = 12;
                        row120.Cells[0].AddParagraph("Associated Child(ren)");
                        row120.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row120.Cells[0].MergeRight = 5;
                        row120.Borders.Visible = false;

                        Row row12 = table12.AddRow();
                        row12.HeadingFormat = true;
                        row12.Format.Alignment = ParagraphAlignment.Center;
                        row12.Format.Font.Bold = true;
                        row12.Cells[0].AddParagraph("Medicaid Billing Number");
                        row12.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                        row12.Cells[1].AddParagraph("First Name");
                        row12.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                        row12.Cells[2].AddParagraph("MI");
                        row12.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                        row12.Cells[3].AddParagraph("Last Name");
                        row12.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                        row12.Cells[4].AddParagraph("Gender");
                        row12.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                        row12.Cells[5].AddParagraph("Date of Birth");
                        row12.Cells[5].Format.Alignment = ParagraphAlignment.Left;

                        string MEMedicaidId = string.Empty;
                        string MEUU9DateOfBirth = string.Empty;
                        string MEFirstName = string.Empty;
                        string MEMiddleInitial = string.Empty;
                        string MELastName = string.Empty;
                        string MEGender = string.Empty;

                        for (int i = 0; i < pdfReq.Under19FamilyMembers.Count; i++)
                        {
                            if (pdfReq.Under19FamilyMembers[i].MedicaidId != null)
                            {
                                MEMedicaidId = pdfReq.Under19FamilyMembers[i].MedicaidId;
                            }
                            if (pdfReq.Under19FamilyMembers[i].DateOfBirth != null)
                            {
                                MEUU9DateOfBirth = GetDateString(pdfReq.Under19FamilyMembers[i].DateOfBirth.Split("T")[0]);
                            }
                            if (pdfReq.Under19FamilyMembers[i].FirstName != null)
                            {
                                MEFirstName = pdfReq.Under19FamilyMembers[i].FirstName;
                            }
                            if (pdfReq.Under19FamilyMembers[i].MiddleInitial != null)
                            {
                                MEMiddleInitial = pdfReq.Under19FamilyMembers[i].MiddleInitial;
                            }
                            if (pdfReq.Under19FamilyMembers[i].LastName != null)
                            {
                                MELastName = pdfReq.Under19FamilyMembers[i].LastName;
                            }
                            if (pdfReq.Under19FamilyMembers[i].Gender != null)
                            {
                                MEGender = pdfReq.Under19FamilyMembers[i].Gender;
                            }

                            row12 = table12.AddRow();
                            row12.HeadingFormat = false;
                            row12.Format.Alignment = ParagraphAlignment.Center;
                            row12.Format.Font.Bold = false;
                            row12.Cells[0].AddParagraph(MEMedicaidId);
                            row12.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                            row12.Cells[1].AddParagraph(MEFirstName);
                            row12.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                            row12.Cells[2].AddParagraph(MEMiddleInitial);
                            row12.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                            row12.Cells[3].AddParagraph(MELastName);
                            row12.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                            row12.Cells[4].AddParagraph(MEGender);
                            row12.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                            row12.Cells[5].AddParagraph(MEUU9DateOfBirth);
                            row12.Cells[5].Format.Alignment = ParagraphAlignment.Left;
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                MigraDoc.Rendering.DocumentRenderer renderer = new DocumentRenderer(document);
                MigraDoc.Rendering.PdfDocumentRenderer pdfRenderer = new MigraDoc.Rendering.PdfDocumentRenderer();
                GlobalFontSettings.FontResolver = new FailsafeFontResolver();
                pdfRenderer.Document = document;
                pdfRenderer.RenderDocument();
                byte[]? response = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdfRenderer.PdfDocument.Save(ms, false);
                    byte[] buffer = new byte[ms.Length];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Flush();
                    ms.Read(buffer, 0, (int)ms.Length);
                    response = ms.ToArray();
                }
                string fileName = "EligibilitySearchResults.pdf";
                Models.PdfResponseJSON pdfr = new Models.PdfResponseJSON();
                pdfr.response = "SUCCESS";
                pdfr.fileBytes = response;
                //return File(response, "application/pdf", fileName);
                return Ok(pdfr);
            }
            catch (Exception ex)
            {
                Models.PdfResponseJSON pdfr = new Models.PdfResponseJSON();
                pdfr.response = "FAILURE";
                return Ok(pdfr);
                //return null;
            }
        }

        private string GetDateString(string date)
        {
            return Convert.ToDateTime(date).ToString("MM/dd/yyyy");
        }
    }
}
