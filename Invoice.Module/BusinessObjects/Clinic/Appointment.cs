using System;
using System.Linq;
using System.Text;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;

namespace DevExpress.DentalClinic.Model {
    [DefaultClassOptions]
    public class Appointment : XPObject, IEvent
    {
        public Appointment(Session session) : base(session) { }
        Patient patientCore;
        [Association]
        public Patient Patient {
            get { return patientCore; }
            set { SetPropertyValue(nameof(Patient), ref patientCore, value); }
        }
        Doctor doctorCore;
        [Association]
        public Doctor Doctor {
            get { return doctorCore; }
            set { SetPropertyValue(nameof(Doctor), ref doctorCore, value); }
        }
        DateTime dateCore;
        public DateTime Date {
            get { return dateCore; }
            set { SetPropertyValue(nameof(Date), ref dateCore, value); }
        }
        [Association]
        public XPCollection<ProcedureItem> ProcedureCollection { get { return GetCollection<ProcedureItem>(nameof(ProcedureCollection)); } }
        TimeSpan durationCore;
        public TimeSpan Duration {
            get { return durationCore; }
            set { SetPropertyValue(nameof(Duration), ref durationCore, value); }
        }
        bool allDayEventCore;
        public bool AllDayEvent {
            get { return allDayEventCore; }
            set { SetPropertyValue(nameof(AllDayEvent), ref allDayEventCore, value); }
        }
        AppointmentStatus statusCore;
        public AppointmentStatus Status {
            get { return statusCore; }
            set { SetPropertyValue(nameof(Status), ref statusCore, value); }
        }
        string commentCore;
        public string Comment {
            get { return commentCore; }
            set { SetPropertyValue(nameof(Comment), ref commentCore, value); }
        }
        [NonPersistent]
        public DateTime EndDate {
            get { return Date + durationCore; }
        }
        [NonPersistent]
        public ProcedureGroup ProcedureGroup {
            get {
                var procedure = ProcedureCollection.FirstOrDefault();
                if(procedure != null)
                    return procedure.Procedure.Group;
                return ProcedureGroup.Diagnosis;
            }
        }
        [NonPersistent]
        public string Description { 
            get {
                var descriptionStringBuilder = new StringBuilder();
                foreach(var procedureItem in ProcedureCollection) {
                    descriptionStringBuilder.AppendLine($"• {procedureItem.Procedure.Name}");
                }
                return descriptionStringBuilder.ToString();
            }
        }

        string IEvent.Subject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string IEvent.Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        DateTime IEvent.StartOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        DateTime IEvent.EndOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool IEvent.AllDay { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string IEvent.Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IEvent.Label { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IEvent.Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IEvent.Type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string IEvent.ResourceId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        object IEvent.AppointmentId => throw new NotImplementedException();
    }
    public enum AppointmentStatus { Open, Completed, Failed, Canceled }
}
