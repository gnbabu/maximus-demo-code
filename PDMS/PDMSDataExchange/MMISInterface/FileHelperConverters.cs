using FileHelpers;

namespace MAXIMUS.DataExchange.PDMS.MMISInterface
{
    public class LicCertArrayConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            return "";
        }

        public override string FieldToString(object from)
        {
            SubmitProviderLicense[] licenses = (SubmitProviderLicense[])from;
            FixedFileEngine engine = new FixedFileEngine(typeof(SubmitProviderLicense));
                      
            return engine.WriteString(licenses).Replace("\r\n", "");
        }
    }

    public class StatusArrayConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            return "";
        }

        public override string FieldToString(object from)
        {
            SubmitProviderStatus[] statuses = (SubmitProviderStatus[])from;
            FixedFileEngine engine = new FixedFileEngine(typeof(SubmitProviderStatus));

            return engine.WriteString(statuses).Replace("\r\n", "");
        }
    }
    public class ProviderCOSArrayConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            return "";
        }

        public override string FieldToString(object from)
        {
            SubmitProviderCOS[] statuses = (SubmitProviderCOS[])from;
            FixedFileEngine engine = new FixedFileEngine(typeof(SubmitProviderCOS));

            return engine.WriteString(statuses).Replace("\r\n", "");
        }
    }
    public class ProviderSpecialtyArrayConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            return "";
        }

        public override string FieldToString(object from)
        {
            SubmitProviderSpecialty[] specs = (SubmitProviderSpecialty[])from;
            FixedFileEngine engine = new FixedFileEngine(typeof(SubmitProviderSpecialty));

            return engine.WriteString(specs).Replace("\r\n", "");
        }
    }
    public class ProviderTaxonomyArrayConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            return "";
        }

        public override string FieldToString(object from)
        {
            SubmitProviderTaxonomy[] specs = (SubmitProviderTaxonomy[])from;
            FixedFileEngine engine = new FixedFileEngine(typeof(SubmitProviderTaxonomy));

            return engine.WriteString(specs).Replace("\r\n", "");
        }
    }
}

