﻿using PoliticalDeterminer.Models;
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

            //GatherTrainingData();
            //Test();
        }

        //Just a function used to test the classifier
        private void Test()
        {
            TextAnalyzer analyzer = TextAnalyzer.GetInstance();
            float result = analyzer.Analyze("");
            Debug.WriteLine(result);
        }

        //Gathers comments from a few specified facebook and reddit accounts to use as training data
        private void GatherTrainingData()
        {
            List<string> conservativeComments = new List<string>();
            List<string> liberalComments = new List<string>();

            RedditAPI rd = new RedditAPI();
            FacebookAPI fb = new FacebookAPI();

            //TODO: Check if these still work with the changes to GetPosts
            conservativeComments.AddRange(fb.GetPostMessages("DonaldTrump"));
            conservativeComments.AddRange(fb.GetPostMessages("VicePresidentPence"));
            conservativeComments.AddRange(fb.GetPostMessages("100000544532899"));
            conservativeComments.AddRange(fb.GetPostMessages("100001387924004"));
            conservativeComments.AddRange(fb.GetPostMessages("100001351601986"));
            conservativeComments.AddRange(fb.GetPostMessages("100007241612747"));

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

            string liberalPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Data/liberal_training.txt");
            string conservativePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Data/conservative_training.txt");

            File.WriteAllLines(liberalPath, liberalComments.ToArray());
            File.WriteAllLines(conservativePath, conservativeComments.ToArray());
        }
    }
}
