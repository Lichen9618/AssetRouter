using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

namespace RPCQuery.RPCHelper
{
    public class SwapCheck
    {
        public static string SwapQuery(string queryJson, string url) 
        {
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", queryJson, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response.Content;
        }
    }
}
