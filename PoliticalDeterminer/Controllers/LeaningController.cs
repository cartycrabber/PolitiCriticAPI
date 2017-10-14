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
        public int GetFacebookLeaning(string id)
        {
            return 0;
        }

        [Route("leaning/reddit/{id}")]
        public int GetRedditLeaning(string id)
        {
            return 0;
        }
    }
}