using PoliticalDeterminer.Models;
using System;
using System.Collections;
using System.Linq;

public static class RedditAnalyzer
{
    public static string[] AnalyzerOfMagicAndPower(RedditComment[] comments)
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
        string[] sortedSubs = new string[subs.Count];
        j = 0;
        while (sortedSubs.Contains(""))
        {
            int maxValue = subPings.Max();
            int maxIndex = subPings.ToList().IndexOf(maxValue);
            sortedSubs[j] = (string) subs[maxIndex];
            subPings[j] = int.MinValue;
        }
        return sortedSubs;
    }
}
