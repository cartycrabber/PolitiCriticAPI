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
        const int MIN_POST_SIZE = 5;

        public FacebookPost[] GetPosts(string url)
        {
            url = url.Trim();
            url = url.TrimEnd('/');
            url = url.Trim();

            int lastIndexOfSlash = url.LastIndexOf("/");

            string pageName = url.Substring(lastIndexOfSlash + 1);

            HttpWebRequest findingPageID = WebRequest.CreateHttp($"https://graph.facebook.com/{pageName}/?" +
                $"access_token={Credentials.FacebookApiID}|{Credentials.FacebookSecret}");

            WebResponse pageIDResponse = findingPageID.GetResponse();

            Stream pageIDStream = pageIDResponse.GetResponseStream();

            string json;

            using (StreamReader sr = new StreamReader(pageIDStream))
            {
                json = sr.ReadToEnd();
            }

            int y = json.IndexOf("id");

            string pageID = json.Substring(y + 5);//add 5 to skip the id":" and get right to the number
            pageID = pageID.Substring(0, pageID.IndexOf('"'));

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
                string jsonData = sr.ReadToEnd();
                posts = FacebookPage.FromJson(jsonData).Data;
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