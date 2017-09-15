using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ApiGatewayDemo.Client
{
    public class ApiGateway
    {
        public static string Post(string url, string data)
        {
            HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json; charset=utf-8";
            webRequest.Accept = "application/json; charset=utf-8";

            var requestByte = Encoding.UTF8.GetBytes(data);
            using (var requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(requestByte, 0, requestByte.Length);
            }
            using (var webResponse = webRequest.GetResponse())
            {
                var responseStream = webResponse.GetResponseStream();
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    var responseText = reader.ReadToEnd();
                    return responseText;
                }
            }
        }
        public static string SHA1(string txt)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytesSha1In = Encoding.UTF8.GetBytes(txt);
            byte[] bytesSha1Out = sha1.ComputeHash(bytesSha1In);
            string sha1Out = string.Join("", bytesSha1Out.Select(i => string.Format("{0:x2}", i)));
            return sha1Out;
        }
    }
}
