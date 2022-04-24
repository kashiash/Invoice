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
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;

namespace Invoice.Module.BusinessObjects
{
    [NavigationItem("Automaty")]

    [XafDefaultProperty(nameof(Name))]
    [XafDisplayName("Export Sql to Excel")]

    public class Sql2Excel : XPObject
    {
        public Sql2Excel(Session session) : base(session)
        { }

        string description;
        string fileName;
        string query;
        string nazwa;
        bool inPlace;
        bool archiwalny;


        [ValueConverter(typeof(TypeToStringConverter))]
        [ImmediatePostData]
        [TypeConverter(typeof(LocalizedClassInfoTypeConverter))]
        [RuleRequiredField]
        public Type ObjectType
        {
            get => GetPropertyValue<Type>(nameof(ObjectType));
            set => SetPropertyValue(nameof(ObjectType), value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => nazwa;
            set => SetPropertyValue(nameof(Name), ref nazwa, value);
        }


        
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        [VisibleInListView(false)]
        [ValueConverter(typeof(TypeToStringConverter))]
        [TypeConverter(typeof(ExportParametersObjectTypeConverter))]
        [Localizable(true)]
        public Type ParametersObjectType
        {

            get => GetPropertyValue<Type>(nameof(ParametersObjectType));
            set => SetPropertyValue(nameof(ParametersObjectType), value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [ElementTypeProperty(nameof(ObjectType))]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]
        [RuleRequiredField(DefaultContexts.Save)]
        public string ExportedFileName
        {
            get => fileName;
            set => SetPropertyValue(nameof(ExportedFileName), ref fileName, value);
        }

        public bool InPlace
        {
            get => inPlace;
            set => SetPropertyValue(nameof(InPlace), ref inPlace, value);
        }

        public bool Archived
        {
            get => archiwalny;
            set => SetPropertyValue(nameof(Archived), ref archiwalny, value);
        }


        [Size(SizeAttribute.Unlimited)]
        [ElementTypeProperty(nameof(ObjectType))]
        [EditorAlias(EditorAliases.PopupExpressionPropertyEditor)]

        [RuleRequiredField(DefaultContexts.Save)]
        public string SqlQueryExpression
        {
            get => query;
            set => SetPropertyValue(nameof(SqlQueryExpression), ref query, value);
        }


    }
    public class ExportParametersObjectTypeConverter : LocalizedClassInfoTypeConverter
    {
        public override List<Type> GetSourceCollection(ITypeDescriptorContext context)
        {
            ITypeInfo parametersObjectBaseTypeInfo = XafTypesInfo.Instance.FindTypeInfo(typeof(ExportParametersObjectBase));

            return new List<Type>(parametersObjectBaseTypeInfo.Descendants.Select(XafTypesInfo.CastTypeInfoToType));
        }
    }

    [DomainComponent]
    public abstract class ExportParametersObjectBase
    {
        [ModelDefault("AllowEdit", "False")]

        public string Description { get;  set; }

        [Browsable(false)]
        public string Query { get; set; }
        [Browsable(false)]
        public string FileName { get; set; }
        public abstract string GetQuery();
    }


    [DomainComponent]

    public class ExportParameters  : ExportParametersObjectBase
    {

        public ExportParameters()
        {
            FromDate = DateTime.Now.Date.AddMonths(-12);
            ToDate = DateTime.Now.Date;
        }

        

        [RuleRequiredField]
        public DateTime FromDate { get; set; }
        [RuleRequiredField]
        public DateTime ToDate { get; set; }
      
 


        public override string GetQuery()
        {
            return Query.Replace("?FromDate", $"'{FromDate.ToString("yyyy-MM-dd")}'")
            .Replace("?ToDate", $"'{ToDate.ToString("yyyy-MM-dd")}'");  
        }


    }

    [DomainComponent]
    [XafDisplayName("Wybierz zakres dat 2")]
    public class ExportParameters2 : ExportParametersObjectBase
    {
        public ExportParameters2()
        {
            FromDate = DateTime.Now.Date.AddMonths(-12);
            ToDate = DateTime.Now.Date;

            FromDate2 = DateTime.Now.Date.AddMonths(-12);
            ToDate2 = DateTime.Now.Date;
        }

        [RuleRequiredField]
        public DateTime FromDate { get; set; }
        [RuleRequiredField]
        public DateTime ToDate { get; set; }

        [RuleRequiredField]
        public DateTime FromDate2 { get; set; }
        [RuleRequiredField]
        public DateTime ToDate2 { get; set; }

        public override string GetQuery()
        {
             return Query.Replace("?FromDate", $"'{FromDate.ToString("yyyy-MM-dd")}'")
             .Replace("?ToDate", $"'{ToDate.ToString("yyyy-MM-dd")}'")
             .Replace("?FromDate2", $"'{FromDate2.ToString("yyyy-MM-dd")}'")
             .Replace("?ToDate2", $"'{ToDate2.ToString("yyyy-MM-dd")}'");
           
        }
    }

}
