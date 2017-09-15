using System;
using System.Configuration;
using Newtonsoft.Json;

namespace ApiGatewayDemo.Client
{
    class Demo
    {
        static void Main(string[] args)
        {
            string apiKey = ConfigurationManager.AppSettings["ApiKey"];
            string apiSecret = ConfigurationManager.AppSettings["ApiSecret"];
            string timestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");           
            var body = new { Id = 1 };
            string sign = ApiGateway.SHA1(string.Format("{0},{1},{2},{3}", apiKey, apiSecret, timestamp, JsonConvert.SerializeObject(body)));

            string url = ConfigurationManager.AppSettings["PartnerApiGatewayAddress"] + "/FxGetOrder";
            var openApiRequest = new
            {
                ApiKey = apiKey,
                TimeStamp = timestamp,
                RequestBody = body,
                Sign = sign
            };
            string data = JsonConvert.SerializeObject(openApiRequest);         
            Console.WriteLine(ApiGateway.Post(url, data));            

            Console.ReadLine();
        }     
    }
}
