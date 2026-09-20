using MAXIMUS.Core.Libraries;
using System;
using System.Data;

namespace MAXIMUS.Controllers.PDMS
{
    public static class StateController
    {
        // db table constants
        public const string dboState = "State";
        public const string StatesRequireCDS = "State";

        // db relationship constants

        // next roster id
        //static int dbkSubmitRosterId = 0;
        //static int dbkReturnRosterId = 0;

        // db field constants
        private const string dbfStateId = dboState + Constants.id;

        public static DataSet SelectStates()
        {
            try
            {

            DataSet states = new DataSet();
            states = DataAccess.ExecuteStoredProcedure("usp_SelectStates", (dboState + Constants.pluralEnding));

            states.Tables[0].TableName = dboState;

            return states;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet SelectCDSRequireStates()
        {
            try
            {

                DataSet states = new DataSet();
                states = DataAccess.ExecuteStoredProcedure("usp_SelectREQUIRE_CDSStates", (StatesRequireCDS + Constants.pluralEnding));

                states.Tables[0].TableName = StatesRequireCDS;

                return states;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectUSStates()
        {
            try
            {
                DataSet states = new DataSet();
                states = DataAccess.ExecuteStoredProcedure("usp_SelectUSStates", (dboState + Constants.pluralEnding));
                 states.Tables[0].TableName = dboState;

                return states;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}
