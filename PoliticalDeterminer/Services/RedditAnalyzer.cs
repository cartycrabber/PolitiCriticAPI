using PoliticalDeterminer.Models;
using System;
using System.Collections;

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
        for (int i = 0; i < sortedSubs.Length; i++)
        {

        }
        return new string[3];
    }
}
