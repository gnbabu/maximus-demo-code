using System.Xml.Serialization;

namespace Corp.Core.Libraries.HospiceReference
{
    public partial class HospiceRequestResponsePayloadBenefitPeriods
    {
        private string segmentIndicatorField;

        private bool isFromInquiry;

        private bool isAllowNewBenPeriod = true;

        private bool isDifferentProdvider;

        private bool isHideBenfitperiodPhy;
        private bool isHideBenfitperiodIDG;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 11)]
        [XmlIgnoreAttribute]
        public string SegmentIndicator
        {
            get
            {
                return this.segmentIndicatorField;
            }
            set
            {
                this.segmentIndicatorField = value;
                this.RaisePropertyChanged("SegmentIndicator");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 12)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsFromInquiry
        {
            get
            {
                return this.isFromInquiry;
            }
            set
            {
                this.isFromInquiry = value;
                this.RaisePropertyChanged("IsFromInquiry");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 13)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsAllowNewBenPeriod
        {
            get
            {
                return this.isAllowNewBenPeriod;
            }
            set
            {
                this.isAllowNewBenPeriod = value;
                this.RaisePropertyChanged("IsAllowNewBenPeriod");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 14)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsDifferentProdvider
        {
            get
            {
                return this.isDifferentProdvider;
            }
            set
            {
                this.isDifferentProdvider = value;
                this.RaisePropertyChanged("IsDifferentProdvider");
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 15)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsHideBenfitperiodPhy
        {
            get
            {
                return this.isHideBenfitperiodPhy;
            }
            set
            {
                this.isHideBenfitperiodPhy = value;
                this.RaisePropertyChanged("IsHideBenfitperiodPhy");
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 16)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsHideBenfitperiodIDG
        {
            get
            {
                return this.isHideBenfitperiodIDG;
            }
            set
            {
                this.isHideBenfitperiodIDG = value;
                this.RaisePropertyChanged("IsHideBenfitperiodIDG");
            }
        }
    }
}
