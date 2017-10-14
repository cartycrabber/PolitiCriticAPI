using System;

public static class RedditAnalyzer
{
    public static void analyzeTags(RedditComment[] comments)
    {
        ArrayList subs = new ArrayList();
        for (int i = 0; i < comments.length; i++)
        {
            if (!subs.contains(comments[i].subreddit))
            {
                subs.add(comments[i].subreddit);
            }
        }
        ArrayList numHitsPerSub = new ArrayList();
        for (int i = 0; i < subs.size(); i++)
        {
            //check if numHitsPerSub has subs[i] already
            //if it already has it, increments end number by 1
            //if not, adds "subs[i] + 1" to numHitsPerSub
            if (numHitsPerSub.contains(subs[i]))
            {

            }
        }
    }
}
