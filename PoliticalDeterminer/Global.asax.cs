using PoliticalDeterminer.Models;
using PoliticalDeterminer.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PoliticalDeterminer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);

            Test();
        }

        private void Test()
        {
            /*FacebookAPI fb = new FacebookAPI();
            List<string> posts = new List<string>();
            posts.Add("text");
            posts.AddRange(fb.GetPostMessages("DonaldTrump"));
            posts.AddRange(fb.GetPostMessages("hillaryclinton"));
            posts.AddRange(fb.GetPostMessages("103209476386105"));
            posts.AddRange(fb.GetPostMessages("FoxNews"));
            posts.AddRange(fb.GetPostMessages("cnn"));
            posts.AddRange(fb.GetPostMessages("bbcnews"));
            posts.AddRange(fb.GetPostMessages("HuffPost"));
            posts.AddRange(fb.GetPostMessages("JerrySeinfeld"));
            posts.AddRange(fb.GetPostMessages("100001006333168"));
            posts.AddRange(fb.GetPostMessages("249653248548055"));
            posts.AddRange(fb.GetPostMessages("126736391195204"));
            posts.AddRange(fb.GetPostMessages("naacp"));
            posts.AddRange(fb.GetPostMessages("antiFaUSA"));
            posts.AddRange(fb.GetPostMessages("400347320041300"));
            posts.AddRange(fb.GetPostMessages("aclu"));
            int fbCount = posts.Count;
            Debug.WriteLine($"Collected {fbCount} Facebook Text Posts");

            RedditAPI rd = new RedditAPI();
            posts.AddRange(rd.GetCommentsText("gallowboob"));
            posts.AddRange(rd.GetCommentsText("the-realDonaldTrump"));
            posts.AddRange(rd.GetCommentsText("HillaryClinton"));
            posts.AddRange(rd.GetCommentsText("SQUEEEEEEEEEPS"));
            posts.AddRange(rd.GetCommentsText("regularly-lies"));
            posts.AddRange(rd.GetCommentsText("awake-at-dawn"));
            posts.AddRange(rd.GetCommentsText("SeriousBlak"));
            posts.AddRange(rd.GetCommentsText("hillaryisgoingdown"));
            posts.AddRange(rd.GetCommentsText("MiceTonerAccount"));
            posts.AddRange(rd.GetCommentsText("progress18"));
            posts.AddRange(rd.GetCommentsText("Rehkit"));
            posts.AddRange(rd.GetCommentsText("wenchette"));
            posts.AddRange(rd.GetCommentsText("untildeath"));
            posts.AddRange(rd.GetCommentsText("CareToRemember"));
            posts.AddRange(rd.GetCommentsText("thetenmeows"));
            Debug.WriteLine($"Collected {posts.Count - fbCount} Reddit Comments");

            for(int i = 0; i < posts.Count; i++)
            {
                posts[i] = ($"\"{posts[i].Replace("\"", "\"\"")}\"");
            }

            File.WriteAllLines(@"C:\Development\HackGT4\SamplePosts.csv", posts);
            */

            List<string> conservativeComments = new List<string>();
            List<string> liberalComments = new List<string>();

            RedditAPI rd = new RedditAPI();
            FacebookAPI fb = new FacebookAPI();

            conservativeComments.AddRange(fb.GetPostMessages("DonaldTrump"));
            conservativeComments.AddRange(fb.GetPostMessages("VicePresidentPence"));
            conservativeComments.AddRange(fb.GetPostMessages("100000544532899"));
            conservativeComments.AddRange(fb.GetPostMessages("100001387924004"));
            conservativeComments.AddRange(fb.GetPostMessages("100001351601986"));

            conservativeComments.AddRange(rd.GetCommentsText("JackalSpat"));
            conservativeComments.AddRange(rd.GetCommentsText("neemarita"));
            conservativeComments.AddRange(rd.GetCommentsText("AVDLatex"));
            conservativeComments.AddRange(rd.GetCommentsText("optionhome"));
            conservativeComments.AddRange(rd.GetCommentsText("2481632641282565121k"));

            liberalComments.AddRange(fb.GetPostMessages("100007731198419"));
            liberalComments.AddRange(fb.GetPostMessages("1534242220"));
            liberalComments.AddRange(fb.GetPostMessages("1521723653"));
            liberalComments.AddRange(fb.GetPostMessages("100001884654096"));
            liberalComments.AddRange(fb.GetPostMessages("hillaryclinton"));

            liberalComments.AddRange(rd.GetCommentsText("Mentoman72"));
            liberalComments.AddRange(rd.GetCommentsText("jereddit"));
            liberalComments.AddRange(rd.GetCommentsText("TheHeckWithItAll"));
            liberalComments.AddRange(rd.GetCommentsText("bustopher-jones"));
            liberalComments.AddRange(rd.GetCommentsText("TheSelfGoverned"));
            liberalComments.AddRange(rd.GetCommentsText("Splatypus"));
        }
    }
}
