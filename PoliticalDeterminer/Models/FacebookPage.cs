// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using PoliticalDeterminer.Models;
//
//    var data = FacebookPage.FromJson(jsonString);
//
namespace PoliticalDeterminer.Models
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class FacebookPage
    {
        [JsonProperty("data")]
        public FacebookPost[] Data { get; set; }

        [JsonProperty("paging")]
        public FBPaging Paging { get; set; }
    }

    public partial class FacebookPost
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created_time")]
        public string CreatedTime { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("story")]
        public string Story { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, Created: {CreatedTime}, Message: {Message}, Story: {Story}";
        }
    }

    public partial class FBPaging
    {
        [JsonProperty("cursors")]
        public FBCursors Cursors { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }
    }

    public partial class FBCursors
    {
        [JsonProperty("after")]
        public string After { get; set; }

        [JsonProperty("before")]
        public string Before { get; set; }
    }

    public partial class FacebookPage
    {
        public static FacebookPage FromJson(string json) => JsonConvert.DeserializeObject<FacebookPage>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this FacebookPage self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
