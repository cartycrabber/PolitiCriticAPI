namespace PoliticalDeterminer.Models
{
    using System;

    using Newtonsoft.Json;

    //Generalize this class so that it can later be extended to other listings, like working with subreddits
    public partial class RedditListing<T>
    {
        [JsonProperty("data")]
        public RedditListingData<T> Data { get; set; }

        [JsonProperty("kind")]
        public string Kind { get; set; }
    }

    public partial class RedditListingData<T>
    {
        [JsonProperty("before")]
        public object Before { get; set; }

        [JsonProperty("modhash")]
        public string Modhash { get; set; }

        [JsonProperty("after")]
        public string After { get; set; }

        [JsonProperty("children")]
        public RedditListingChild<T>[] Children { get; set; }

        [JsonProperty("whitelist_status")]
        public string WhitelistStatus { get; set; }
    }

    public partial class RedditListingChild<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("kind")]
        public string Kind { get; set; }
    }

    public partial class RedditListing<T>
    {
        public static RedditListing<T> FromJson(string json) => JsonConvert.DeserializeObject<RedditListing<T>>(json, RedditListingConverter.Settings);
    }

    public class RedditListingConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Edited);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (t == typeof(Edited))
                return new Edited(reader, serializer);
            throw new Exception("Unknown type");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = value.GetType();
            if (t == typeof(Edited))
            {
                ((Edited)value).WriteJson(writer, serializer);
                return;
            }
            throw new Exception("Unknown type");
        }

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = { new RedditListingConverter() },
        };
    }
}
