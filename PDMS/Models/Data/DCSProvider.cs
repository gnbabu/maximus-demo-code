using MAXIMUS.Core.Libraries;
using System.Data;

namespace MAXIMUS.Models.Data.PDMS
{
    public class DCSProvider : Provider
    {
        public DCSProvider() { }

        #region Properties
        //collection of eligibilities?       
        #endregion

        #region Public Methods
        public void LoadDCSProviderObjectFromDataset(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
                return;

            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                return;

            DataRow dr = dt.Rows[0];
            PartyID = Methods.GetIntValue(dr["PartyID"]);
            FirstName = Methods.GetStringValue(dr["FirstName"]);
            LastName = Methods.GetStringValue(dr["LastName"]);
            NPI = Methods.GetStringValue(dr["NPI"]);
            TaxID = Methods.GetStringValue(dr["TaxID"]);
            OrganizationName = Methods.GetStringValue(dr["OrganizationName"]);
            BaseMedicaidID = Methods.GetStringValue(dr["BaseMedicaidID"]);
            ProviderTypeName = Methods.GetStringValue(dr["ProviderTypeName"]);
            ProviderType = Methods.GetStringValue(dr["ProviderType"]);
        }
        #endregion

    }
}
