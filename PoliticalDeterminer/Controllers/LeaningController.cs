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

        [Route("leaning/facebook/{id}")]
        public double GetFacebookLeaning(string id)
        {
            Debug.WriteLine("Entering Facebook Endpoint");
            return new Random().NextDouble() * 2.0 - 1.0;
        }

        [Route("leaning/reddit/{id}")]
        public double GetRedditLeaning(string id)
        {
            Debug.WriteLine("Entering Reddit Endpoint");
            return RedditAnalyzer.Analyze(id);
        }
    }
}