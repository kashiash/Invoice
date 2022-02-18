using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.Editors;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.BusinessObjects
{
    [NavigationItem("Automaty")]

    [XafDefaultProperty(nameof(Nazwa))]
    [XafDisplayName("Sql to Excel")]

    public class Sql2Excel : XPObject
    {
        public Sql2Excel(Session session) : base(session)
        { }




        string tematWiadomosci;
        string zapytanieSQL;

        string nazwa;
        bool udostepnijNaListach;

        bool archiwalny;


        [XafDisplayName("Typ obiektu")]
        [ValueConverter(typeof(TypeToStringConverter))]
        [ImmediatePostData]
        [TypeConverter(typeof(LocalizedClassInfoTypeConverter))]
        public Type ObjectType
        {
            get => GetPropertyValue<Type>(nameof(ObjectType));
            set => SetPropertyValue(nameof(ObjectType), value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Nazwa
        {
            get => nazwa;
            set => SetPropertyValue(nameof(Nazwa), ref nazwa, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TematWiadomosci
        {
            get => tematWiadomosci;
            set => SetPropertyValue(nameof(TematWiadomosci), ref tematWiadomosci, value);
        }

        public bool UdostepnijNaListach
        {
            get => udostepnijNaListach;
            set => SetPropertyValue(nameof(UdostepnijNaListach), ref udostepnijNaListach, value);
        }

        public bool Archiwalny
        {
            get => archiwalny;
            set => SetPropertyValue(nameof(Archiwalny), ref archiwalny, value);
        }

        [ElementTypeProperty(nameof(ObjectType))]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        [XafDisplayName("Temat:")]
        public string ZapytanieSQL
        {
            get => zapytanieSQL;
            set => SetPropertyValue(nameof(ZapytanieSQL), ref zapytanieSQL, value);
        }



    }
}
