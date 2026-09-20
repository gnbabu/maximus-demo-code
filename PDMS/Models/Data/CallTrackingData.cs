using System;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class CallTrackingData
    {
        public CallTrackingData() { }

        public int CallTrackingID { get; set; }

        public string RegID { get; set; } //leaving this as string based on demo - when determine what information will actually be collecting, make value types match.

        public bool NoRegistrationOnFile { get; set; }

        public int SourceID { get; set; }

        public int NextActionID { get; set; } //being used as office location so may want to rename this

        public int SubjectID { get; set; }

        public int ResolutionID { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public TimeSpan Duration { get; set; }

        public string UserName { get; set; }

        public string CaseID { get; set; }

        public string MemberID { get; set; }

        public string CallerOther { get; set; }

        public string ReasonOther { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? LastModifiedUser { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public string CallDetails { get; set; }

        public string NPI { get; set; }

        public string MedicaidID { get; set; }

    }
}
