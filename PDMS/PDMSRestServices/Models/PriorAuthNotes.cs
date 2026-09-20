using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDMSRestServices.Models
{
    public class PriorAuthNotes
    {
        public string NotesType { get; set; }
        public string NoteDesc { get; set; }
        public string NoteStatus { get; set; }
        public string NoteRevieweProviderDesc { get; set; }
        public string ReasonCodeDesc { get; set; }
        public string ReasonCode { get; set; }
        public string NoteAuthType { get; set; }
        public string CreatedUser { get; set; }
        public string InstitutionalSaveID { get; set; }
        public string DentalSaveID { get; set; }
        public string ProfessionalSaveID { get; set; }
        public string MedicaidId { get; set; }
    }

    public class PriorAuthNotesSave
    {
        public int noteId { get; set; }
        public string notesType { get; set; }
        public string noteDesc { get; set; }
        public string noteStatus { get; set; }
        public string noteAuthType { get; set; }
        public string CreatedUser { get; set; }
        public string medicaidId { get; set; }
        public string operation { get; set; }
        public string linkId { get; set; }
    }

    public class ProviderNotes
    {
        public int noteId { get; set; }

        public string Note { get; set; }
    }

    public class NoteResult
    {
        public int ID { get; set; }
        public bool status { get; set; }
    }

}