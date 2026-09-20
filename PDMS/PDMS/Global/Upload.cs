using System.Collections.Generic;
using System.Data;

/// <summary>
/// Summary description for Upload
/// </summary>
public class Upload
{
    public DataSet LoadUserSectionControl(int regId, int applicationTypeId, int providerCategoryTypeId, int providerTypeId, int pageStep, string sectionName = null)
    {
        DataSet dsUploadFiles = new DataSet();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        if (regId > 0 && pageStep > 0)
        {
            // First dataset will get all the docs that are required for this section
            DataSet dsRequiredFiles =
                psc.SelectRegSectionUploadControl(pageStep, applicationTypeId, providerTypeId, providerCategoryTypeId, sectionName, regId);

            // Second dataset will get all the docs that are already uploaded by the user
            dsUploadFiles = psc.SelectRegSectionUploadDocument(pageStep, regId, providerTypeId, sectionName, 0);

            //Make a list of ID's for which file has been uploaded
            List<string> IDWithFile = new List<string>();
            if (Helper.HasRows(dsUploadFiles))
            {
                DataTable dtSectionControl = dsUploadFiles.Tables[0];

                foreach (DataRow dr in dtSectionControl.Rows)
                {
                    IDWithFile.Add(Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr));
                }
            }

            // Filter rows from dsRequiredFiles that are already uploaded, newTable will've remaining rows
            DataTable dtRequired = dsRequiredFiles.Tables[0];
            //DataTable dtRemaining = dsRequiredFiles.Tables[0].Clone();

            string expression = string.Empty;
            if (IDWithFile.Count > 0)
            {
                expression = "REG_SECTION_UPLOAD_CONTROL_ID NOT IN (";
                foreach (var item in IDWithFile)
                {
                    expression += item + ",";
                }

                expression = expression.TrimEnd(',');
                expression += ")";
            }


            DataView dv = new DataView(dsRequiredFiles.Tables[0]);
            if ((dsRequiredFiles.Tables[0].Rows.Count > 0) && (expression != string.Empty))
                dv.RowFilter = expression;

            DataTable newTable = new DataTable();
            if (dv.Count > 0)
            {
                newTable = dv.ToTable();
            }

            dsUploadFiles.Merge(newTable);

        }

        return dsUploadFiles;
    }

    public DataSet LoadUserSectionControlEdit(int regId, int applicationTypeId, int providerCategoryTypeId, int providerTypeId, int pageStep, int rowId, string sectionName = null, bool isEdit = false)
    {
        DataSet dsUploadFiles = new DataSet();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        if (regId > 0 && pageStep > 0)
        {

            dsUploadFiles = psc.SelectRegSectionUploadControlandDocument(pageStep, applicationTypeId, providerTypeId, providerCategoryTypeId, sectionName, regId, rowId);

            //// Second dataset will get all the docs that are already uploaded by the user
            //if (isEdit)
            //    dsUploadFiles = psc.SelectRegSectionUploadDocument(pageStep, regId, providerTypeId, sectionName, rowId);

            ////if (!Helper.HasRows(dsUploadFiles))
            ////{
            //    // First dataset will get all the docs that are required for this section
            //    DataSet dsRequiredFiles =
            //        psc.SelectRegSectionUploadControl(pageStep, applicationTypeId, providerTypeId, providerCategoryTypeId, sectionName,regId);

            //    dsUploadFiles.Merge(dsRequiredFiles);
            //}
        }

        return dsUploadFiles;
    }

}

