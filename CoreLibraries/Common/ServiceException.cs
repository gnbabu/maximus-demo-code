using System;
using System.Runtime.Serialization;

namespace MAXIMUS.Services.PDMS
{
    [DataContractAttribute]
    public class ServiceException
    {
        public ServiceException(Exception ex)
        {
            this.Message = ex.Message;
            this.Source = ex.Source;
            this.StackTrace = ex.StackTrace;

            if(ex.InnerException != null)
                this.InnerException = new ServiceException(ex.InnerException);
        }

        [DataMember]
        public ServiceException InnerException;

        [DataMember]
        public string Message;

        [DataMember]
        public string Source;

        [DataMember]
        public string StackTrace;

    }
}