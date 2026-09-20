namespace MAXIMUS.DataExchange.PDMS
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlType("Attachments_ControlFile")]
    public partial class PAAttachments_ControlFile : object, System.ComponentModel.INotifyPropertyChanged
    {
        private string EDITransaction_TypeField;
        private string PayerRequestedField;
        private string Member_IDField;
        private string PA_NumberField;
        private string Attachment_ControlNumberField;
        private string Provider_IDField;
        private string Provider_NPIField;
        private string SenderIDField;
        private string ReceiverIDField;
        private string Document_TypeField;
        private string DocumentNameField;
        private string UUIDField;
        private string TimeStampField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string EDITransaction_Type
        {
            get
            {
                return this.EDITransaction_TypeField;
            }
            set
            {
                this.EDITransaction_TypeField = value;
                this.RaisePropertyChanged("EDITransaction_Type");
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string PayerRequested
        {
            get
            {
                return this.PayerRequestedField;
            }
            set
            {
                this.PayerRequestedField = value;
                this.RaisePropertyChanged("PayerRequested");
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Member_ID
        {
            get
            {
                return this.Member_IDField;
            }
            set
            {
                this.Member_IDField = value;
                this.RaisePropertyChanged("Member_ID");
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string PA_number
        {
            get
            {
                return this.PA_NumberField;
            }
            set
            {
                this.PA_NumberField = value;
                this.RaisePropertyChanged("PA_NUMBER");
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string Attachment_ControlNumber
        {
            get
            {
                return this.Attachment_ControlNumberField;
            }
            set
            {
                this.Attachment_ControlNumberField = value;
                this.RaisePropertyChanged("Attachment_ControlNumber");
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string Provider_ID
        {
            get
            {
                return this.Provider_IDField;
            }
            set
            {
                this.Provider_IDField = value;
                this.RaisePropertyChanged("Provider_ID");
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string Provider_NPI
        {
            get
            {
                return this.Provider_NPIField;
            }
            set
            {
                this.Provider_NPIField = value;
                this.RaisePropertyChanged("Provider_NPI");
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string SenderID
        {
            get
            {
                return this.SenderIDField;
            }
            set
            {
                this.SenderIDField = value;
                this.RaisePropertyChanged("SenderID");
            }
        }
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string ReceiverID
        {
            get
            {
                return this.ReceiverIDField;
            }
            set
            {
                this.ReceiverIDField = value;
                this.RaisePropertyChanged("ReceiverID");
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public string DocumentType
        {
            get
            {
                return this.Document_TypeField;
            }
            set
            {
                this.Document_TypeField = value;
                this.RaisePropertyChanged("DOCUMENTTYPE");
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public string DocumentName
        {
            get
            {
                return this.DocumentNameField;
            }
            set
            {
                this.DocumentNameField = value;
                this.RaisePropertyChanged("DocumentName");
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string UUID
        {
            get
            {
                return this.UUIDField;
            }
            set
            {
                this.UUIDField = value;
                this.RaisePropertyChanged("UUID");
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string Timestamp
        {
            get
            {
                return this.TimeStampField;
            }
            set
            {
                this.TimeStampField = value;
                this.RaisePropertyChanged("Timestamp");
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
}
