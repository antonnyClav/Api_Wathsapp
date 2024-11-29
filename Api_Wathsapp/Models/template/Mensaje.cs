namespace Api_Wathsapp.Models.template
{
    public class Mensaje
    {
        public string messaging_product { get; set; }
        public string recipient_type { get; set; }
        public string to { get; set; }
        public string type { get; set; }
        public Text text { get; set; }
    }

    public class Text
    {
        public bool preview_url { get; set; }
        public string body { get; set; }
    }
}
