using System.Xml.Serialization;

namespace Corp.Core.Libraries.AttachmentServiceReference
{
    public partial class SendAttachment
    {
        private int benPeriodField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        [XmlIgnoreAttribute]
        public int BenPeriod
        {
            get
            {
                return this.benPeriodField;
            }
            set
            {
                this.benPeriodField = value;
                this.RaisePropertyChanged("BenPeriod");
            }
        }
    }
}
