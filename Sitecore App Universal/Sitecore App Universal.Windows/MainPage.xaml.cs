using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Sitecore_App_Universal.Account;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Sitecore_App_Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        #region TreeItems
        private ObservableCollection<SitecoreContentTree> _treeItems;
        public ObservableCollection<SitecoreContentTree> TreeItems
        {
            get { return _treeItems; }
            set { _treeItems = value; }
        }

        #endregion

        public MainPage()
        {
            this.InitializeComponent();

            var jsonString = Authentication.SitecoreSiteInformation.Content.ReadAsStringAsync().Result;
            JsonObject root = Windows.Data.Json.JsonValue.Parse(jsonString).GetObject();

            //Get Json Result
            if (root.ContainsKey("result"))
            {
                JsonObject SearchItems = root.GetNamedObject("result");
                //Get All Item Arrays
                JsonArray mySearchItems = SearchItems.GetNamedArray("items");

                TreeItems = new Authentication().ParseSitecoreTree(mySearchItems);

                TreeViewControl.ItemsSource = TreeItems;
                SVContentTree.MaxHeight = Window.Current.Bounds.Height - 5;
            }
            //GetHttpResponse(); 
        }

        private void TreeViewControl_SelectedItemChanged(object sender, WinRTXamlToolkit.Controls.RoutedPropertyChangedEventArgs<object> e)
        {
            FieldNamePanel.Children.Clear();
            FieldValuePanel.Children.Clear();
            var SitecoreFields = e.NewValue as SitecoreContentTree;
            foreach (var item in SitecoreFields.ItemField)
            {
                TextBox fieldtitle = new TextBox(); fieldtitle.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
                fieldtitle.Height = 20;
                fieldtitle.IsEnabled = false;
                TextBox fieldvalue = new TextBox(); fieldvalue.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
                fieldtitle.Text = item.Key;
                fieldvalue.Text = item.Value;
                FieldNamePanel.Children.Add(fieldtitle);
                FieldValuePanel.Children.Add(fieldvalue);

                //TextBlock fieldtitle = new TextBlock(); fieldtitle.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
                //fieldtitle.FontSize = 20;
                //TextBox fieldvalue = new TextBox(); fieldvalue.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
                //fieldtitle.Text = item.Key;
                //fieldvalue.Text = item.Value;

                //Grid MyGrid = new Grid();

                //MyGrid.RowDefinitions.Add(new RowDefinition());
                //MyGrid.ColumnDefinitions.Add(new ColumnDefinition());
                //MyGrid.ColumnDefinitions.Add(new ColumnDefinition());

                //Grid.SetColumn(fieldtitle, 0);
                //Grid.SetRow(fieldtitle, 0);

                //Grid.SetColumn(fieldvalue, 1);
                //Grid.SetRow(fieldvalue, 0);

                //MyGrid.Children.Add(fieldtitle);
                //MyGrid.Children.Add(fieldvalue);

                //FieldsGrid.Children.Add(MyGrid);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (TreeViewControl.SelectedItem != null && TreeViewControl.SelectedValue != null)
            {
                SitecoreContentTree contentTreeSelectedItem = TreeViewControl.SelectedItem as SitecoreContentTree;
                foreach(var itemFields in FieldValuePanel.Children)
                {
                    
                }
            }
        }

    }
}
