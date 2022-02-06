using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Utils;
using DevExpress.Utils.Serializing;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Invoice.Module.Win.Controllers
{
    public class GridViewController : ViewController<ListView>, IModelExtender
    {
        GridListEditor gridListEditor = null;
        GridFormatRulesStore formatRulesStore = null;
        protected override void OnActivated()
        {
            base.OnActivated();
            gridListEditor = View.Editor as GridListEditor;
            if (gridListEditor != null)
            {

                View.ModelSaved += View_ModelSaved;
            }
        }


        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

            gridListEditor.GridView.OptionsMenu.ShowConditionalFormattingItem = true;

            GridView gridView = gridListEditor.GridView;

            SetListView(gridView);

            InitializeFormatRules();

        }

        private static void SetListView(GridView gridView)
        {
            gridView.OptionsView.EnableAppearanceOddRow = true;
            //  checkbox do zaznaczania rekordów
            //  gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            //  gridView.Appearance.OddRow.BackColor = Color.FromArgb(244, 244, 244);
            gridView.OptionsView.ShowFooter = true;
            gridView.OptionsView.GroupFooterShowMode = GroupFooterShowMode.VisibleIfExpanded;
            gridView.OptionsMenu.ShowGroupSummaryEditorItem = true;
            gridView.OptionsMenu.ShowConditionalFormattingItem = true;
            gridView.OptionsPrint.ExpandAllGroups = false;
            //  właczamy filtry pod nagłowkami
            gridView.OptionsView.ShowAutoFilterRow = false;
            //  właczamy scroll - ustaw false 
            gridView.OptionsView.ColumnAutoWidth = false;
            //  właczamy zmiane rozmiru kolumn
            gridView.OptionsView.RowAutoHeight = true;

            gridView.OptionsView.ColumnHeaderAutoHeight = DefaultBoolean.True;


            gridView.OptionsFind.AlwaysVisible = true;
            gridView.OptionsFind.FindMode = FindMode.Always;
            gridView.OptionsFind.ClearFindOnClose = true;
            gridView.OptionsFind.FindDelay = 500;
            gridView.UserCellPadding = new System.Windows.Forms.Padding(0);
            gridView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Default;
        }

        void View_ModelSaved(object sender, EventArgs e)
        {
            ((IModelListViewGridFormatRuleSettings)View.Model).GridFormattingSettings = formatRulesStore.Save();
        }

        protected override void OnDeactivated()
        {
            if (gridListEditor != null)
            {
                View.ModelSaved -= View_ModelSaved;

                gridListEditor = null;
            }
            base.OnDeactivated();
        }



        public interface IModelListViewGridFormatRuleSettings
        {
            [Browsable(false)]
            string GridFormattingSettings { get; set; }
        }


        private void InitializeFormatRules()
        {
            gridListEditor.GridView.OptionsMenu.ShowConditionalFormattingItem = true;
            formatRulesStore = new GridFormatRulesStore();
            formatRulesStore.FormatRules = gridListEditor.GridView.FormatRules;
            formatRulesStore.Restore(((IModelListViewGridFormatRuleSettings)View.Model).GridFormattingSettings);
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            extenders.Add<IModelListView, IModelListViewGridFormatRuleSettings>();
        }
    }


    //Dennis: This is a modified version of the solution given at
    //https://www.devexpress.com/Support/Center/Question/Details/T289562
    //that can save/load settings into/from a string.
    internal class GridFormatRulesStore : IXtraSerializable
    {
        GridFormatRuleCollection formatRules;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(true),
         XtraSerializableProperty(XtraSerializationVisibility.Collection, true, false, true, 1000, XtraSerializationFlags.DefaultValue)]
        public virtual GridFormatRuleCollection FormatRules
        {
            get { return formatRules; }
            set { formatRules = value; }
        }
        public string Save()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                SaveCore(new XmlXtraSerializer(), stream);
                return Convert.ToBase64String(stream.GetBuffer());
            }
        }
        public void Restore(string settings)
        {
            if (!string.IsNullOrEmpty(settings))
            {
                using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(settings)))
                {
                    RestoreCore(new XmlXtraSerializer(), stream);
                }
            }
        }
        void SaveCore(XtraSerializer serializer, object path)
        {
            Stream stream = path as Stream;
            if (stream != null)
                serializer.SerializeObject(this, stream, GetType().Name);
            else
                serializer.SerializeObject(this, path.ToString(), GetType().Name);
        }
        void RestoreCore(XtraSerializer serializer, object path)
        {
            Stream stream = path as Stream;
            if (stream != null)
                serializer.DeserializeObject(this, stream, GetType().Name);
            else
                serializer.DeserializeObject(this, path.ToString(), GetType().Name);
        }
        void XtraClearFormatRules(XtraItemEventArgs e) { FormatRules.Clear(); }
        object XtraCreateFormatRulesItem(XtraItemEventArgs e)
        {
            var rule = new GridFormatRule();
            formatRules.Add(rule);
            return rule;
        }
        #region IXtraSerializable
        public void OnEndDeserializing(string restoredVersion) { }
        public void OnEndSerializing() { }
        public void OnStartDeserializing(LayoutAllowEventArgs e) { }
        public void OnStartSerializing() { }
        #endregion
    }
}


