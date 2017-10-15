using PoliticalDeterminer.Services;
using System.Diagnostics;
using System.Web.Http;

namespace PoliticalDeterminer.Controllers
{
    public class LeaningController : ApiController
    {
        //These are the entry points to the API from the web

        [Route("leaning/facebook")]
        public double GetFacebookLeaning([FromUri]string user)
        {
            Debug.WriteLine("Entering Facebook Endpoint");
            return FacebookAnalyzer.Analyze(user);
        }

        [Route("leaning/reddit")]
        public double GetRedditLeaning([FromUri]string user)
        {
            Debug.WriteLine("Entering Reddit Endpoint");
            return RedditAnalyzer.Analyze(user);
        }
    }
}