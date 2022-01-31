using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace Invoice.Module.BusinessObjects
{
    [DefaultProperty(nameof(TreeListBaseObject.Caption))]
    [DefaultClassOptions]
    public class TreeListBaseObject : BaseObject, ITreeNode
    {
        private TreeListBaseObject parentObject;
        private string caption;
        public TreeListBaseObject(Session session) : base(session)
        {
        }

        public string Caption
        {
            get { return caption; }
            set { SetPropertyValue<string>(nameof(Caption), ref caption, value); }
        }

        [Browsable(false)]
        [Association("TreeListBaseObject-TreeListBaseObject")]
        public TreeListBaseObject ParentObject
        {
            get { return parentObject; }
            set { SetPropertyValue<TreeListBaseObject>(nameof(ParentObject), ref parentObject, value); }
        }

        [Association("TreeListBaseObject-TreeListBaseObject"), Aggregated]
        public XPCollection<TreeListBaseObject> NestedObjects
        {
            get { return GetCollection<TreeListBaseObject>(nameof(NestedObjects)); }
        }

        #region ITreeNode Members
        IBindingList ITreeNode.Children { get { return NestedObjects; } }

        string ITreeNode.Name { get { return Caption; } }

        ITreeNode ITreeNode.Parent { get { return ParentObject; } }
		#endregion

        [Action(Caption= "Create test data", ImageName ="BOSkull", AutoCommit =true)]
        public void InitializeObject()
        {
            int index = 0;
            this.Caption = $"Test tree list object #{index}";
            CreateChildren(this, 5);
        }

        private void CreateChildren(TreeListBaseObject treeListEditorObject, int count)
        {
            TreeListBaseObject parentLevelObject = treeListEditorObject;
            int currentLevel = 0;
            while(parentLevelObject.ParentObject != null)
            {
                parentLevelObject = parentLevelObject.ParentObject;
                currentLevel++;
            }
            if(currentLevel >= 5)
            {
                return;
            }
            for(int i = 0; i < count; i++)
            {
                TreeListBaseObject childObject = new TreeListBaseObject(Session);
                childObject.ParentObject = treeListEditorObject;
                childObject.Caption = $"{treeListEditorObject.Caption}-{i}";
                childObject.Save();
                CreateChildren(childObject, count + 1);
            }
        }
    }
}
