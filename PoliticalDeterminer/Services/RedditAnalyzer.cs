using PoliticalDeterminer.Models;
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

    public static float SubredditKarmaAnalyzer(RedditComment[] comments)
    {
        return -1;
    }

    public static float SubredditActivityAnalyzer(RedditComment[] comments)
    {
        
        ArrayList subs = new ArrayList();
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
        for (int i = 0; i < subs.Count; i++)
        {
            numHitsPerSub.Add(comments[i].Subreddit);
        }
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
        int[] subPings2 = new int[subs.Count];
        for (int i = 0; i < subPings.Length; i++)
        {
            subPings2[i] = subPings[i];
        }
        string[] sortedSubs = new string[subs.Count];
        j = 0;
        int a = 0;
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
        for (int i = 0; i < sortedSubs.Length; i++)
        {
            if (conservativeSubs.Contains(sortedSubs[i]))
            {
                extremity += subPings2[i] / (i + 1);
            }
            if (liberalSubs.Contains(sortedSubs[i]))
            {
                extremity -= subPings2[i] / (i + 1);
            }
        }
        return extremity;
    }

    public static string[] CommentAnalyzer(RedditComment[] comments)
    {
        //Use Will's class pleaze
    }
}
