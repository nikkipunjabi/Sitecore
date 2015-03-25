using Sitecore_App_Universal.Account;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Sitecore_App_Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        private async void SiteURLGo_Click(object sender, RoutedEventArgs e)
        {
            ProggressBarVisible(true);
            SP_SiteURL.Visibility = Visibility.Collapsed;
            SP_ProgressRing.Visibility = Visibility.Visible;
            if (txtSitecoreSiteURL != null && !string.IsNullOrWhiteSpace(txtSitecoreSiteURL.Text))
            {
                Authentication auth = new Authentication();
                var response = await auth.GetHttpResponse(txtSitecoreSiteURL.Text);
                var checkResponse = response;
            
               if (response.IsSuccessStatusCode)
               {
                    this.Frame.Navigate(typeof(MainPage));
               }
            }
            SP_ProgressRing.Visibility = Visibility.Collapsed;
            SP_SiteURL.Visibility = Visibility.Visible;
        }

        private void ProggressBarVisible(bool visible)
        {
            SP_ProgressRing.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            SP_SiteURL.Visibility = !visible ? Visibility.Visible:Visibility.Collapsed;
        }

    }
}
