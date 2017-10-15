using System.Collections.Generic;
using System.Linq;
using PoliticalDeterminer.Models;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace PoliticalDeterminer.Services
{
    public class FacebookAPI
    {
        //Minimum number of words allowed in a post. Posts with less words are discarded
        const int MIN_POST_SIZE = 5;

        /// <summary>
        /// Gets the last 100 Facebook posts on a specified Facebook page
        /// </summary>
        /// <param name="url">The Url of the Facebook page to get</param>
        /// <returns>An array of Facebook posts</returns>
        public FacebookPost[] GetPosts(string url)
        {
            //First, need to take the user's page URL and find their page ID
            //This is because looking up a user's posts MUST be done with a pageID, not a username

            string pageName = string.Empty;
            //Trim off everything infront of the page name, if there is any
            int lastIndexOfSlash = url.LastIndexOf("facebook.com/");
            if(lastIndexOfSlash > -1)
                pageName = url.Substring(lastIndexOfSlash + "facebook.com/".Length);
            //Trim off everything after the page name, if there is anything
            int endSlashIndex = pageName.IndexOf("/");
            if(endSlashIndex > -1)
                pageName = pageName.Substring(0, endSlashIndex);

            //Create an HTTP GET Request to get the pageID corresponding to the pageName, using the page name and developer credentials
            HttpWebRequest findingPageID = WebRequest.CreateHttp($"https://graph.facebook.com/{pageName}/?" +
                $"access_token={Credentials.FacebookApiID}|{Credentials.FacebookSecret}");

            //Get the response, should be a json object containing a username and id field
            WebResponse pageIDResponse = findingPageID.GetResponse();

            //Stream the json into a string
            Stream pageIDStream = pageIDResponse.GetResponseStream();
            string json;
            using (StreamReader sr = new StreamReader(pageIDStream))
            {
                json = sr.ReadToEnd();
            }

            //the id attribute is the one we want
            int y = json.IndexOf("id");

            string pageID = json.Substring(y + 5);//add 5 to skip the id":" and get right to the number
            pageID = pageID.Substring(0, pageID.IndexOf('"'));//cut off everything after the number (we know the number will end with a ")


            //Now that we have the page ID we can request their posts
            //Create a new GET request with our new pageID, limiting our search to 100 results (the max allowed by the FB API)
            HttpWebRequest request = WebRequest.CreateHttp($"https://graph.facebook.com/v2.10/{pageID}/posts?limit=100&" +
                $"access_token={Credentials.FacebookApiID}|{Credentials.FacebookSecret}");
            request.Method = "GET";

            //Get the response Json, catching any weird exceptions
            WebResponse response;
            try
            {
                response = request.GetResponse();
            } catch (WebException e)
            {
                Debug.WriteLine($"Exception for request {request.RequestUri}: {e.Message}");
                return new FacebookPost[] { };
            }

            //Stream the Json object into a string, and then deserialize it into a FacebookPage object, from which the FacebookPost array can be extracted
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

        /// <summary>
        /// Gets the last 100 Facebook posts on a specified Facebook page, then converts them to their raw text
        /// </summary>
        /// <param name="url">The Url of the Facebook page to get</param>
        /// <returns>An array of strings representing the text of the Facebook posts</returns>
        public string[] GetPostMessages(string url)
        {
            //Get the posts like normal, and then add all their Messages to a string array
            FacebookPost[] posts = GetPosts(url);

            //Use a list because we don't know how many string we'll get
            List<string> messages = new List<string>();

            //Check post size and replace new line characters with spaces so everything is on one line
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