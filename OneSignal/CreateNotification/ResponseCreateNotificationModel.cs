using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneSignal.Push
{
    public class ResponseCreateNotificationModel
    {
        public string id { get; set; }
        public int recipients { get; set; }
        public string external_id { get; set; }
        public Errors errors { get; set; }
    }

    public class Errors
    {
        public string[] invalid_player_ids { get; set; }
    } 
}