using PoliticalDeterminer.Models;

namespace PoliticalDeterminer.Services
{
    public static class FacebookAnalyzer
    {
        private static TextAnalyzer textAnalyzer = TextAnalyzer.GetInstance();
        private static FacebookAPI api = new FacebookAPI();

        /// <summary>
        /// Analyzes the last 100 posts made by the given user and returns their estimated political leaning
        /// </summary>
        /// <param name="pageUrl">The url of the user's facebook page</param>
        /// <returns>A float representing estimated political leaning, with 0 being liberal, 1 being conservative, and 0.5 being moderate</returns>
        public static float Analyze(string pageUrl)
        {
            FacebookPost[] comments = api.GetPosts(pageUrl);
            float commentAnalysis = PostAnalyzer(comments);
            return commentAnalysis;
        }

        /// <summary>
        /// Analyzes the text content of an array of Facebook posts
        /// </summary>
        /// <param name="comments">The array of Facebook posts to analyze</param>
        /// <returns>A float representing estimated political leaning, with 0 being liberal, 1 being conservative, and 0.5 being moderate</returns>
        public static float PostAnalyzer(FacebookPost[] comments)
        {
            //Convert the array of FacebookPost to an array of sttring
            string[] strings = new string[comments.Length];
            for (int i = 0; i < comments.Length; i++)
            {
                strings[i] = comments[i].Message;
            }

            return textAnalyzer.Analyze(strings);
        }
    }
}