using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using MAXIMUS.Core.Libraries;

namespace PDMSRestServices.Facade
{
    public class RegistrationFacade
    {
        public static int SelectClaimAttachmentTypeId(string typename, string claim)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    SqlParms.CreateParameter("DocumentType", DbType.String, typename, true),
                    SqlParms.CreateParameter("Claim", DbType.String, claim, true)
                };

                object scalarResult = DataAccess.ExecuteScalar("usp_SelectClaimAttachmentTypeId", parameters);

                if (scalarResult == null || string.IsNullOrWhiteSpace(scalarResult.ToString()))
                {
                    return 0;
                }
                if (int.TryParse(scalarResult.ToString(), out int result))
                {
                    return result;
                }
                throw new FormatException("Returned value could not be parsed to an integer.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GenerateDocumentNumber(string MedicaidId)
        {
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("GRPMedicaid_ID", DbType.String, MedicaidId, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProviderByGRPMedicaidID", parameters, "RegData_" + MedicaidId);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet SelectProviderByGRPMedicaidID(string GRPMedicaid_ID)
        {
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("GRPMedicaid_ID", DbType.String, GRPMedicaid_ID, false));

                DataSet lookup = new DataSet();
                lookup = DataAccess.ExecuteStoredProcedure("usp_SelectProviderByGRPMedicaidID", parameters, "RegData_" + GRPMedicaid_ID);

                lookup.Tables[0].TableName = "RegData";

                return lookup;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
