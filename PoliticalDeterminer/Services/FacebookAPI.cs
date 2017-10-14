using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PoliticalDeterminer.Models;
using System.Net;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;

namespace PoliticalDeterminer.Services
{
    public class FacebookAPI
    {
        public async System.Threading.Tasks.Task<FacebookPost[]> GetPostsAsync(string url)
        {
            int lastIndexOfSlash = url.LastIndexOf("/");

            string pageName = url.Substring(lastIndexOfSlash, url.Length);

            HttpWebRequest findingPageID = WebRequest.CreateHttp($"https://graph.facebook.com/{pageName}" +
                $"access_token={Credentials.FacebookApiID}|{Credentials.FacebookSecret}");

            WebResponse pageIDResponse = findingPageID.GetResponse();

            Stream pageIDStream = pageIDResponse.GetResponseStream();

            string json;

            using (StreamReader sr = new StreamReader(pageIDStream))
            {
                json = sr.ReadToEnd();
            }

            int y = json.IndexOf("id");

            FacebookPost[] fbID = FacebookPage.FromJson(json).Data;

            string pageID = fbID[0].Id;

            HttpWebRequest request = WebRequest.CreateHttp($"https://graph.facebook.com/v2.10/{pageID}/posts?limit=100&" +
                $"access_token={Credentials.FacebookApiID}|{Credentials.FacebookSecret}");
            request.Method = "GET";

            WebResponse response = request.GetResponse();

            Stream stream = response.GetResponseStream();

            FacebookPost[] posts;

            using (StreamReader sr = new StreamReader(stream))
            {
                string jsonData = sr.ReadToEnd();
                posts = FacebookPage.FromJson(jsonData).Data;
            }

            return posts;
        }

        public string[] GetPostMessages(string pageID)
        {
            FacebookPost[] posts = GetPostsAsync(pageID);

            string[] messages = new string[posts.Length];

            for(int i = 0; i < posts.Length; i++)
            {
                messages[i] = posts[i].Message;
            }

            return messages;
        }
    }
}