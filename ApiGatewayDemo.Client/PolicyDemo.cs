using System;
using System.Configuration;
using System.Diagnostics;
using Newtonsoft.Json;

namespace ApiGatewayDemo.Client
{
    public class PolicyDemo
    {
        static readonly string ApiKey = ConfigurationManager.AppSettings["ApiKey"];
        static readonly string ApiSecret = ConfigurationManager.AppSettings["ApiSecret"];

        static void Main1()
        {
            //超时演示：
            Timeout();
            //限流演示：
            //Limit();
            //熔断演示：
            //Fusing();
        }

        private static void Timeout()
        {
            var parameters = new           
            {
                Id = 1,
                CustomerName = "张三",               
                IsTakeAway = true,
                CreatedDate = DateTime.Now,
                StatusCode = 1,
                OrderItemList = new[]
                {
                    new
                    {
                        Product = new
                        {
                            Id = 100,
                            Name = "香港两日游",
                            StatusCode = 1
                        },
                        Quantity = 5
                    },
                    new
                    {
                        Product = new
                        {
                            Id = 101,
                            Name = "产品2",
                            StatusCode = 1
                        },
                        Quantity = 10
                    }
                }
            };
           
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Invoke("/FxCreateOrder", parameters);
            sw.Stop();
            Console.WriteLine("执行总时间（ms）：{0}", sw.ElapsedMilliseconds);

            Console.ReadLine();
        }

        private static void Limit()
        {
            Stopwatch stopwatch = new Stopwatch();
            for (int i = 0; i < 30; i++)
            //for (int i = 0; i < 19; i++)
            {
                Console.WriteLine("第{0}次调用：", i + 1);
                stopwatch.Restart();
                Invoke("/FxGetOrder", new { Id = 1 });
                stopwatch.Stop();
                Console.WriteLine("执行耗时：{0}ms.", stopwatch.ElapsedMilliseconds);
            }

            Console.ReadLine();
        }

        private static void Fusing()
        {
            //for (int i = 0; i < 35; i++)
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("第{0}次调用：", i + 1);
                Invoke("/FxGetOrder", new { Id = 0 });
            }

            Console.ReadLine();
        }

        private static void Invoke(string openApiName, object parameters)
        {
            string timestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
            var body = parameters;
            string sign = ApiGateway.SHA1(string.Format("{0},{1},{2},{3}", ApiKey, ApiSecret, timestamp, JsonConvert.SerializeObject(body)));

            var openApiRequest = new
            {
                ApiKey = ApiKey,
                TimeStamp = timestamp,
                RequestBody = body,
                Sign = sign
            };
            string data = JsonConvert.SerializeObject(openApiRequest);
            string url = ConfigurationManager.AppSettings["PartnerApiGatewayAddress"] + openApiName;

            var result = ApiGateway.Post(url, data);
            Console.WriteLine(result);
        }
    }
}
