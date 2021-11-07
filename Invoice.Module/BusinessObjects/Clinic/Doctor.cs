namespace DevExpress.DentalClinic.Model {
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using DevExpress.Persistent.Base;
    using DevExpress.Persistent.BaseImpl;
    using DevExpress.Xpo;


    [DefaultClassOptions]
    public class Doctor : BaseObject {
        public Doctor(Session session) 
            : base(session) {
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            addressCore = new Address(Session);
    
        }
        //
        string firstNameCore;
        public string FirstName
        {
            get { return firstNameCore; }
            set { SetPropertyValue(nameof(FirstName), ref firstNameCore, value); }
        }
        string lastNameCore;
        public string LastName
        {
            get { return lastNameCore; }
            set { SetPropertyValue(nameof(LastName), ref lastNameCore, value); }
        }
        [PersistentAlias("Concat(FirstName, ' ', LastName)")]
        public string FullName
        {
            get { return EvaluateAlias("FullName") as string; }
        }
        DateTime birthdayCore;
        public DateTime Birthday
        {
            get { return birthdayCore; }
            set { SetPropertyValue(nameof(Birthday), ref birthdayCore, value); }
        }
        string emailCore;
        public string Email
        {
            get { return emailCore; }
            set { SetPropertyValue(nameof(Email), ref emailCore, value); }
        }
        string phoneCore;
        public string Phone
        {
            get { return phoneCore; }
            set { SetPropertyValue(nameof(Phone), ref phoneCore, value); }
        }

        Address addressCore;
        [Aggregated]
        public Address Address
        {
            get { return addressCore; }
            set { SetPropertyValue(nameof(Address), ref addressCore, value); }
        }
        string notesCore;
        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get { return notesCore; }
            set { SetPropertyValue(nameof(Notes), ref notesCore, value); }
        }

        [Association]
        public XPCollection<Appointment> AppointmentCollection {
            get { return GetCollection<Appointment>(); }
        }
    }
}
