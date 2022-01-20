using System;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.Persistent.BaseImpl;

namespace Invoice.Module.Blazor.Editors.UploadAssignedFileEditor
{
    public class UploadAssignedFileEditorModel : ComponentModelBase
    {
        public object Value
        {
            get => GetPropertyValue<object>();
            set => SetPropertyValue(value);
        }

        public bool ReadOnly
        {
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }

        public void SetValueFromUI(object value)
        {
            SetPropertyValue(value, notify: false, nameof(Value));
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler ValueChanged;
    }
}