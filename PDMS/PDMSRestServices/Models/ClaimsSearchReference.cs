namespace PDMSRestServices.Models.ClaimsSearch
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://service.operationmgmt.fi/ClaimsService/search", ConfigurationName = "FI.ClaimsSearchReference.ClaimsSearchService")]
    public interface ClaimsSearchService
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://service.operationmgmt.fi/ClaimsService/SearchClaims", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        SearchClaimsResponse SearchClaims(SearchClaimsRequest request);

        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action = "http://service.operationmgmt.fi/ClaimsService/SearchClaims", ReplyAction = "*")]
        System.Threading.Tasks.Task<SearchClaimsResponse> SearchClaimsAsync(SearchClaimsRequest request);
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.operationmgmt.fi/ClaimsService/search")]
    public partial class MessageHeaderType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private MessageHeaderTypeBusinessFlow businessFlowField;

        private string stateCodeField;

        private MessageHeaderTypeRequestorSystem requestorSystemField;

        private MessageHeaderTypeSubscriber[] subscriberSystemField;

        private string moduleTransactionIdField;

        private string additionalModuleTransactionIdField;

        private System.DateTime requestTimestampField;

        private string sITransactionKeyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public MessageHeaderTypeBusinessFlow BusinessFlow
        {
            get
            {
                return this.businessFlowField;
            }
            set
            {
                this.businessFlowField = value;
                this.RaisePropertyChanged("BusinessFlow");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string StateCode
        {
            get
            {
                return this.stateCodeField;
            }
            set
            {
                this.stateCodeField = value;
                this.RaisePropertyChanged("StateCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public MessageHeaderTypeRequestorSystem RequestorSystem
        {
            get
            {
                return this.requestorSystemField;
            }
            set
            {
                this.requestorSystemField = value;
                this.RaisePropertyChanged("RequestorSystem");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Subscriber", IsNullable = false)]
        public MessageHeaderTypeSubscriber[] SubscriberSystem
        {
            get
            {
                return this.subscriberSystemField;
            }
            set
            {
                this.subscriberSystemField = value;
                this.RaisePropertyChanged("SubscriberSystem");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string ModuleTransactionId
        {
            get
            {
                return this.moduleTransactionIdField;
            }
            set
            {
                this.moduleTransactionIdField = value;
                this.RaisePropertyChanged("ModuleTransactionId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string AdditionalModuleTransactionId
        {
            get
            {
                return this.additionalModuleTransactionIdField;
            }
            set
            {
                this.additionalModuleTransactionIdField = value;
                this.RaisePropertyChanged("AdditionalModuleTransactionId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public System.DateTime RequestTimestamp
        {
            get
            {
                return this.requestTimestampField;
            }
            set
            {
                this.requestTimestampField = value;
                this.RaisePropertyChanged("RequestTimestamp");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string SITransactionKey
        {
            get
            {
                return this.sITransactionKeyField;
            }
            set
            {
                this.sITransactionKeyField = value;
                this.RaisePropertyChanged("SITransactionKey");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://service.operationmgmt.fi/ClaimsService/search")]
    public enum MessageHeaderTypeBusinessFlow
    {

        /// <remarks/>
        SearchClaims,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://service.operationmgmt.fi/ClaimsService/search")]
    public enum MessageHeaderTypeRequestorSystem
    {

        /// <remarks/>
        PNM,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://service.operationmgmt.fi/ClaimsService/search")]
    public enum MessageHeaderTypeSubscriber
    {

        /// <remarks/>
        FI,

        /// <remarks/>
        EDI,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.operationmgmt.fi/ClaimsService/search")]
    public partial class ErrorDetailsType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string errorCodeField;

        private string errorDescriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
                this.RaisePropertyChanged("ErrorCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ErrorDescription
        {
            get
            {
                return this.errorDescriptionField;
            }
            set
            {
                this.errorDescriptionField = value;
                this.RaisePropertyChanged("ErrorDescription");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.operationmgmt.fi/ClaimsService/search")]
    public partial class ClaimHeaderResponseType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string payorTypeField;

        private string iCNField;

        private string claimTypeField;

        private string claimStatusField;

        private string patientAccountNumberField;

        private string memberIdField;

        private string memberNameField;

        private decimal totalPaidAmountField;

        private bool totalPaidAmountFieldSpecified;

        private System.DateTime claimPaidDateField;

        private bool claimPaidDateFieldSpecified;

        private System.DateTime adjudicationDateField;

        private bool adjudicationDateFieldSpecified;

        private System.DateTime claimSubmissionDateField;

        private decimal totalChargesField;

        private System.DateTime fromDOSField;

        private System.DateTime thruDOSField;

        private string dRGCodeField;

        private string originalClaimICNField;

        private string originalClaimAdjustmentReasonField;

        private System.DateTime remittanceAdviceDateField;

        private bool remittanceAdviceDateFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string PayorType
        {
            get
            {
                return this.payorTypeField;
            }
            set
            {
                this.payorTypeField = value;
                this.RaisePropertyChanged("PayorType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ICN
        {
            get
            {
                return this.iCNField;
            }
            set
            {
                this.iCNField = value;
                this.RaisePropertyChanged("ICN");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ClaimType
        {
            get
            {
                return this.claimTypeField;
            }
            set
            {
                this.claimTypeField = value;
                this.RaisePropertyChanged("ClaimType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string ClaimStatus
        {
            get
            {
                return this.claimStatusField;
            }
            set
            {
                this.claimStatusField = value;
                this.RaisePropertyChanged("ClaimStatus");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string PatientAccountNumber
        {
            get
            {
                return this.patientAccountNumberField;
            }
            set
            {
                this.patientAccountNumberField = value;
                this.RaisePropertyChanged("PatientAccountNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string MemberId
        {
            get
            {
                return this.memberIdField;
            }
            set
            {
                this.memberIdField = value;
                this.RaisePropertyChanged("MemberId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string MemberName
        {
            get
            {
                return this.memberNameField;
            }
            set
            {
                this.memberNameField = value;
                this.RaisePropertyChanged("MemberName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public decimal TotalPaidAmount
        {
            get
            {
                return this.totalPaidAmountField;
            }
            set
            {
                this.totalPaidAmountField = value;
                this.RaisePropertyChanged("TotalPaidAmount");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TotalPaidAmountSpecified
        {
            get
            {
                return this.totalPaidAmountFieldSpecified;
            }
            set
            {
                this.totalPaidAmountFieldSpecified = value;
                this.RaisePropertyChanged("TotalPaidAmountSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", Order = 8)]
        public System.DateTime ClaimPaidDate
        {
            get
            {
                return this.claimPaidDateField;
            }
            set
            {
                this.claimPaidDateField = value;
                this.RaisePropertyChanged("ClaimPaidDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ClaimPaidDateSpecified
        {
            get
            {
                return this.claimPaidDateFieldSpecified;
            }
            set
            {
                this.claimPaidDateFieldSpecified = value;
                this.RaisePropertyChanged("ClaimPaidDateSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", Order = 9)]
        public System.DateTime AdjudicationDate
        {
            get
            {
                return this.adjudicationDateField;
            }
            set
            {
                this.adjudicationDateField = value;
                this.RaisePropertyChanged("AdjudicationDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AdjudicationDateSpecified
        {
            get
            {
                return this.adjudicationDateFieldSpecified;
            }
            set
            {
                this.adjudicationDateFieldSpecified = value;
                this.RaisePropertyChanged("AdjudicationDateSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", Order = 10)]
        public System.DateTime ClaimSubmissionDate
        {
            get
            {
                return this.claimSubmissionDateField;
            }
            set
            {
                this.claimSubmissionDateField = value;
                this.RaisePropertyChanged("ClaimSubmissionDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public decimal TotalCharges
        {
            get
            {
                return this.totalChargesField;
            }
            set
            {
                this.totalChargesField = value;
                this.RaisePropertyChanged("TotalCharges");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", Order = 12)]
        public System.DateTime FromDOS
        {
            get
            {
                return this.fromDOSField;
            }
            set
            {
                this.fromDOSField = value;
                this.RaisePropertyChanged("FromDOS");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", Order = 13)]
        public System.DateTime ThruDOS
        {
            get
            {
                return this.thruDOSField;
            }
            set
            {
                this.thruDOSField = value;
                this.RaisePropertyChanged("ThruDOS");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public string DRGCode
        {
            get
            {
                return this.dRGCodeField;
            }
            set
            {
                this.dRGCodeField = value;
                this.RaisePropertyChanged("DRGCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public string OriginalClaimICN
        {
            get
            {
                return this.originalClaimICNField;
            }
            set
            {
                this.originalClaimICNField = value;
                this.RaisePropertyChanged("OriginalClaimICN");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public string OriginalClaimAdjustmentReason
        {
            get
            {
                return this.originalClaimAdjustmentReasonField;
            }
            set
            {
                this.originalClaimAdjustmentReasonField = value;
                this.RaisePropertyChanged("OriginalClaimAdjustmentReason");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", Order = 17)]
        public System.DateTime RemittanceAdviceDate
        {
            get
            {
                return this.remittanceAdviceDateField;
            }
            set
            {
                this.remittanceAdviceDateField = value;
                this.RaisePropertyChanged("RemittanceAdviceDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RemittanceAdviceDateSpecified
        {
            get
            {
                return this.remittanceAdviceDateFieldSpecified;
            }
            set
            {
                this.remittanceAdviceDateFieldSpecified = value;
                this.RaisePropertyChanged("RemittanceAdviceDateSpecified");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.operationmgmt.fi/ClaimsService/search")]
    public partial class ResponseHeaderType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string sITransactionKeyField;

        private string moduleTransactionIdField;

        private string additionalModuleTransactionIdField;

        private string responseCodeField;

        private ResponseHeaderTypeResponseType responseTypeField;

        private bool responseTypeFieldSpecified;

        private string responseMessageField;

        private string responseDetailsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string SITransactionKey
        {
            get
            {
                return this.sITransactionKeyField;
            }
            set
            {
                this.sITransactionKeyField = value;
                this.RaisePropertyChanged("SITransactionKey");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ModuleTransactionId
        {
            get
            {
                return this.moduleTransactionIdField;
            }
            set
            {
                this.moduleTransactionIdField = value;
                this.RaisePropertyChanged("ModuleTransactionId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string AdditionalModuleTransactionId
        {
            get
            {
                return this.additionalModuleTransactionIdField;
            }
            set
            {
                this.additionalModuleTransactionIdField = value;
                this.RaisePropertyChanged("AdditionalModuleTransactionId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string ResponseCode
        {
            get
            {
                return this.responseCodeField;
            }
            set
            {
                this.responseCodeField = value;
                this.RaisePropertyChanged("ResponseCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public ResponseHeaderTypeResponseType ResponseType
        {
            get
            {
                return this.responseTypeField;
            }
            set
            {
                this.responseTypeField = value;
                this.RaisePropertyChanged("ResponseType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ResponseTypeSpecified
        {
            get
            {
                return this.responseTypeFieldSpecified;
            }
            set
            {
                this.responseTypeFieldSpecified = value;
                this.RaisePropertyChanged("ResponseTypeSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string ResponseMessage
        {
            get
            {
                return this.responseMessageField;
            }
            set
            {
                this.responseMessageField = value;
                this.RaisePropertyChanged("ResponseMessage");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string ResponseDetails
        {
            get
            {
                return this.responseDetailsField;
            }
            set
            {
                this.responseDetailsField = value;
                this.RaisePropertyChanged("ResponseDetails");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://service.operationmgmt.fi/ClaimsService/search")]
    public enum ResponseHeaderTypeResponseType
    {

        /// <remarks/>
        SUCCESS,

        /// <remarks/>
        FAILURE,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.operationmgmt.fi/ClaimsService/search")]
    public partial class SearchClaimsRequestPayloadType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string payorTypeField;

        private string iCNField;

        private string patientAccountNumberField;

        private string memberMedicaidIdField;

        private string renderingProviderIDField;

        private string billingProviderIDField;

        private string prescriptionNumberField;

        private string claimTypeField;

        private string statusField;

        private decimal totalChargesField;

        private bool totalChargesFieldSpecified;

        private System.DateTime fromDOSField;

        private bool fromDOSFieldSpecified;

        private System.DateTime thruDOSField;

        private bool thruDOSFieldSpecified;

        private System.DateTime remittanceAdviceDateField;

        private bool remittanceAdviceDateFieldSpecified;

        private string maxRecordsField;

        private string offsetField;

        private string pageSizeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string PayorType
        {
            get
            {
                return this.payorTypeField;
            }
            set
            {
                this.payorTypeField = value;
                this.RaisePropertyChanged("PayorType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ICN
        {
            get
            {
                return this.iCNField;
            }
            set
            {
                this.iCNField = value;
                this.RaisePropertyChanged("ICN");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string PatientAccountNumber
        {
            get
            {
                return this.patientAccountNumberField;
            }
            set
            {
                this.patientAccountNumberField = value;
                this.RaisePropertyChanged("PatientAccountNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string MemberMedicaidId
        {
            get
            {
                return this.memberMedicaidIdField;
            }
            set
            {
                this.memberMedicaidIdField = value;
                this.RaisePropertyChanged("MemberMedicaidId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string RenderingProviderID
        {
            get
            {
                return this.renderingProviderIDField;
            }
            set
            {
                this.renderingProviderIDField = value;
                this.RaisePropertyChanged("RenderingProviderID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string BillingProviderID
        {
            get
            {
                return this.billingProviderIDField;
            }
            set
            {
                this.billingProviderIDField = value;
                this.RaisePropertyChanged("BillingProviderID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string PrescriptionNumber
        {
            get
            {
                return this.prescriptionNumberField;
            }
            set
            {
                this.prescriptionNumberField = value;
                this.RaisePropertyChanged("PrescriptionNumber");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string ClaimType
        {
            get
            {
                return this.claimTypeField;
            }
            set
            {
                this.claimTypeField = value;
                this.RaisePropertyChanged("ClaimType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
                this.RaisePropertyChanged("Status");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public decimal TotalCharges
        {
            get
            {
                return this.totalChargesField;
            }
            set
            {
                this.totalChargesField = value;
                this.RaisePropertyChanged("TotalCharges");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TotalChargesSpecified
        {
            get
            {
                return this.totalChargesFieldSpecified;
            }
            set
            {
                this.totalChargesFieldSpecified = value;
                this.RaisePropertyChanged("TotalChargesSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", Order = 10)]
        public System.DateTime FromDOS
        {
            get
            {
                return this.fromDOSField;
            }
            set
            {
                this.fromDOSField = value;
                this.RaisePropertyChanged("FromDOS");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FromDOSSpecified
        {
            get
            {
                return this.fromDOSFieldSpecified;
            }
            set
            {
                this.fromDOSFieldSpecified = value;
                this.RaisePropertyChanged("FromDOSSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", Order = 11)]
        public System.DateTime ThruDOS
        {
            get
            {
                return this.thruDOSField;
            }
            set
            {
                this.thruDOSField = value;
                this.RaisePropertyChanged("ThruDOS");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ThruDOSSpecified
        {
            get
            {
                return this.thruDOSFieldSpecified;
            }
            set
            {
                this.thruDOSFieldSpecified = value;
                this.RaisePropertyChanged("ThruDOSSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", Order = 12)]
        public System.DateTime RemittanceAdviceDate
        {
            get
            {
                return this.remittanceAdviceDateField;
            }
            set
            {
                this.remittanceAdviceDateField = value;
                this.RaisePropertyChanged("RemittanceAdviceDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RemittanceAdviceDateSpecified
        {
            get
            {
                return this.remittanceAdviceDateFieldSpecified;
            }
            set
            {
                this.remittanceAdviceDateFieldSpecified = value;
                this.RaisePropertyChanged("RemittanceAdviceDateSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 13)]
        public string MaxRecords
        {
            get
            {
                return this.maxRecordsField;
            }
            set
            {
                this.maxRecordsField = value;
                this.RaisePropertyChanged("MaxRecords");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 14)]
        public string Offset
        {
            get
            {
                return this.offsetField;
            }
            set
            {
                this.offsetField = value;
                this.RaisePropertyChanged("Offset");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 15)]
        public string PageSize
        {
            get
            {
                return this.pageSizeField;
            }
            set
            {
                this.pageSizeField = value;
                this.RaisePropertyChanged("PageSize");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SearchClaimsRequest", WrapperNamespace = "http://service.operationmgmt.fi/ClaimsService/search", IsWrapped = true)]
    public partial class SearchClaimsRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.operationmgmt.fi/ClaimsService/search", Order = 0)]
        public MessageHeaderType MessageHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.operationmgmt.fi/ClaimsService/search", Order = 1)]
        public SearchClaimsRequestPayloadType RequestPayload;

        public SearchClaimsRequest()
        {
        }

        public SearchClaimsRequest(MessageHeaderType MessageHeader,SearchClaimsRequestPayloadType RequestPayload)
        {
            this.MessageHeader = MessageHeader;
            this.RequestPayload = RequestPayload;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SearchClaimsResponse", WrapperNamespace = "http://service.operationmgmt.fi/ClaimsService/search", IsWrapped = true)]
    public partial class SearchClaimsResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.operationmgmt.fi/ClaimsService/search", Order = 0)]
        public  ResponseHeaderType ResponseHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.operationmgmt.fi/ClaimsService/search", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ClaimHeaderResponse")]
        public  ClaimHeaderResponseType[] ClaimHeaderResponse;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.operationmgmt.fi/ClaimsService/search", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string Offset;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.operationmgmt.fi/ClaimsService/search", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string TotalRecords;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://service.operationmgmt.fi/ClaimsService/search", Order = 4)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ErrorDetails", IsNullable = false)]
        public  ErrorDetailsType[] Errors;

        public SearchClaimsResponse()
        {
        }

        public SearchClaimsResponse(ResponseHeaderType ResponseHeader, ClaimHeaderResponseType[] ClaimHeaderResponse, string Offset, string TotalRecords, ErrorDetailsType[] Errors)
        {
            this.ResponseHeader = ResponseHeader;
            this.ClaimHeaderResponse = ClaimHeaderResponse;
            this.Offset = Offset;
            this.TotalRecords = TotalRecords;
            this.Errors = Errors;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ClaimsSearchServiceChannel : ClaimsSearchService, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ClaimsSearchServiceClient : System.ServiceModel.ClientBase<ClaimsSearchService>, ClaimsSearchService
    {

        public ClaimsSearchServiceClient()
        {
        }

        public ClaimsSearchServiceClient(string endpointConfigurationName) :
                base(endpointConfigurationName)
        {
        }

        public ClaimsSearchServiceClient(string endpointConfigurationName, string remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public ClaimsSearchServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public ClaimsSearchServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SearchClaimsResponse ClaimsSearchService.SearchClaims(SearchClaimsRequest request)
        {
            return base.Channel.SearchClaims(request);
        }

        public ResponseHeaderType SearchClaims(MessageHeaderType MessageHeader, SearchClaimsRequestPayloadType RequestPayload, out ClaimHeaderResponseType[] ClaimHeaderResponse, out string Offset, out string TotalRecords, out ErrorDetailsType[] Errors)
        {
            SearchClaimsRequest inValue = new SearchClaimsRequest();
            inValue.MessageHeader = MessageHeader;
            inValue.RequestPayload = RequestPayload;
            SearchClaimsResponse retVal = ((ClaimsSearchService)(this)).SearchClaims(inValue);
            ClaimHeaderResponse = retVal.ClaimHeaderResponse;
            ClaimHeaderResponse = retVal.ClaimHeaderResponse;
            Offset = retVal.Offset;
            TotalRecords = retVal.TotalRecords;
            Errors = retVal.Errors;
            return retVal.ResponseHeader;
        }

        public System.Threading.Tasks.Task<SearchClaimsResponse> SearchClaimsAsync(SearchClaimsRequest request)
        {
            return base.Channel.SearchClaimsAsync(request);
        }
    }
}
