using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.Blazor.Editors
{
    [PropertyEditor(typeof(DateTime), false)]
    public class CustomDateTimePropertyEditor : DateTimePropertyEditor
    {
        public CustomDateTimePropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        protected override void OnControlCreated()
        {
            base.OnControlCreated();
            if (Control is DxDateEditAdapter adapter)
            {
                adapter.ComponentModel.TimeSectionVisible = true;
            }
        }
    }
}
