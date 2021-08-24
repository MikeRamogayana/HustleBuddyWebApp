using RestSharp;
using System.Web.Script.Serialization;

namespace IOSG_Web_App.Controller
{
    public class APIController
    {
        private static RestClient client = new RestClient("https://hustlebuddy.azurewebsites.net/api"); //("https://localhost:44305/api"); //

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

        public static T Deserialize<T>(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            T obj = serializer.Deserialize<T>(json);
            return obj;
        }

        public static string Serialize<T>(T obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(obj);
            return json;
        }
    }
}