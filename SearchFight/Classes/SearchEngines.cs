using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace SearchFight.Classes
{
    public class SearchEngines
    {
        public static string LastErrorMessage = "";

        public SearchEngines()
        {

        }
        public static float SearchUsingGoogle(string sStringToSearch)
        {
            float fResult = -1;
            try
            {
                // SOURCE: https://developers.google.com/custom-search/json-api/v1/reference/cse/list
                var RESTClient = new RestClient("https://www.googleapis.com");

                IRestRequest RESTRequest = new RestRequest("customsearch/v1", Method.GET);
                RESTRequest.AddQueryParameter("q", "'" + sStringToSearch + "'");
                RESTRequest.AddQueryParameter("key", "AIzaSyDcxygs5EMusfGzEi1O9drH8qKz1gqUggw");
                RESTRequest.AddQueryParameter("cx", "005430748436740110835:6b0qwxqidpk");

                IRestResponse response = RESTClient.Execute(RESTRequest);
                var content = response.Content;

                if (content.IndexOf("\"error\":", StringComparison.CurrentCultureIgnoreCase) > 0)
                {
                    LastErrorMessage = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(content)["error"]["message"];
                }
                else
                {
                    fResult = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(content)["searchInformation"]["totalResults"];
                }                    
            }
            catch (Exception ex)
            {
                LastErrorMessage = ex.Message;
            }
            return fResult;
        }
        public static float SearchUsingBing(string sStringToSearch)
        {
            float fResult = -1;
            try
            {
                // Construct the URI of the search request
                string uriBase = "https://api.cognitive.microsoft.com/bing/v7.0/search";
                string accessKey = "41cc03407704454c9b30fc5d1db0a888";

                var uriQuery = uriBase + "?q=" + Uri.EscapeDataString(sStringToSearch);

                // Perform the Web request and get the response
                WebRequest request = HttpWebRequest.Create(uriQuery);
                request.Headers["Ocp-Apim-Subscription-Key"] = accessKey;
                HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
                string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

                // Create result object for return
                fResult = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json)["webPages"]["totalEstimatedMatches"];
            }
            catch (Exception ex)
            {
                LastErrorMessage = ex.Message;
            }
            return fResult;
        }
        public static float SearchUsingYandex(string sStringToSearch)
        {
            // SOURCE: https://xml.yandex.com/test/

            float fResult = -1;
            try
            {
                // Construct the URI of the search request
                string uriBase = "https://yandex.com/search/xml?l10n=en&user=mleon33&key=03.569928522:026d93fb772282a6855ad4596370b615";
                var uriQuery = uriBase + "&query=" + Uri.EscapeDataString(sStringToSearch);
                bool bAllIsOK = false;

                // Perform the Web request and get the response
                WebRequest request = HttpWebRequest.Create(uriQuery);
                HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;

                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                XmlReader xmlReader = XmlReader.Create(dataStream);
                XDocument xDoc = XDocument.Load(xmlReader);

                if (xDoc.Root != null)
                {
                    var e = (from element in xDoc.Elements("yandexsearch").Elements("response").Elements("results").Elements("grouping").Elements("found")
                                select element);

                    foreach (XElement element in e)
                    {
                        if (element.Attribute("priority").Value == "all")
                        {
                            fResult = Convert.ToInt64(element.Value);
                            bAllIsOK = true;
                        }
                    }

                    if (!bAllIsOK)
                    {
                        var f = (from element in xDoc.Elements("yandexsearch").Elements("response").Elements("error")
                                 select element);

                        foreach(XElement element in f)
                        {
                            LastErrorMessage = element.Value + "\n";
                        }
                        
                    }
                    return fResult;
                }
            }
            catch (Exception ex)
            {
                LastErrorMessage = ex.Message;
            }

            return fResult;
        }
    }
}
