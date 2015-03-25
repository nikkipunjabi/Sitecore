using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Sitecore_App_Universal.Account
{
    public class Authentication
    {
        public async Task<HttpResponseMessage> GetHttpResponse(string sitecoreSiteURL)
        {
            //HttpWebRequest request = new HttpWebRequest();
            //AuthenticationHeaderValue authHeaderUsername = new AuthenticationHeaderValue("X-Scitemwebapi-Username", @"sitecore\admin");
            //AuthenticationHeaderValue authHeaderPasssword = new AuthenticationHeaderValue("X-Scitemwebapi-Password", "b");

            //Don't remove below code
            //var client = new HttpClient();
            //client.DefaultRequestHeaders.Add("X-Scitemwebapi-Username", @"sitecore\admin");
            //client.DefaultRequestHeaders.Add("X-Scitemwebapi-Password", "b");
            //string requestURL = string.Format("http://{0}/-/item/v1/?scope=c&sc_database=master&query=/sitecore/content", sitecoreSiteURL);
            //var response = await client.GetAsync(requestURL);
            //SitecoreSiteInformation = response;
            //return response;


            //Thanks to Vasiliy: https://stackoverflow.com/questions/26347030/sitecore-web-api-user-authentication/26349404#26349404
            //Thanks to Vikram: http://stackoverflow.com/questions/25383502/sitecore-7-2-item-web-api-user-authentication
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("http://"+sitecoreSiteURL+"/-/item/v1/?scope=c&sc_database=master&query=/sitecore/content"),
                    Method = HttpMethod.Get
                };

                request.Headers.Add("X-Scitemwebapi-Username", @"sitecore\admin");
                request.Headers.Add("X-Scitemwebapi-Password", "b");

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    SiteURL = sitecoreSiteURL;
                    SitecoreSiteInformation = response;
                }
                return response;
            }
        }

        public static HttpResponseMessage SitecoreSiteInformation { get; set; }

        public static string SiteURL { get; set; }

        public ObservableCollection<SitecoreContentTree> ParseSitecoreTree(JsonArray SitecoreSite)
        {
            try
            {
                ObservableCollection<SitecoreContentTree> tree = new ObservableCollection<SitecoreContentTree>();

                foreach (var item in SitecoreSite)
                {
                    SitecoreContentTree myitem = new SitecoreContentTree();
                    var currentitem = item.GetObject();

                    myitem.SitecoreItem = currentitem.GetNamedValue("DisplayName").GetString();
                    myitem.SitecoreItemId = currentitem.GetNamedValue("ID").GetString();

                    if (currentitem.GetNamedValue("Fields").GetObject().Count > 0)
                    {
                        foreach (var fields in currentitem.GetNamedValue("Fields").GetObject())
                        {
                            var _fieldText = fields.Value.GetObject().GetNamedValue("Name").GetString();
                            var _fieldValue = fields.Value.GetObject().GetNamedValue("Value").GetString();

                            //Fill Fields
                            myitem._hasFields = true;

                            myitem.ItemField.Add(_fieldText, _fieldValue);

                        }
                    }


                    if (currentitem.GetNamedValue("HasChildren").GetBoolean())
                    {
                        myitem.HasChildrens = true;
                        var parentitemid = currentitem.GetNamedValue("ID").GetString();

                        using (var client = new HttpClient())
                        {
                            var request = new HttpRequestMessage
                            {
                                RequestUri = new Uri("http://" + SiteURL + "/-/item/v1/?scope=c&sc_database=master&sc_itemid=" + parentitemid),
                                Method = HttpMethod.Get
                            };

                            request.Headers.Add("X-Scitemwebapi-Username", @"sitecore\admin");
                            request.Headers.Add("X-Scitemwebapi-Password", "b");

                            var response = client.SendAsync(request).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                //var responseString = await response.Content.ReadAsStringAsync();

                                var jsonString = response.Content.ReadAsStringAsync();
                                JsonObject root = Windows.Data.Json.JsonValue.Parse(jsonString.Result).GetObject();
                                JsonObject SearchItems = root.GetNamedObject("result");
                                JsonArray mySearchItems = SearchItems.GetNamedArray("items");
                                myitem.Childrens = ParseSitecoreTree(mySearchItems);
                            }


                        }

                        ////var response = client.GetAsync("http://sitecoreforsearch/-/item/v1/?scope=c&sc_database=master&sc_itemid=" + parentitemid).Result;
                        //var jsonString = response.Content.ReadAsStringAsync();
                        //JsonObject root = Windows.Data.Json.JsonValue.Parse(jsonString.Result).GetObject();
                        //JsonObject SearchItems = root.GetNamedObject("result");
                        //JsonArray mySearchItems = SearchItems.GetNamedArray("items");
                        //myitem.Childrens = ParseSitecoreTree(mySearchItems);

                    }
                    else
                    {
                        myitem.HasChildrens = false;
                    }
                    tree.Add(myitem);
                }
                return tree;
            }
            catch (Exception ex)
            {
                //An item with the same key has already been added + ObservableCollection
                string errr = ex.Message + ex;
                return null;
            }
        }
    }
}
