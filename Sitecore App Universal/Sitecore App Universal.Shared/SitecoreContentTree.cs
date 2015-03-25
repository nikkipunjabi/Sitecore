using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Sitecore_App_Universal
{
    public class SitecoreContentTree
    {
        private string _sitecoreItemID;

        private string _sitecoreItem;

        private bool _hasChildren = false;

        public string SitecoreItem { get { return _sitecoreItem; } set { _sitecoreItem = value; } }

        public bool HasChildrens { get { return _hasChildren; } set { _hasChildren = value; } }

        public string SitecoreItemId { get { return _sitecoreItemID; } set { _sitecoreItemID = value; } }

        #region Children
        private ObservableCollection<SitecoreContentTree> _childrens = new ObservableCollection<SitecoreContentTree>();
        /// <summary>
        /// Gets or sets the child items.
        /// </summary>
        public ObservableCollection<SitecoreContentTree> Childrens
        {
            get { return _childrens; }
            set { _childrens = value; }
        }

        #endregion

        #region Fields
        public bool _hasFields = false;

        private Dictionary<string, string> _itemField = new Dictionary<string, string>();
        public Dictionary<string, string> ItemField { get { return _itemField; } set { _itemField = value; } }

        #endregion

    }
}
