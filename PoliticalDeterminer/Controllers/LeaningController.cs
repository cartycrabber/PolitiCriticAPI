using PoliticalDeterminer.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PoliticalDeterminer.Controllers
{
    public class LeaningController : ApiController
    {

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