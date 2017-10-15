using PoliticalDeterminer.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace PoliticalDeterminer.Services
{
    public class RedditAPI
    {
        //Minimum number of words allowed in a post. Posts with less words are discarded
        const int MIN_COMMENT_SIZE = 5;

        /// <summary>
        /// Gets the last 100 Reddit comments for a specified username
        /// </summary>
        /// <param name="user">Reddit Username to lookup</param>
        /// <returns>Array of Reddit Comments</returns>
        public RedditComment[] GetComments(string user)
        {
            //Create our GET request with the specified user, limiting to 100 comments (max allowed by Reddit API)
            HttpWebRequest request = WebRequest.CreateHttp($"https://www.reddit.com/user/{user}/comments/.json?limit=100");
            request.Method = "GET";

            //Get response Json object
            WebResponse response = request.GetResponse();

            //Stream Json to string then deserialize into a RedditListing of type RedditComment (Basically a listing of comments)
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

        /// <summary>
        /// Gets the last 100 Reddit comments for a specified username, then converts them to their raw text
        /// </summary>
        /// <param name="user">Reddit Username to lookup</param>
        /// <returns>An array of strings representing the text of the Reddit comments</returns>
        public string[] GetCommentsText(string user)
        {
            //Get the comments like normal, and then add all their Messages to a string array
            RedditComment[] comments = GetComments(user);

            List<string> text = new List<string>();

            //Check comment size and replace new line characters with spaces so everything is on one line
            for (int i = 0; i < comments.Length; i++)
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