using System;
using System.Collections.Generic;
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
            return new Random().NextDouble() * 2.0 - 1.0;
        }

        [Route("leaning/reddit/{id}")]
        public double GetRedditLeaning(string id)
        {
            return new Random().NextDouble() * 2.0 - 1.0;
        }
    }
}