using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HustleBuddy_Web.Data
{
    public class DatabaseContext
    {
        private static RestClient client = new RestClient("https://hustlebuddy.azurewebsites.net/api/"); //("https://localhost:44305/api"); //

        public static IRestResponse GetRequest(string url)
        {
            var request = new RestRequest(url, Method.GET);
            request.OnBeforeDeserialization = response =>
            {
                response.ContentType = "application/json";
            };

            return client.Execute(request);
        }

        public static IRestResponse PostRequest<T>(string url, T obj)
        {
            var request = new RestRequest(url, Method.POST);
            request.AddJsonBody(obj);
            request.OnBeforeDeserialization = response =>
            {
                response.ContentType = "text/plain";
            };
            return client.Execute(request);
        }

        public static IRestResponse PutRequest<T>(string url, T obj)
        {
            var request = new RestRequest(url, Method.PUT);
            request.AddJsonBody(obj);
            request.OnBeforeDeserialization = response =>
            {
                response.ContentType = "text/plain";
            };
            return client.Execute(request);
        }

        public static IRestResponse DeleteRequest(string url)
        {
            var request = new RestRequest(url, Method.DELETE);
            request.OnBeforeDeserialization = response =>
            {
                response.ContentType = "application/json";
            };

            return client.Execute(request);
        }
    }
}
