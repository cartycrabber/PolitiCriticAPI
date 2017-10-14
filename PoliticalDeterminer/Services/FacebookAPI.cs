using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PoliticalDeterminer.Models;
using System.Net;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace PoliticalDeterminer.Services
{
    public class FacebookAPI
    {
        public FacebookPost[] GetPosts(string pageID)
        {
            HttpWebRequest request = WebRequest.CreateHttp($"https://graph.facebook.com/v2.10/{pageID}/posts?limit=100&" +
                $"access_token={Credentials.FacebookApiID}|{Credentials.FacebookSecret}");
            request.Method = "GET";

            WebResponse response = request.GetResponse();

            Stream stream = response.GetResponseStream();

            FacebookPost[] posts;

            using (StreamReader sr = new StreamReader(stream))
            {
                string json = sr.ReadToEnd();
                posts = FacebookPage.FromJson(json).Data;
            }

            return posts;
        }

        public string[] GetPostMessages(string pageID)
        {
            FacebookPost[] posts = GetPosts(pageID);

            string[] messages = new string[posts.Length];

            for(int i = 0; i < posts.Length; i++)
            {
                messages[i] = posts[i].Message;
            }

            return messages;
        }
    }
}