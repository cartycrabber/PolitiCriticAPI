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
            //First, need to take the user's page URL and find their page ID

            int lastIndexOfSlash = url.LastIndexOf("facebook.com/");

            string pageName = url.Substring(lastIndexOfSlash + "facebook.com/".Length);
            int endSlashIndex = pageName.IndexOf("/");
            if(endSlashIndex > -1)
            {
                pageName = pageName.Substring(0, endSlashIndex);
            }

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


            //Now that we have the page ID we can request their posts

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

            //Go through and remove the posts that don't have text

            List<FacebookPost> temp = new List<FacebookPost>();

            foreach (FacebookPost p in posts)
            {
                if (p.Message != null && p.Message != "")
                    temp.Add(p);
            }

            return temp.ToArray();
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