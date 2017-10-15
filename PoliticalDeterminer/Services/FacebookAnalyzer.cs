using PoliticalDeterminer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoliticalDeterminer.Services
{
    public static class FacebookAnalyzer
    {
        private static TextAnalyzer textAnalyzer = TextAnalyzer.GetInstance();
        private static FacebookAPI api = new FacebookAPI();

        public static float Analyze(string username)
        {
            FacebookPost[] comments = api.GetPosts(username);
            float commentAnalysis = PostAnalyzer(comments);
            return commentAnalysis;
        }

        public static float PostAnalyzer(FacebookPost[] comments)
        {
            string[] strings = new string[comments.Length];
            for (int i = 0; i < comments.Length; i++)
            {
                strings[i] = comments[i].Message;
            }
            return textAnalyzer.Analyze(strings);
        }
    }
}