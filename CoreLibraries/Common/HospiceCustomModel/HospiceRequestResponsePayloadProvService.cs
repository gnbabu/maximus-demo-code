using System.Xml.Serialization;

namespace Corp.Core.Libraries.HospiceReference
{
    public partial class HospiceRequestResponsePayloadProvService
    {
        private string segmentIndicatorField;

        private bool isFromInquiry;

        private int benPeriod;

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
                this.RaisePropertyChanged("isFromInquiry");
            }
        }

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 13)]
        //[XmlIgnoreAttribute]
        //public int BenPeriod
        //{
        //    get
        //    {
        //        return this.benPeriod;
        //    }
        //    set
        //    {
        //        this.benPeriod = value;
        //        this.RaisePropertyChanged("BenPeriod");
        //    }
        //}
    }
}
