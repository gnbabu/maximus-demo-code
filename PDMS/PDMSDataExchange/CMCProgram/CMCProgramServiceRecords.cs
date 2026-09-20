using FileHelpers;
using System;
using System.Globalization;

namespace MAXIMUS.DataExchange.PDMS.CMCProgram
{

    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class CMCProgramServiceRecords
    {
        [FieldQuoted('\'', QuoteMode.AlwaysQuoted)]
        private string practiceMedicaidID;

        [FieldQuoted]
        private string federalTaxID;

        [FieldQuoted]
        private string practiceServiceAddr1;

        [FieldQuoted]
        private string practiceServiceAddr2;

        [FieldQuoted]
        private string practiceServiceCity;

        [FieldQuoted]
        private string practiceServiceState;

        [FieldQuoted]
        private string practiceServiceZip;

        [FieldOptional]
        private string qualEnrollCount;

        [FieldOptional]
        private string error;

        public string PracticeMedicaidID { get => practiceMedicaidID; set => practiceMedicaidID = value; }
        public string FederalTaxID { get => federalTaxID; set => federalTaxID = value; }
        public string PracticeServiceAddr1 { get => practiceServiceAddr1; set => practiceServiceAddr1 = value; }
        public string PracticeServiceAddr2 { get => practiceServiceAddr2; set => practiceServiceAddr2 = value; }
        public string PracticeServiceCity { get => practiceServiceCity; set => practiceServiceCity = value; }
        public string PracticeServiceState { get => practiceServiceState; set => practiceServiceState = value; }
        public string PracticeServiceZip { get => practiceServiceZip; set => practiceServiceZip = value; }
        public string QualEnrollCount { get => qualEnrollCount; set => qualEnrollCount = value; }
        public string Error { get => error; set => error = value; }

    }
}
