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

        private static string ConvertMessageText(string s, int count)
        {
            return "" + s + "\n\nこれを頼んだひとは" + count + "人目だよ";
        }

        private void handleMessageText(Newtonsoft.Json.Linq.JToken messageObject)
        {
            string messageType = "" + messageObject["type"];
            if (messageType == "text")
            {
                string s = "" + messageObject["text"];
                if (s.Contains("かずのこ"))
                {
                    int current = increment_sakana("かずのこ");
                    string messageText = ConvertMessageText("うちのかずのこはのりで固めたやつだよ... おおきな声で言わないでね...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("こもちこんぶ"))
                {
                    int current = increment_sakana("こもちこんぶ");
                    string messageText = ConvertMessageText("なんでかわかんないけどちょっと高いよね...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("ほたて"))
                {
                    int current = increment_sakana("ほたて");
                    string messageText = ConvertMessageText("忘れられがちだよね...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("かき"))
                {
                    int current = increment_sakana("かき");
                    string messageText = ConvertMessageText("何個？", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("いかのしおから"))
                {
                    int current = increment_sakana("いかのしおから");
                    string messageText = ConvertMessageText("おや、もう少し酒が必要かい？", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("しおから"))
                {
                    int current = increment_sakana("しおから");
                    string messageText = ConvertMessageText("おや、もう少し酒が必要かい？", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("ばくらい"))
                {
                    int current = increment_sakana("ばくらい");
                    string messageText = ConvertMessageText("おや、趣味があうじゃないかおまえさん", current);
                    this.replyTextMessageWithImage(messageText, "https://s3-ap-northeast-1.amazonaws.com/osushijp/images/bakurai.jpg");
                }
                else if (s.Contains("おしんこ"))
                {
                    int current = increment_sakana("おしんこ");
                    string messageText = ConvertMessageText("はい", current);
                    this.replyTextMessageWithImage(messageText, "https://s3-ap-northeast-1.amazonaws.com/osushijp/images/oshinko.jpg");
                }
                else if (s.Contains("なまこ"))
                {
                    int current = increment_sakana("なまこ");
                    string messageText = ConvertMessageText("ポン酢でいいかい？", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("たいらがい"))
                {
                    int current = increment_sakana("たいらがい");
                    string messageText = ConvertMessageText("たいらがいには毛が生えているよ...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("あかがい"))
                {
                    int current = increment_sakana("あかがい");
                    string messageText = ConvertMessageText("くにゃくにゃしてるよね...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("はまぐり"))
                {
                    int current = increment_sakana("はまぐり");
                    string messageText = ConvertMessageText("寿司屋にはあまり置いてないよね...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("ほっきがい"))
                {
                    int current = increment_sakana("ほっきがい");
                    string messageText = ConvertMessageText("ほっきも今は無いかな...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("いくら"))
                {
                    int current = increment_sakana("いくら");
                    string messageText = ConvertMessageText("あんまり食べ過ぎちゃだめよ...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("すじこ"))
                {
                    int current = increment_sakana("すじこ");
                    string messageText = ConvertMessageText("東京のすじこは小さいね...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("たまご"))
                {
                    int current = increment_sakana("たまご");
                    string messageText = ConvertMessageText("2カンでいいかね...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("はまち"))
                {
                    int current = increment_sakana("はまち");
                    string messageText = ConvertMessageText("今はぶりがおいしいね", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("がり"))
                {
                    int current = increment_sakana("がり");
                    string messageText = ConvertMessageText("あんたさっきからがりで飲んでんじゃないよ...", current);
                    this.replyTextMessageWithImage(messageText, "https://s3-ap-northeast-1.amazonaws.com/osushijp/images/gari.jpg");
                }
                else if (s.Contains("ぶり"))
                {
                    int current = increment_sakana("ぶり");
                    string messageText = ConvertMessageText("ちょうどおいしいのあるよ", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("すずき"))
                {
                    int current = increment_sakana("すずき");
                    string messageText = ConvertMessageText("おっと、ちょうどいいの入ったとこだよ...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("ごちそうさま"))
                {
                    int current = increment_sakana("ごちそうさま");
                    string messageText = ConvertMessageText("はいまいど～！スライドのURLだよ \n\nhttps://docs.google.com/presentation/d/17LnDIV9xhyn3Iag9BL6GzWi8H8kwYMphDhX7YijHtTI/", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("かんじょう"))
                {
                    int current = increment_sakana("かんじょう");
                    string messageText = ConvertMessageText("はいまいど～！スライドのURLだよ \n\nhttps://docs.google.com/presentation/d/17LnDIV9xhyn3Iag9BL6GzWi8H8kwYMphDhX7YijHtTI/", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("おかんじょう"))
                {
                    int current = increment_sakana("おかんじょう");
                    string messageText = ConvertMessageText("はいまいど～！スライドのURLだよ \n\nhttps://docs.google.com/presentation/d/17LnDIV9xhyn3Iag9BL6GzWi8H8kwYMphDhX7YijHtTI/", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("きんめだい"))
                {
                    int current = increment_sakana("きんめだい");
                    string messageText = ConvertMessageText("あんまり時期じゃないねえ...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("かつお"))
                {
                    int current = increment_sakana("かつお");
                    string messageText = ConvertMessageText("いまあんまりおいしくないよ...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("ほや"))
                {
                    int current = increment_sakana("ほや");
                    string messageText = ConvertMessageText("肝と和えるとおいしいよ", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("くろだい"))
                {
                    int current = increment_sakana("くろだい");
                    string messageText = ConvertMessageText("いまおいしいけど東京の寿司屋じゃあんまり出さないね...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("いしだい"))
                {
                    int current = increment_sakana("いしだい");
                    string messageText = ConvertMessageText("いまおいしいけど東京の寿司屋じゃあんまり出さないね...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("たい"))
                {
                    int current = increment_sakana("たい");
                    string messageText = ConvertMessageText("いいね！いまおいしいよ～", current);
                    this.replyTextMessageWithImage(messageText, "https://s3-ap-northeast-1.amazonaws.com/osushijp/images/tai.jpg");
                }
                else if (s.Contains("まだい"))
                {
                    int current = increment_sakana("まだい");
                    string messageText = ConvertMessageText("まだいってたいだよね", current);
                    this.replyTextMessageWithImage(messageText, "https://s3-ap-northeast-1.amazonaws.com/osushijp/images/tai.jpg");
                }
                else if (s.Contains("かんぱち"))
                {
                    int current = increment_sakana("かんぱち");
                    string messageText = ConvertMessageText("おお、いまおいしいね！", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("さより"))
                {
                    int current = increment_sakana("さより");
                    string messageText = ConvertMessageText("あんまり好きっていうひといなくない？", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("ひらめ"))
                {
                    int current = increment_sakana("ひらめ");
                    string messageText = ConvertMessageText("ひらめも秋からだね", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("いわし"))
                {
                    int current = increment_sakana("いわし");
                    string messageText = ConvertMessageText("今年はもうおわりだよ... いわしは初夏から夏にかけておいしくなるね", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("あじ"))
                {
                    int current = increment_sakana("あじ");
                    string messageText = ConvertMessageText("今年はそろそろおわりかな...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("たこ"))
                {
                    int current = increment_sakana("たこ");
                    string messageText = ConvertMessageText("生たこは塩もみしないと雑菌がいっぱいいるんだよ... けっこう疲れるんだよ...", current);
                    this.replyTextMessageWithImage(messageText, "https://s3-ap-northeast-1.amazonaws.com/osushijp/images/namatako.jpg");
                }
                else if (s.Contains("ひとで"))
                {
                    int current = increment_sakana("ひとで");
                    string messageText = ConvertMessageText("自分でとってきて家でやりなよ...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("うに"))
                {
                    int current = increment_sakana("うに");
                    string messageText = ConvertMessageText("うには一年中あるけど毎日のように食べちゃだめだよ...", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("いか"))
                {
                    int current = increment_sakana("いか");
                    string messageText = ConvertMessageText("いかはいかセンターの方がおいしいよ...\n\nhttp://ikacenter.com/", current);
                    this.replyTextMessage(messageText);
                }
                else if (s.Contains("かれい"))
                {
                    int current = increment_sakana("かれい");
                    string messageText = ConvertMessageText("かれいはちょっと無いよね... ひらめがおいしいよ", current);
                    this.replyTextMessage(messageText);
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

        private static readonly IDictionary<string, int> _counter = new Dictionary<string, int>();

        private static int increment_sakana(string key)
        {
            lock (_counter)
            {
                int current = 0;
                if (_counter.ContainsKey(key))
                {
                    current = _counter[key];
                }
                current++;
                _counter[key] = current;
                return current;
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
                text = text + " #test #おすしのはちべえ";
                tokens.Statuses.Update(new { status = text });
            }
            catch
            {

            }
        }
    }
}