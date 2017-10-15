using PoliticalDeterminer.Models;
using PoliticalDeterminer.Services;
using System;
using System.Collections;
using System.Linq;

public static class RedditAnalyzer
{
    private static string[] conservativeSubs = { "republican", "conservative", "the_donald", "conservatives", "monarchism", "new_right",
    "objectivism", "paleoconservative", "republicans", "romney", "trueobjectivism" };

    private static string[] liberalSubs = { "liberal", "alltheleft", "classical_liberals", "cornbreadliberals", "democrats", "demsocialist",
    "greenparty", "labor", "leftcommunism", "leninism", "neoprogs", "obama", "progressive", "socialdemocracy", "socialism" };

    private static string[] commentText;

    private static TextAnalyzer textAnalyzer = TextAnalyzer.GetInstance();

    private static RedditAPI api = new RedditAPI();

    public static float Analyze(string username)
    {
        RedditComment[] comments = api.GetComments(username);
        //int subKarma = SubredditKarmaAnalyzer(comments);
        //float subActivity = SubredditActivityAnalyzer(comments);
        float commentAnalysis = CommentAnalyzer(comments);
        return commentAnalysis;
    }

    public static int SubredditKarmaAnalyzer(RedditComment[] comments)
    {
        ArrayList subs = new ArrayList();
        ArrayList deltaSubs = new ArrayList();
        for (int i = 0; i < comments.Length; i++)
        {
            subs.Add(comments[i].Subreddit);
            subs.Add(comments[i].Score);
            if (!deltaSubs.Contains(comments[i].Subreddit))
            {
                deltaSubs.Add(comments[i].Subreddit);
            }
        }
        ArrayList subs2 = new ArrayList();
        for (int i = 0; i < subs.Count; i++)
        {
            subs2[i] = subs[i];
        }
        ArrayList subPopularityInOrder = new ArrayList();
        ArrayList subScoreInOrder = new ArrayList();
        for (int i = 0; i < subs.Count; i++)
        {
            subScoreInOrder[i] = int.MinValue;
        }
        //First need list of subs in order of score, then need scores themselves
        while (subScoreInOrder.Count < deltaSubs.Count)
        {
            string currentSub = (string) subs2[0];
            int score = 0;
            for (int i = 0; i < subs.Count; i++)
            {
                if (subs[i].Equals(currentSub))
                {
                    score += (int) subs[i + 1];
                    subs2.Remove(i);
                    subs2.Remove(i);
                }
            }
            int index = 0;
            while ((int) subScoreInOrder[index] > score)
            {
                index++;
            }
            subScoreInOrder.Insert(index, score);
            subPopularityInOrder.Insert(index, currentSub);
        }
        int extremity = 0;
        for (int i = 0; i < (int) subScoreInOrder.Count; i++)
        {
            if (conservativeSubs.Contains(subPopularityInOrder[i]))
            {
                extremity += (int) subScoreInOrder[i];
            }
            if (liberalSubs.Contains(subPopularityInOrder[i]))
            {
                extremity -= (int) subScoreInOrder[i];
            }
        }
        return extremity;
    }

    public static float SubredditActivityAnalyzer(RedditComment[] comments)
    {
        
        ArrayList subs = new ArrayList();

        //Finding all subs found in comments, 1 instance of each sub
        for (int i = 0; i < comments.Length; i++)
        {
            if (!subs.Contains(comments[i].Subreddit))
            {
                subs.Add(comments[i].Subreddit);
            }
        }
        ArrayList numHitsPerSub = new ArrayList();
        int[] subPings = new int[subs.Count];
        int j = 0;
        //Finds all subs found in comments, creates copies of the same sub
        for (int i = 0; i < subs.Count; i++)
        {
            numHitsPerSub.Add(comments[i].Subreddit);
        }
        //counts number of times sub is found
        while (numHitsPerSub.Count > 0)
        {
            string sub = (string) numHitsPerSub[0];
            int counter = 0;
            while (numHitsPerSub.IndexOf(sub) > -1)
            {
                numHitsPerSub.Remove(numHitsPerSub.IndexOf(sub));
                counter++;
            }
            subPings[j] = counter;
            j++;
        }
        ArrayList subPings2 = new ArrayList();
        for (int i = 0; i < subPings.Length; i++)
        {
            subPings2[i] = subPings[i];
        }
        string[] sortedSubs = new string[subs.Count];
        j = 0;
        int a = 0;
        //Finds most popular subreddit, adds that one first to end string[]
        while (sortedSubs.Contains(""))
        {
            int maxValue = subPings.Max();
            int maxIndex = subPings.ToList().IndexOf(maxValue);
            sortedSubs[j] = (string) subs[maxIndex];
            subPings2[a] = subPings[j];
            subPings[j] = int.MinValue;
            a++;
        }
        float extremity = 0;
        int prevNumber = 0;
        int delta = 0;
        //Loops through the sub pings and sets the extremity based off of the sub pings and the popularity of each sub
        for (int i = 0; i < sortedSubs.Length; i++) //MIGHT HAVE TO CHANGE "i < sortedSubs.Length" TO "subPings2.Count > 0"
        {
            //If multiple subreddits have the same number of pings, they affect the extremity value the same amount
            if (prevNumber == (int) subPings2[0])
            {
                delta++;
            } else
            {
                delta = 0;
            }
            prevNumber = (int) subPings2[0];
            if (conservativeSubs.Contains(sortedSubs[i]))
            {
                extremity += (int) subPings2[0] / (i + 1 - delta);
            }
            if (liberalSubs.Contains(sortedSubs[i]))
            {
                extremity -= (int) subPings2[0] / (i + 1 - delta);
            }
            subPings2.Remove(0);
        }
        return extremity;
    }

    public static float CommentAnalyzer(RedditComment[] comments)
    {
        string[] strings = new string[comments.Length];
        for(int i = 0; i < comments.Length; i++)
        {
            strings[i] = comments[i].Body;
        }
        return textAnalyzer.Analyze(strings);
    }
}
