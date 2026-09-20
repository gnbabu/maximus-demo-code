using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TibcoDocImportNetCore
{
    public class SqlParms
    {

        #region "Properties"

        List<SqlParameter> parameters = new List<SqlParameter>();
        public List<SqlParameter> Parameters
        {
            get
            {
                return this.parameters;
            }
        }

        #endregion

        public void AddParameter(SqlParameter parm)
        {
            this.parameters.Add(parm);
        }

        public static SqlParameter CreateParameter(string fieldName, DbType parmType, object itemValue, bool nullable)
        {

            SqlParameter parameter;

            // configure the parameter name based on the field name
            string parameterName = "@{0}";
            parameterName = String.Format(parameterName, fieldName);

            try
            {
                switch (parmType)
                {
                    case DbType.Boolean:

                        parameter = itemValue == null ? new SqlParameter(parameterName, DBNull.Value) : new SqlParameter(parameterName, Methods.GetIntValue(itemValue, nullable));
                        break;

                    case DbType.Int16:

                        parameter = itemValue == null ? new SqlParameter(parameterName, DBNull.Value) : new SqlParameter(parameterName, Methods.GetIntValue(itemValue, nullable));
                        break;

                    case DbType.Int32:

                        parameter = itemValue == null ? new SqlParameter(parameterName, DBNull.Value) : new SqlParameter(parameterName, Methods.GetIntValue(itemValue, nullable));
                        break;

                    case DbType.Int64:

                        parameter = itemValue == null ? new SqlParameter(parameterName, DBNull.Value) : new SqlParameter(parameterName, Methods.GetIntValue64(itemValue, nullable));
                        break;

                    case DbType.String:

                        parameter = itemValue == null ? new SqlParameter(parameterName, DBNull.Value) : new SqlParameter(parameterName, Methods.GetStringValue(itemValue, nullable));
                        break;

                    case DbType.Decimal:

                        parameter = itemValue == null ? new SqlParameter(parameterName, DBNull.Value) : new SqlParameter(parameterName, Methods.GetDecimalValue(itemValue, nullable));
                        break;

                    case DbType.DateTime:

                        parameter = itemValue == null ? new SqlParameter(parameterName, DBNull.Value) : new SqlParameter(parameterName, Methods.GetDateTimeValue(itemValue, nullable));
                        break;

                    case DbType.Date:

                        parameter = itemValue == null ? new SqlParameter(parameterName, DBNull.Value) : new SqlParameter(parameterName, Methods.GetDateValue(itemValue, nullable));
                        break;

                    case DbType.Guid:

                        parameter = itemValue == null ? new SqlParameter(parameterName, DBNull.Value) : new SqlParameter(parameterName, Methods.GetGuidValue(itemValue, nullable));
                        break;

                    case DbType.Double:

                        parameter = itemValue == null ? new SqlParameter(parameterName, DBNull.Value) : new SqlParameter(parameterName, Methods.GetDoubleValue(itemValue, nullable));
                        break;

                    default:

                        throw new NotImplementedException();
                }

            }
            catch (Exception ex)
            {
                throw  CoreException.ThrowException(ex);
            }
            return parameter;
        }

        public static SqlParameter CreateParameter(string fieldName, DbType parmType, DataRow row, bool nullable)
        {
            try
            {
                object value = row[fieldName];
                return CreateParameter(fieldName, parmType, value, nullable);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}
