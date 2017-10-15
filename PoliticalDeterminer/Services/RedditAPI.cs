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
        const int MIN_COMMENT_SIZE = 5;

        public RedditComment[] GetComments(string user)
        {
            HttpWebRequest request = WebRequest.CreateHttp($"https://www.reddit.com/user/{user}/comments/.json?limit=100");
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

        public string[] GetCommentsText(string user)
        {
            RedditComment[] comments = GetComments(user);
            List<string> text = new List<string>();

            for(int i = 0; i < comments.Length; i++)
            {
                if (comments[i].Body != null)
                {
                    if(comments[i].Body.Split(' ').Count() >= MIN_COMMENT_SIZE)
                        text.Add(comments[i].Body.Replace("\n", " "));
                }
            }

            return text.ToArray();
        }
    }
}