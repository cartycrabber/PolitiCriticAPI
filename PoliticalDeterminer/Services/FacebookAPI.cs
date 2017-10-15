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
        const int MIN_POST_SIZE = 5;

        public FacebookPost[] GetPosts(string pageID)
        {
            HttpWebRequest request = WebRequest.CreateHttp($"https://graph.facebook.com/v2.10/{pageID}/posts?limit=100&" +
                $"access_token={Credentials.FacebookApiID}|{Credentials.FacebookSecret}");
            request.Method = "GET";

            WebResponse response;

            try
            {
                response = request.GetResponse();
            } catch (WebException e)
            {
                Debug.WriteLine($"Exception for request {request.RequestUri}: {e.Message}");
                return new FacebookPost[] { };
            }

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

            List<string> messages = new List<string>();

            for(int i = 0; i < posts.Length; i++)
            {
                if(posts[i].Message != null)
                {
                    if(posts[i].Message.Split(' ').Count() >= MIN_POST_SIZE)
                        messages.Add(posts[i].Message.Replace("\n", " "));
                }

            }

            return messages.ToArray();
        }
    }
}