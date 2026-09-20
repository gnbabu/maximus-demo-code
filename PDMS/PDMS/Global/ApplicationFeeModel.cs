using System;

[Serializable]
    public partial class ApplicationFeeModel
    {
       
        public ApplicationFeeModel() { }
        public string transactionNumber { get; set; }

        public string statusCode { get; set; }

        public string transactionStatus { get; set; }
        
    }

