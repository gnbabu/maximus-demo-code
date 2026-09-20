using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDMSWebAPI.ProviderApplicationStatusInq
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://hp.com/OhioMITS/Interfaces/AppStatusInquiry/ProvideAppStatusInquiryDataImp" +
            "l/Processes/IVR/Starter", ConfigurationName = "ProviderApplicationStatusInq.AppStatusInquiryportType")]
    public interface AppStatusInquiryportType
    {

        [System.ServiceModel.OperationContractAttribute(Action = "/Processes/IVR/Starter/ProvideAppStatusInquiryData", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        PDMSWebAPI.ProviderApplicationStatusInq.ProvideAppStatusInquiryDataResponse ProvideAppStatusInquiryData(PDMSWebAPI.ProviderApplicationStatusInq.ProvideAppStatusInquiryDataRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "/Processes/IVR/Starter/ProvideAppStatusInquiryData", ReplyAction = "*")]
        System.Threading.Tasks.Task<PDMSWebAPI.ProviderApplicationStatusInq.ProvideAppStatusInquiryDataResponse> ProvideAppStatusInquiryDataAsync(PDMSWebAPI.ProviderApplicationStatusInq.ProvideAppStatusInquiryDataRequest request);
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://hp.com/OhioMITS/Interfaces/AppStatusInquiryRequest")]
    public partial class AppStatusInquiryRequestMessageHeader : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string businessFlowField;

        private string stateCodeField;

        private string requestorSystemField;

        private string moduleTransactionIdField;

        private string subscriberSystemField;

        private string additionalModuleTransactionIdField;

        private string sITransactionKeyField;

        private string requestTimestampField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string BusinessFlow
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
        public string RequestorSystem
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string SubscriberSystem
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
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string RequestTimestamp
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://hp.com/OhioMITS/Interfaces/AppStatusInquiryRequest")]
    public partial class AppStatusInquiryRequestAppStatusInquiry : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string atnField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string atn
        {
            get
            {
                return this.atnField;
            }
            set
            {
                this.atnField = value;
                this.RaisePropertyChanged("atn");
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://hp.com/OhioMITS/Interfaces/AppStatusInquiryResponse")]
    public partial class AppStatusInquiryResponseAppStatusInquiryError : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string errorCodeField;

        private string errorDescriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string errorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
                this.RaisePropertyChanged("errorCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string errorDescription
        {
            get
            {
                return this.errorDescriptionField;
            }
            set
            {
                this.errorDescriptionField = value;
                this.RaisePropertyChanged("errorDescription");
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://hp.com/OhioMITS/Interfaces/AppStatusInquiryResponse")]
    public partial class AppStatusInquiryResponseAppStatusInquiry : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string appStatusCodeField;

        private string appStatusDescpField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string appStatusCode
        {
            get
            {
                return this.appStatusCodeField;
            }
            set
            {
                this.appStatusCodeField = value;
                this.RaisePropertyChanged("appStatusCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string appStatusDescription
        {
            get
            {
                return this.appStatusDescpField;
            }
            set
            {
                this.appStatusDescpField = value;
                this.RaisePropertyChanged("appStatusDescription");
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://hp.com/OhioMITS/Interfaces/AppStatusInquiryResponse")]
    public enum AppStatusInquiryResponseAppStatusInquiryAppStatus
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute(" ")]
        Item0,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Item5,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("6")]
        Item6,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("7")]
        Item7,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("8")]
        Item8,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("9")]
        Item9,

        /// <remarks/>
        B,

        /// <remarks/>
        N,

        /// <remarks/>
        R,

        /// <remarks/>
        W,

        /// <remarks/>
        V,
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "AppStatusInquiryRequest", WrapperNamespace = "http://hp.com/OhioMITS/Interfaces/AppStatusInquiryRequest", IsWrapped = true)]
    public partial class ProvideAppStatusInquiryDataRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://hp.com/OhioMITS/Interfaces/AppStatusInquiryRequest", Order = 0)]
        public PDMSWebAPI.ProviderApplicationStatusInq.AppStatusInquiryRequestMessageHeader MessageHeader;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://hp.com/OhioMITS/Interfaces/AppStatusInquiryRequest", Order = 1)]
        public PDMSWebAPI.ProviderApplicationStatusInq.AppStatusInquiryRequestAppStatusInquiry appStatusInquiry;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://hp.com/OhioMITS/Interfaces/AppStatusInquiryRequest", Order = 2)]
        public string clientsecuritykey;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://hp.com/OhioMITS/Interfaces/AppStatusInquiryRequest", Order = 3)]
        public string transactionId;

        public ProvideAppStatusInquiryDataRequest()
        {
        }

        public ProvideAppStatusInquiryDataRequest(PDMSWebAPI.ProviderApplicationStatusInq.AppStatusInquiryRequestMessageHeader MessageHeader, PDMSWebAPI.ProviderApplicationStatusInq.AppStatusInquiryRequestAppStatusInquiry appStatusInquiry, string clientsecuritykey, string transactionId)
        {
            this.MessageHeader = MessageHeader;
            this.appStatusInquiry = appStatusInquiry;
            this.clientsecuritykey = clientsecuritykey;
            this.transactionId = transactionId;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "AppStatusInquiryResponse", WrapperNamespace = "http://hp.com/OhioMITS/Interfaces/AppStatusInquiryResponse", IsWrapped = false)]
    
    public partial class ProvideAppStatusInquiryDataResponse
    {
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://hp.com/OhioMITS/Interfaces/AppStatusInquiryResponse", Order = 0)]
        public AppStatusInquiryResponse AppStatusInquiryResponse;

        public ProvideAppStatusInquiryDataResponse()
        {
        }

        public ProvideAppStatusInquiryDataResponse(AppStatusInquiryResponse AppStatusInquiryResponse)
        {
            this.AppStatusInquiryResponse = AppStatusInquiryResponse;
        }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://hp.com/OhioMITS/Interfaces/AppStatusInquiryResponse")]
    public partial class AppStatusInquiryResponse
    {

        private string sITransactionKeyField;

        private AppStatusInquiryResponseAppStatusInquiryError appStatusInquiryErrorField;

        private AppStatusInquiryResponseAppStatusInquiry appStatusInquiryField;

        private string responseStatusField;

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
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public AppStatusInquiryResponseAppStatusInquiryError appStatusInquiryError
        {
            get
            {
                return this.appStatusInquiryErrorField;
            }
            set
            {
                this.appStatusInquiryErrorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public AppStatusInquiryResponseAppStatusInquiry appStatusInquiry
        {
            get
            {
                return this.appStatusInquiryField;
            }
            set
            {
                this.appStatusInquiryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string responseStatus
        {
            get
            {
                return this.responseStatusField;
            }
            set
            {
                this.responseStatusField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface AppStatusInquiryportTypeChannel : PDMSWebAPI.ProviderApplicationStatusInq.AppStatusInquiryportType, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AppStatusInquiryportTypeClient : System.ServiceModel.ClientBase<PDMSWebAPI.ProviderApplicationStatusInq.AppStatusInquiryportType>, PDMSWebAPI.ProviderApplicationStatusInq.AppStatusInquiryportType
    {

        public AppStatusInquiryportTypeClient()
        {
        }

        public AppStatusInquiryportTypeClient(string endpointConfigurationName) :
                base(endpointConfigurationName)
        {
        }

        public AppStatusInquiryportTypeClient(string endpointConfigurationName, string remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public AppStatusInquiryportTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public AppStatusInquiryportTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public PDMSWebAPI.ProviderApplicationStatusInq.ProvideAppStatusInquiryDataResponse ProvideAppStatusInquiryData(PDMSWebAPI.ProviderApplicationStatusInq.ProvideAppStatusInquiryDataRequest request)
        {
            return base.Channel.ProvideAppStatusInquiryData(request);
        }

        public System.Threading.Tasks.Task<PDMSWebAPI.ProviderApplicationStatusInq.ProvideAppStatusInquiryDataResponse> ProvideAppStatusInquiryDataAsync(PDMSWebAPI.ProviderApplicationStatusInq.ProvideAppStatusInquiryDataRequest request)
        {
            return base.Channel.ProvideAppStatusInquiryDataAsync(request);
        }
    }
}