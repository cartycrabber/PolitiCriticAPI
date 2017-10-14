using PoliticalDeterminer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace PoliticalDeterminer.Services
{
    public class RedditAPI
    {
        public RedditComment[] GetComments(string user)
        {
            HttpWebRequest request = WebRequest.CreateHttp($"https://www.reddit.com/user/{user}/comments/.json");
            request.Method = "GET";

            WebResponse response = request.GetResponse();

            Stream stream = response.GetResponseStream();

            RedditComment[] comments;

            using (StreamReader sr = new StreamReader(stream))
            {
                string json = sr.ReadToEnd();
                RedditListingChild<RedditComment>[] children = RedditListing<RedditComment>.FromJson(json).Data.Children;
                comments = new RedditComment[children.Length];
                for(int i = 0; i < children.Length; i++)
                {
                    comments[i] = children[i].Data;
                }
            }

            return comments;
        }
    }
}