//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;

namespace CarsDb.Module.BusinessObjects
{

    public partial class car_make : XPLiteObject
    {
        int fid_car_make;
        [Key(false)]
        [Browsable(false)]
        public int id_car_make
        {
            get { return fid_car_make; }
            set { SetPropertyValue<int>("id_car_make", ref fid_car_make, value); }
        }
        string fname;
        [Size(255)]
        [XafDisplayName("Producent")]
        public string name
        {
            get { return fname; }
            set { SetPropertyValue<string>("name", ref fname, value); }
        }

        string fnameCommon;
        [Size(255)]
        [XafDisplayName("Nazwa własna")]
        public string nameCommon
        {
            get { return fnameCommon; }
            set { SetPropertyValue<string>("nameCommon", ref fnameCommon, value); }
        }

        int fdate_create;
        [Browsable(false)]
        public int date_create
        {
            get { return fdate_create; }
            set { SetPropertyValue<int>("date_create", ref fdate_create, value); }
        }
        int fdate_update;
        [Browsable(false)]
        public int date_update
        {
            get { return fdate_update; }
            set { SetPropertyValue<int>("date_update", ref fdate_update, value); }
        }
        car_type fid_car_type;
        [XafDisplayName("Typ")]
        public car_type id_car_type
        {
            get { return fid_car_type; }
            set { SetPropertyValue<car_type>("id_car_type", ref fid_car_type, value); }
        }

        bool archiwalny;
        public bool Archiwalny
        {
            get
            {
                return archiwalny;
            }
            set
            {
                SetPropertyValue("Archiwalny", ref archiwalny, value);
            }
        }
        [XafDisplayName("Modele")]
        [Association(@"car_modelReferencescar_make"), DevExpress.Xpo.Aggregated]
        public XPCollection<car_model> car_models { get { return GetCollection<car_model>("car_models"); } }
    }

}
