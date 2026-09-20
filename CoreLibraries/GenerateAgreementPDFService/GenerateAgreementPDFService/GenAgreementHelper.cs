using MAXIMUS.Core.Libraries;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateAgreementPDFService
{
    public static class GenAgreementHelper
    {
        public static int GetDocumentIDbyFileName(string fileName)
        {
            int docID = 0;
            try
            {               
                using (SqlConnection connection = new SqlConnection(AppSettings.GetConnectionString("mainDB")))
                {
                    connection.Open();
                    using (SqlCommand spCommand = new SqlCommand("usp_GetDocumentIDByFileName", connection))
                    {
                        spCommand.CommandType = CommandType.StoredProcedure;
                        spCommand.Parameters.Add("@DOCUMENT_NAME", SqlDbType.VarChar).Value = fileName;
                        docID = (int)spCommand.ExecuteScalar();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            finally
            {

            }
            return docID;
        }
        public static int GetOnBaseDocumentID(int docID)
        {
            int onBasedocID = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(AppSettings.GetConnectionString("mainDB")))
                {
                    connection.Open();
                    using (SqlCommand spCommand = new SqlCommand("usp_GetOnBaseDocumentId", connection))
                    {
                        spCommand.CommandType = CommandType.StoredProcedure;
                        spCommand.Parameters.Add("@DOCUMENT_ID", SqlDbType.Int).Value = docID;
                        onBasedocID = (int)spCommand.ExecuteScalar();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            finally
            {

            }
            return onBasedocID;
        }

        public static DataSet SelectRegToGenerateAgreement()
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection connection = new SqlConnection(AppSettings.GetConnectionString("mainDB")))
                {
                    connection.Open();
                    using (SqlCommand spCommand = new SqlCommand("usp_SelectRegToGenerateAgreement", connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = spCommand;
                        adapter.Fill(ds);
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            finally
            {

            }
            return ds;
        }

        public static void UpdateAgreementProcessedStatus(int stgPDFid, int docId, int onbaseDocID, string userID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(AppSettings.GetConnectionString("mainDB")))
                {
                    connection.Open();
                    using (SqlCommand spCommand = new SqlCommand("usp_UpdateSTG_GENERATE_PROVIDER_AGREEMENT_PDF", connection))
                    {
                        spCommand.CommandType = CommandType.StoredProcedure;
                        spCommand.Parameters.Add("@STG_PDF_ID", SqlDbType.Int).Value = stgPDFid;
                        spCommand.Parameters.Add("@DOCUMENT_ID", SqlDbType.Int).Value = docId;
                        spCommand.Parameters.Add("@ONBASE_DOCUMENT_ID", SqlDbType.Int).Value = onbaseDocID;
                        spCommand.Parameters.Add("@USER_ID", SqlDbType.VarChar).Value = userID;
                        int rowsAffected = spCommand.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
            finally
            {

            }
        }

    }
}
