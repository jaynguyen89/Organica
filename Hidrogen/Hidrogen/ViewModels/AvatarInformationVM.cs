using Newtonsoft.Json;

namespace Hidrogen.ViewModels {

    public class AvatarInformationVM {

        public string Id { get; set; }

        [JsonProperty("url_viewer")]
        public string UrlViewer { get; set; }

        [JsonProperty("url")]
        public string DirectUrl { get; set; }

        [JsonProperty("display_url")]
        public string DisplayUrl { get; set; }

        [JsonProperty("time")]
        public long UploadedOn { get; set; }

        [JsonProperty("thumb")]
        public AvatarVM Thumbnail { get; set; }

        public AvatarVM Medium { get; set; }

        public AvatarVM Image { get; set; }

        [JsonProperty("delete_url")]
        public string DeleteEndpoint { get; set; }
    }

    public class AvatarVM {

        [JsonProperty("filename")]
        public string FileName { get; set; }

        [JsonProperty("url")]
        public string FileUrl { get; set; }

        [JsonProperty("size")]
        public int FileSize { get; set; }
    }
}