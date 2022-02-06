using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Invoice.Module.BusinessObjects.Tests
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
   // [DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class DemoClass : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public DemoClass(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}


        string positionTitle;
        public string PositionTitle
        {
            get { return positionTitle; }
            set { SetPropertyValue(nameof(PositionTitle), ref positionTitle, value); }
        }



        private Position _LookupPropertyForDisplay;
        [NonPersistent, XafDisplayName("Position")]
        public Position LookupPropertyForDisplay
        {
            get
            {
                if ((_LookupPropertyForDisplay == null && !string.IsNullOrEmpty(positionTitle)) ||
                (_LookupPropertyForDisplay != null && _LookupPropertyForDisplay.Title != PositionTitle))
                {
                    _LookupPropertyForDisplay =
                        Session.FindObject<Position>(new BinaryOperator("Title", positionTitle));
                }
                return _LookupPropertyForDisplay;
            }
            set
            {
                SetPropertyValue<Position>(nameof(LookupPropertyForDisplay),
                                           ref _LookupPropertyForDisplay, value);
                if (!IsLoading && !IsSaving)
                {
                    PositionTitle = value != null ? value.Title : string.Empty;
                }
            }
        }


    }

    [DefaultClassOptions, XafDefaultProperty(nameof(Title))]
    public class Position : BaseObject
    {
        public Position(Session session) : base(session) { }
        string _title;
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                SetPropertyValue(nameof(Title), ref _title, value);
            }
        }
    }

}