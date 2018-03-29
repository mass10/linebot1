using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using System.Net.Http;
using CoreTweet;

namespace WebApplication1.Models
{
    public sealed class EventOperator
    {
        private string replyToken = "";
        private string request_signature = "";
        private string signature = "";

        public EventOperator(Newtonsoft.Json.Linq.JObject eventObject, string request_signature, string signature)
        {
            this.request_signature = request_signature;
            this.signature = signature;

            string type = "" + eventObject["type"];
            string replyToken = "" + eventObject["replyToken"];
            this.replyToken = replyToken;
            if (type == "message")
            {
                // メッセージを受信した
                this.handleMessageText(eventObject["message"]);
            }
            else if (type == "follow")
            {
                // フォロー再開した(友だち追加ではない)
            }
            else
            {

            }
        }

        private void replyTextMessage(string messageText)
        {
            //messageText = messageText + "\n\nrequest_signature=[" + request_signature + "], signature=[" + signature + "]";

            System.Collections.IList messages = new System.Collections.ArrayList();
            {
                var message_data = new System.Collections.SortedList();
                message_data["type"] = "text";
                message_data["text"] = messageText;
                messages.Add(message_data);
            }
            var content = new Dictionary<string, object>();
            content["replyToken"] = this.replyToken;
            content["messages"] = messages;

            // リプライ
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(content);
                var request_body = new StringContent(json, Encoding.UTF8, "application/json");
                var authorization = String.Format("Bearer {0}", Constants.CHANNEL_ACCESS_TOKEN);
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", authorization);
                client.PostAsync("https://api.line.me/v2/bot/message/reply", request_body);
            }
        }

        private void replyTextMessageWithImage(string messageText, string imageUrl)
        {
            System.Collections.IList messages = new System.Collections.ArrayList();

            // メッセージ文字列
            {
                var message_data = new System.Collections.SortedList();
                message_data["type"] = "text";
                message_data["text"] = messageText;
                messages.Add(message_data);
            }

            // 絵を付ける
            {
                var message_data = new System.Collections.SortedList();
                message_data["type"] = "image";
                message_data["originalContentUrl"] = imageUrl;
                message_data["previewImageUrl"] = imageUrl;
                messages.Add(message_data);
            }

            var content = new Dictionary<string, object>();
            content["replyToken"] = this.replyToken;
            content["messages"] = messages;

            // リプライ
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(content);
                var request_body = new StringContent(json, Encoding.UTF8, "application/json");
                var authorization = String.Format("Bearer {0}", Constants.CHANNEL_ACCESS_TOKEN);
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", authorization);
                client.PostAsync("https://api.line.me/v2/bot/message/reply", request_body);
            }
        }

        private void handleMessageText(Newtonsoft.Json.Linq.JToken messageObject)
        {
            string messageType = "" + messageObject["type"];
            if (messageType == "text")
            {
                string s = "" + messageObject["text"];
                if (s.Contains("文字列"))
                {
                    this.replyTextMessage("こんにちは");
                }
                else if (s.Contains("ばくらい"))
                {
                    this.replyTextMessageWithImage(
                        "はい", "https://s3-ap-northeast-1.amazonaws.com/osushijp/images/bakurai.jpg");
                }
                else if (s.Contains("おしんこ"))
                {
                    this.replyTextMessageWithImage(
                        "はい", "https://s3-ap-northeast-1.amazonaws.com/osushijp/images/oshinko.jpg");
                }
                else if (s.Contains("がり"))
                {
                    this.replyTextMessageWithImage(
                        "がりです", "https://s3-ap-northeast-1.amazonaws.com/osushijp/images/gari.jpg");
                }
                else if (s.Contains("URL"))
                {
                    this.replyTextMessage(
                        "URL を含む応答\n\nhttps://docs.google.com/presentation/d/xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/");
                }
                else if (s.Contains("たい"))
                {
                    this.replyTextMessageWithImage(
                        "たいです", "https://s3-ap-northeast-1.amazonaws.com/osushijp/images/tai.jpg");
                }
                else if (s.Contains("いか"))
                {
                    this.replyTextMessage(
                        "いかはいかセンターの方がおいしいよ...\n\nhttp://ikacenter.com/");
                }
                else
                {
                    this.replyTextMessage("" + s + "なんかないよ");
                }
                // 更新
                {
                    PostT(s);
                }
            }
            else
            {
                this.replyTextMessage("ちょっとやめてよ");
            }
        }

        private static void PostT(string text)
        {
            try
            {
                var tokens = CoreTweet.Tokens.Create(
                    "xxxxxxxxxxxxxxxxxxxxxxxxx",
                    "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
                    "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
                    "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                text = text + " #test";
                tokens.Statuses.Update(new { status = text });
            }
            catch
            {

            }
        }
    }
}