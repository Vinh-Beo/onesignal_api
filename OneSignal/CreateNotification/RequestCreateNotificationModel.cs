namespace OneSignal.Push
{
    public class RequestCreateNotificationModel
    {
        public string app_id { get; set; }
        public string external_id { get; set; }
        public Contents contents { get; set; }
        public Headings headings { get; set; }
        public Button[] buttons { get; set; }
        public object data { get; set; }
        public string[] include_player_ids { get; set; }
    }

    public class Contents
    {
        public string en { get; set; }
        public string vi { get; set; }
    }
    public class Headings
    {
        public string en { get; set; }
        public string vi { get; set; }
    }

    public class Button
    {
        public string id { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
    }

    public class Text
    {
        public string en { get; set; }
        public string vi { get; set; }
    }

    public class Data
    {
        public string code { get; set; }
        public string type { get; set; }
        public int index { get; set; }
        public int channel { get; set; }
    }
    public class SceneData
    {
        public string code { get; set; }
        public string type { get; set; }
        public long id { get; set; }
        public int mceIndex { get; set; }
    }
}