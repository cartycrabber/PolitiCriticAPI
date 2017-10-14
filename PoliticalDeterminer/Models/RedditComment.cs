using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoliticalDeterminer.Models
{

    public partial class RedditComment
    {
        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("num_comments")]
        public long NumComments { get; set; }

        [JsonProperty("banned_by")]
        public object BannedBy { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("approved_by")]
        public object ApprovedBy { get; set; }

        [JsonProperty("approved_at_utc")]
        public object ApprovedAtUtc { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("author_flair_text")]
        public string AuthorFlairText { get; set; }

        [JsonProperty("author_flair_css_class")]
        public string AuthorFlairCssClass { get; set; }

        [JsonProperty("banned_at_utc")]
        public object BannedAtUtc { get; set; }

        [JsonProperty("can_mod_post")]
        public bool CanModPost { get; set; }

        [JsonProperty("body_html")]
        public string BodyHtml { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("can_gild")]
        public bool CanGild { get; set; }

        [JsonProperty("collapsed_reason")]
        public object CollapsedReason { get; set; }

        [JsonProperty("collapsed")]
        public bool Collapsed { get; set; }

        [JsonProperty("controversiality")]
        public long Controversiality { get; set; }

        [JsonProperty("likes")]
        public object Likes { get; set; }

        [JsonProperty("edited")]
        public Edited Edited { get; set; }

        [JsonProperty("distinguished")]
        public object Distinguished { get; set; }

        [JsonProperty("created_utc")]
        public long CreatedUtc { get; set; }

        [JsonProperty("downs")]
        public long Downs { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("gilded")]
        public long Gilded { get; set; }

        [JsonProperty("is_submitter")]
        public bool IsSubmitter { get; set; }

        [JsonProperty("link_title")]
        public string LinkTitle { get; set; }

        [JsonProperty("link_id")]
        public string LinkId { get; set; }

        [JsonProperty("link_author")]
        public string LinkAuthor { get; set; }

        [JsonProperty("link_permalink")]
        public string LinkPermalink { get; set; }

        [JsonProperty("mod_reports")]
        public object[] ModReports { get; set; }

        [JsonProperty("link_url")]
        public string LinkUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("saved")]
        public bool Saved { get; set; }

        [JsonProperty("quarantine")]
        public bool Quarantine { get; set; }

        [JsonProperty("over_18")]
        public bool Over18 { get; set; }

        [JsonProperty("num_reports")]
        public object NumReports { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("replies")]
        public string Replies { get; set; }

        [JsonProperty("removal_reason")]
        public object RemovalReason { get; set; }

        [JsonProperty("report_reasons")]
        public object ReportReasons { get; set; }

        [JsonProperty("subreddit")]
        public string Subreddit { get; set; }

        [JsonProperty("score_hidden")]
        public bool ScoreHidden { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("stickied")]
        public bool Stickied { get; set; }

        [JsonProperty("subreddit_name_prefixed")]
        public string SubredditNamePrefixed { get; set; }

        [JsonProperty("ups")]
        public long Ups { get; set; }

        [JsonProperty("subreddit_id")]
        public string SubredditId { get; set; }

        [JsonProperty("subreddit_type")]
        public string SubredditType { get; set; }

        [JsonProperty("user_reports")]
        public object[] UserReports { get; set; }
    }

    public partial struct Edited
    {
        public long? Int;
        public bool? Bool;
        public float? Float;
    }

    public partial struct Edited
    {
        public Edited(JsonReader reader, JsonSerializer serializer)
        {
            Int = null;
            Bool = null;
            Float = null;

            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    Int = serializer.Deserialize<long>(reader);
                    break;
                case JsonToken.Boolean:
                    Bool = serializer.Deserialize<bool>(reader);
                    break;
                case JsonToken.Float:
                    Float = serializer.Deserialize<float>(reader);
                    break;
                default: throw new Exception("Cannot convert Edited");
            }
        }

        public void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            if (Int != null)
            {
                serializer.Serialize(writer, Int);
                return;
            }
            if (Bool != null)
            {
                serializer.Serialize(writer, Bool);
                return;
            }
            throw new Exception("Union must not be null");
        }
    }
}