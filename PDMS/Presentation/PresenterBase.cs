using System.Collections.Generic;

namespace MAXIMUS.Presentation.PDMS
{
    public abstract class PresenterBase
    {
        #region Member Variables
            protected Dictionary<string, string> m_ErrList = null;

            public static class ReturnType
            {
                public const string VALIDATION_ERROR = "Validation Error";
                public const string ERROR = "Error";
                public const string WARNING = "Warning";
                public const string INFORMATIONAL = "Informational";
            }

            public static class GenericErrorMessage
            {
                public const string UPDATE_FAILED = "An error occurred saving the changes.";
                public const string INSERT_FAILED = "An error occurred saving the changes.";
                public const string RETRIEVAL_FAILED = "An error occurred retrieving the information.";
                public const string VIEW_CANNOT_BE_NULL = "View cannot be null.";
            }

        #endregion

        #region Properties
            public Dictionary<string, string> ErrorList
            {
                get
                {
                    return (this.m_ErrList);
                }
            }

            public bool hasErrors
            {
                get
                {
                    return (ErrorList != null && ErrorList.Count > 0);
                }
            }

            public string NextWarningKey()
            {
                return NextKey(ReturnType.WARNING);
            }

            public string NextErrorKey()
            {
                return NextKey(ReturnType.ERROR);
            }

            public string NextValidationKey()
            {
                return NextKey(ReturnType.VALIDATION_ERROR);
            }

            private string NextKey(string keyType)
            {
                return string.Concat(keyType, (ErrorList.Count + 1).ToString());
            }
        #endregion

        #region Constructors

        public PresenterBase()
		{
			    this.m_ErrList = new Dictionary<string, string>();
		}
		#endregion

        #region Methods
       
        #endregion

    }
}
