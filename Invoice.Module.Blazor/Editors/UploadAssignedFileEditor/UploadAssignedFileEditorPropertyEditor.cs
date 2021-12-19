using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using System;

namespace Invoice.Module.Blazor.Editors.UploadAssignedFileEditor
{
    [PropertyEditor(typeof(object), "UploadAssignedFileEditor", false)]
    public class UploadAssignedFileEditorPropertyEditor : BlazorPropertyEditorBase
    {
        public UploadAssignedFileEditorPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        protected override IComponentAdapter CreateComponentAdapter() => new UploadAssignedFIleEditorAdapter(new UploadAssignedFileEditorModel());
    }
}
