#define __LOG

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace API // API main pms
{
    /// <summary>
    /// Summary description for Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    // [System.Web.Script.Services.ScriptService]
    public class Service : System.Web.Services.WebService
    {
       
        private const long timeTO = (60 * 60);
        private const int SystemGMT = 7;
#if __LOG
        private string path = "log";
#else
        //private string path = "";
#endif

        #region private method

#if (__LOG)

        private void LOG(string data)
        {
            try
            {
                if (path != "")
                {
                    TextWriter file = new StreamWriter(HttpContext.Current.Server.MapPath(".") + "\\" + path + "_" + DateTime.Now.ToString("MMyyyy") + ".txt", true);

                    file.WriteLine(DateTime.Now.ToString() + ":::" + data);
                    file.Close();
                }
            }
            catch
            {
            }
        }

#endif

        #endregion private method
        #region Time convertion
        public DateTime DateTimeNowToGMT0(int gmt)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = DateTime.Now;
            dtDateTime = dtDateTime.AddSeconds(-gmt * 3600);
            return dtDateTime;
        }
        #endregion
        #region MCE
        #region MessageUpdate
        [WebMethod(Description = "MessageUpdate ")]
        public void MessageUpdate(string input)

        {
#if (__LOG)
            LOG("MessageUpdate Input: " + input);
#endif
            List<API.GetPlayerId> _playerIdLst = new List<API.GetPlayerId>();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {


                if (string.IsNullOrWhiteSpace(input))
                {
                    return;
                }

#if (__LOG)
                LOG("MessageUpdate Decode Input: " + input);
#endif

                // Step 4 :Make request to onesignal
                // using System.Net;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(OneSignalUrl.Notifications);
                httpWebRequest.KeepAlive = true;
                httpWebRequest.ContentType = OneSignalContentType.JsonUTF8;
                httpWebRequest.Method = OneSignalMethod.POST;
                httpWebRequest.Headers.Add(OneSignalHeader.Authorization, "Basic " + UserConstant.RestAPIKey);

                OneSignal.Push.RequestCreateNotificationModel _req = new OneSignal.Push.RequestCreateNotificationModel()
                {
                    app_id = UserConstant.OneSignalAppId,
                    include_player_ids = new string[_playerIdLst.Count],
                    headings = new OneSignal.Push.Headings
                    {
                        en = "Tittle",
                        vi = "Tiêu đề",
                    }
                };

                _req.contents = new OneSignal.Push.Contents()
                {
                    en = String.Format("Your message"),
                    vi = String.Format("Tin nhắn")
                };


                try
                {
                    using (var stream = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        stream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(_req).ToString());
                    }

                    // Step 5 :Get response from onesignal
                    HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var stream = new StreamReader(response.GetResponseStream()))
                    {
                        var _retval = stream.ReadToEnd();
#if (__LOG)
                        LOG("\r\nGet response:" + _retval);
#endif

                        if (string.IsNullOrWhiteSpace(_retval))
                        {

                            return;
                        }

                        OneSignal.Push.ResponseCreateNotificationModel _resp = Newtonsoft.Json.JsonConvert.DeserializeObject<OneSignal.Push.ResponseCreateNotificationModel>(_retval);
                    }
                }
                catch (WebException e)
                {
#if (__LOG)
                    LOG("Error Response 0: " + e.ToString());
#endif
                    StreamReader sr = new StreamReader(e.Response.GetResponseStream());
#if (__LOG)
                    LOG("Error Response 1: " + sr.ReadToEnd());
#endif
                }

                
                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);
#if (__LOG)
                LOG("MessageUpdate Runtime: " + elapsedTime);
#endif
                return;
            }
#if (__LOG)
            catch (Exception e)
            {
                LOG("MessageUpdate API Fail: " + e.ToString());
                return;
            }
#else
            catch (Exception)
            {
#if (__LOG)
                LOG("MessageUpdate API FAIL");
#endif
                result = string.Empty; 
                return result;
            }
#endif
        }
        #endregion MessageUpdate
        #endregion MCE
    }
}