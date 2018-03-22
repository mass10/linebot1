using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using System.Net.Http;

namespace WebApplication1
{
    /// <summary>
    /// _event の概要の説明です
    /// </summary>
    public class _event : IHttpHandler
    {
        private static void EventHandler(Newtonsoft.Json.Linq.JObject eventData, string request_signature, string signature)
        {
            Models.EventOperator op = new Models.EventOperator(eventData, request_signature, signature);
        }

        private static string CreateSignatureOfRequest(byte[] body)
        {
            try
            {
                //string documentContents = "";
                //using (Stream receiveStream = request.InputStream)
                //{
                //    using (StreamReader readStream = new StreamReader(receiveStream, request.ContentEncoding))
                //    {
                //        documentContents = readStream.ReadToEnd();
                //    }
                //}

                byte[] key = Encoding.UTF8.GetBytes(Models.Constants.CHANNEL_SECRET);
                using (System.Security.Cryptography.HMACSHA256 hmac = new System.Security.Cryptography.HMACSHA256(key))
                {
                    var hash = hmac.ComputeHash(body, 0, body.Length);
                    return Convert.ToBase64String(hash);
                }
            }
            catch
            {
                return "ERROR";
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var request_body = new StreamReader(request.InputStream, request.ContentEncoding).ReadToEnd();
            var unknown_data = Newtonsoft.Json.JsonConvert.DeserializeObject(request_body);
            Newtonsoft.Json.Linq.JObject unknown_event = (Newtonsoft.Json.Linq.JObject)unknown_data;
            var events = unknown_event["events"];

            string request_signature = "" + request.Headers["X-Line-Signature"];
            string signature = CreateSignatureOfRequest(request.ContentEncoding.GetBytes(request_body));

            if (request_signature != signature)
            {
                return;
            }

            foreach (Newtonsoft.Json.Linq.JObject e in events)
            {
                EventHandler(e, request_signature, signature);
                return;
            }
            context.Response.ContentType = "application/json; charset=UTF-8";
            context.Response.Write("{}");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}