using Common.DataModels;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace MAXIMUS.Controllers.PDMS
{
    public static class DocumentsAndReportsController
    {
        public static void UpdatePage_Configuration(int Page_Configuration_ID, string PageName, string SectionName, string LINK_TEXT, string SHOW_AS_LINK_OR_TEXT, string reference_path, int DocumentId, DateTime? LAST_MODIFIED_DATE_TIME, string LAST_MODIFIED_USER, int section_display_order, string displaytext)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Page_Configuration_ID", DbType.Int32, Page_Configuration_ID, true));
            parameters.Add(SqlParms.CreateParameter("Page_Name", DbType.String, PageName, true));
            parameters.Add(SqlParms.CreateParameter("Section_Name", DbType.String, SectionName, true));
            parameters.Add(SqlParms.CreateParameter("LINK_TEXT", DbType.String, LINK_TEXT, true));
            parameters.Add(SqlParms.CreateParameter("SHOW_AS_LINK_OR_TEXT", DbType.String, SHOW_AS_LINK_OR_TEXT, true));
            parameters.Add(SqlParms.CreateParameter("reference_path", DbType.String, reference_path, true));
            parameters.Add(SqlParms.CreateParameter("Document_Id", DbType.Int32, DocumentId, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, LAST_MODIFIED_DATE_TIME, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, LAST_MODIFIED_USER, true));
            parameters.Add(SqlParms.CreateParameter("section_display_order", DbType.Int32, section_display_order, true));
            parameters.Add(SqlParms.CreateParameter("displaytext", DbType.String, displaytext, true));
            DataAccess.ExecuteScalar("updatePage_Configuration", parameters);
        }

        public static void InsertPage_Configuration(string PageName, string SectionName, string LINK_TEXT, string SHOW_AS_LINK_OR_TEXT, string reference_path, int DocumentId, DateTime? LAST_MODIFIED_DATE_TIME, string LAST_MODIFIED_USER, int section_display_order, string displaytext)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Page_Name", DbType.String, PageName, true));
            parameters.Add(SqlParms.CreateParameter("Section_Name", DbType.String, SectionName, true));
            parameters.Add(SqlParms.CreateParameter("LINK_TEXT", DbType.String, LINK_TEXT, true));
            parameters.Add(SqlParms.CreateParameter("SHOW_AS_LINK_OR_TEXT", DbType.String, SHOW_AS_LINK_OR_TEXT, true));
            parameters.Add(SqlParms.CreateParameter("reference_path", DbType.String, reference_path, true));
            parameters.Add(SqlParms.CreateParameter("Document_Id", DbType.Int32, DocumentId, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, LAST_MODIFIED_DATE_TIME, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, LAST_MODIFIED_USER, true));
            parameters.Add(SqlParms.CreateParameter("section_display_order", DbType.Int32, section_display_order, true));
            parameters.Add(SqlParms.CreateParameter("displaytext", DbType.String, displaytext, true));
            Convert.ToInt32(DataAccess.ExecuteScalar("insertPage_Configuration", parameters));
        }

        public static int InsertUploadDocument(string Name, string Description, string FileName, DateTime? LAST_MODIFIED_DATE_TIME, string LAST_MODIFIED_USER)
        {
            int docId = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Name", DbType.String, Name, true));
            parameters.Add(SqlParms.CreateParameter("Description", DbType.String, Description, true));
            parameters.Add(SqlParms.CreateParameter("File_Name", DbType.String, FileName, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, LAST_MODIFIED_DATE_TIME, true));
            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, LAST_MODIFIED_USER, true));
            docId = Convert.ToInt32(DataAccess.ExecuteScalar("insertUPLOAD_DOCUMENT", parameters));
            return docId;
        }

        public static void LogAccess(
            string UserId,
            string DocumentId,
            string ProviderId,
            string DocumentFormat,
            int REPORT_DOCUMENT_TYPE_ID, MAXIMUS.Core.Libraries.Enumerations.LogAccessType AccessType, string NPI)
        {
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                string[] arrAccessType = Enum.GetNames(AccessType.GetType());
                string test = arrAccessType[0];
                parameters.Add(SqlParms.CreateParameter("UserId", DbType.Guid, UserId, false));
                parameters.Add(SqlParms.CreateParameter("DocumentId", DbType.String, DocumentId, false));
                parameters.Add(SqlParms.CreateParameter("ProviderId", DbType.String, ProviderId, false));
                parameters.Add(SqlParms.CreateParameter("DocumentFormat", DbType.String, DocumentFormat, false));
                parameters.Add(SqlParms.CreateParameter("REPORT_DOCUMENT_TYPE_ID", DbType.Int32, REPORT_DOCUMENT_TYPE_ID, false));
                parameters.Add(SqlParms.CreateParameter("AccessType", DbType.Int32, (int)AccessType, false));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, false));
                //@NPI
                DataAccess.ExecuteScalar("usp_LogReportDownload", parameters);

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static LearningDoc GetLearningDoc(string fileId)
        {
            if (string.IsNullOrWhiteSpace(fileId)) return new LearningDoc();
            DataSet ds = new DataSet();
            var p = new SqlParameter("@DocId", fileId);
            var @params = new List<SqlParameter>(1) { p };
            try
            {
                ds = DataAccess.ExecuteStoredProcedure("usp_GetLearningDoc", @params, "categories");
            }
            catch (SqlException) { ds = null; }
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return new LearningDoc();
            var ld = new LearningDoc()
            {
                ID = ds.Tables[0].Rows[0].Field<Guid>("ID").ToString(),
                Name = ds.Tables[0].Rows[0].Field<string>("Name"),
                FileName = ds.Tables[0].Rows[0].Field<string>("FileName"),
                CatId = ds.Tables[0].Rows[0].Field<int>("CatId"),
                Order = ds.Tables[0].Rows[0].Field<int>("Order")
            };
            return ld;
            }

        public static LearningCategoryDocs[] GetLearningDocs()
        {
            var ld = new LearningCategoryDocs[] { };
            DataSet ds = new DataSet();
            ds = DataAccess.ExecuteStoredProcedure("usp_GetLearningCategories", null, "categories");
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return ld;
            ld = LoadLearningCategories(ds, 0);
            return ld;
        }

        private static LearningCategoryDocs[] LoadLearningCategories(DataSet ds, int parentCatId)
        {
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return new LearningCategoryDocs[] { };
            var catIdExpression = parentCatId == 0 ? "is null" : "= " + parentCatId.ToString();
            var catRows = ds.Tables[0].Select("ParentCatId "+ catIdExpression, "[Order] asc");
            if (catRows == null || catRows.Length == 0) return new LearningCategoryDocs[] { };
            var lc = new List<LearningCategoryDocs>(catRows.Length);
            foreach (var r in catRows)
            {
                var lc1 = new LearningCategoryDocs()
                {
                    ID = r.Field<int>("ID"),
                    Name = r.Field<string>("Name"),
                    ParentCatId = IfNull(r.Field<int?>("ParentCatId"),0),
                    Order = r.Field<int>("Order")
                };
                lc1.Documents = LoadLearningDocs(lc1.ID);
                lc.Add(lc1);
                lc1.ChildCategories = LoadLearningCategories(ds, lc1.ID);
            }
            return lc.ToArray();
        }

        private static LearningDoc[] LoadLearningDocs(int catId)
        {
            var ld = new List<LearningDoc>(20);
            DataSet ds = new DataSet();
            var p = new SqlParameter("@CatId", catId);
            var @params = new List<SqlParameter>(1) { p };
            ds = DataAccess.ExecuteStoredProcedure("usp_GetLearningDocs", @params, "docs");
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return ld.ToArray();
            foreach (DataRow r in ds.Tables[0].Rows)
                ld.Add(new LearningDoc()
                {
                    ID = r.Field<Guid>("ID").ToString(),
                    Name = r.Field<string>("Name"),
                    FileName = r.Field<string>("FileName"),
                    CatId = r.Field<int>("CatId"),
                    Order = r.Field<int>("Order")
                });
            return ld.ToArray();
        }

        private static int IfNull(int? val, int defaultVal = 0) { if (val.HasValue) return val.Value; else return defaultVal;  }
        private static string IfNull(string val, string defaultVal = "") { if (val != null) return val; else return defaultVal; }
    }
}
