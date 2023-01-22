using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Timers;
using MCDzienny.Settings;

namespace MCDzienny
{
    public static class Heartbeat
    {
        private static string hash;

        public static string serverURL;

        private static string staticVars;
        
        private static string mcdziennyMessage;
        
        private static HttpWebRequest request;

        private static StreamWriter beatlogger;

        private static Random Seed = new Random();

        private static readonly int betacraftHeartbeatDelay = 25000;

        private static readonly Timer heartbeatTimer;

        private static readonly Timer mcdziennyTimer = new Timer(240000.0);
        
        private static readonly Timer classiCube = new Timer(40000.0);

        public static char[] reservedChars =
        {
            ' ',
            '!',
            '*',
            '\'',
            '(',
            ')',
            ';',
            ':',
            '@',
            '&',
            '=',
            '+',
            '$',
            ',',
            '/',
            '?',
            '%',
            '#',
            '[',
            ']'
        };
        
        private static bool firstBeat = true;

        static Heartbeat()
        {
            heartbeatTimer = new Timer(betacraftHeartbeatDelay);
        }
        
        public static void Init()
        {
            if (Server.logbeat && !File.Exists("heartbeat.log")) File.Create("heartbeat.log").Close();
            staticVars = "version=" + 7;
            mcdziennyMessage = string.Concat(staticVars, "&serverversion=", Server.Version, "&sqlite=",
                !Server.useMySQL, "&chkport=", true);
            Pump(Beat.Betacraft);
            heartbeatTimer.AutoReset = false;
            heartbeatTimer.Elapsed += delegate
            {
                try
                {
                    Pump(Beat.Betacraft);
                }
                catch (Exception ex)
                {
                    Server.ErrorLog(ex);
                }

                heartbeatTimer.Start();
            };
            heartbeatTimer.Start();
            mcdziennyTimer.Start();
            classiCube.Elapsed += delegate
            {
                try
                {
                    if (GeneralSettings.All.AllowAndListOnClassiCube) Pump(Beat.ClassiCube);
                }
                catch (Exception ex)
                {
                    Server.ErrorLog(ex);
                }
            };
            classiCube.Start();
        }
        
        public static bool Pump(Beat type)
        {
            var text = "";
            text += staticVars;
            var uriString = "http://betacraft.uk/heartbeat.jsp";
            var num = 0;
            try
            {
                var num2 = 0;
                num++;
                switch (type)
                {
                    case Beat.Betacraft:
                    {
                        if (Server.logbeat) beatlogger = new StreamWriter("heartbeat.log", true);
                        var stringBuilder = new StringBuilder();
                        stringBuilder.AppendFormat(
                            "version={0}&port={1}&max={2}&name={3}&salt={4}&users={5}&public={6}", 7, Server.port,
                            Server.players, UrlEncode(Server.name), Server.Salt, Player.number - num2, Server.isPublic);
                        text = stringBuilder.ToString();
                        break;
                    }
                    case Beat.ClassiCube:
                    {
                        var stringBuilder2 = new StringBuilder();
                        stringBuilder2.AppendFormat(
                            "public={0}&max={1}&users={2}&port={3}&version={4}&salt={5}&name={6}&software=MCDzienny 12.0.0",
                            Server.isPublic, Server.players, Player.number - num2, Server.port, 7,
                            Server.SaltClassiCube, UrlEncode(Server.name));
                        text = stringBuilder2.ToString();
                        uriString = "http://www.classicube.net/server/heartbeat";
                        break;
                    }
                }

                request = (HttpWebRequest) WebRequest.Create(new Uri(uriString));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                var bytes = Encoding.ASCII.GetBytes(text);
                request.ContentLength = bytes.Length;
                request.Timeout = 10000;
                try
                {
                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                        if (type == Beat.Betacraft && Server.logbeat)
                            beatlogger.WriteLine("Request sent at " + DateTime.Now);
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.Timeout)
                    {
                        if (type == Beat.Betacraft && Server.logbeat)
                            beatlogger.WriteLine("Timeout detected at " + DateTime.Now);
                    }
                    else if (type == Beat.Betacraft && Server.logbeat)
                    {
                        beatlogger.WriteLine("Non-timeout exception detected: " + ex.Message);
                        beatlogger.Write("Stack Trace: " + ex.StackTrace);
                    }
                }

                if (type == Beat.Betacraft)
                    try
                    {
                        using (var response = request.GetResponse())
                        {
                            using (var streamReader = new StreamReader(response.GetResponseStream()))
                            {
                                var text2 = streamReader.ReadToEnd().Trim();
                                if (hash == null)
                                {
                                    if (type == Beat.Betacraft && Server.logbeat)
                                    {
                                        beatlogger.WriteLine("Response received at " + DateTime.Now);
                                        beatlogger.WriteLine("Received: " + text2);
                                    }

                                    hash = text2.Substring(text2.LastIndexOf('=') + 1);
                                    Server.ServerHash = hash;
                                    serverURL = text2;
                                    Server.s.UpdateUrl(serverURL);
                                    File.WriteAllText("text/externalurl.txt", text2);
                                    Server.s.Log("URL found: " + text2);
                                    Pump(Beat.MCDzienny);
                                }
                                else if (type == Beat.Betacraft && Server.logbeat)
                                {
                                    beatlogger.WriteLine("Response received at " + DateTime.Now);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                if (type == Beat.ClassiCube)
                    try
                    {
                        using (var response2 = request.GetResponse())
                        {
                            using (var streamReader2 = new StreamReader(response2.GetResponseStream()))
                            {
                                if (firstBeat)
                                {
                                    firstBeat = false;
                                    var str = streamReader2.ReadToEnd().Trim();
                                    Server.s.Log("ClassiCube URL found: " + str);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
            }
            catch (WebException ex2)
            {
                if (ex2.Status == WebExceptionStatus.Timeout && type == Beat.Betacraft && Server.logbeat)
                    beatlogger.WriteLine("Timeout detected at " + DateTime.Now);
                if (type == Beat.Betacraft && hash == null)
                    Server.s.Log("Warning: Couldn't get response from betacraft.uk website. Retrying...");
            }
            catch
            {
                if (type == Beat.Betacraft && Server.logbeat)
                    beatlogger.WriteLine(string.Concat("Heartbeat failure #", num, " at ", DateTime.Now.ToString()));
                if (type == Beat.Betacraft && Server.logbeat)
                {
                    beatlogger.WriteLine("Failed three times.  Stopping.");
                    beatlogger.Close();
                }

                return false;
            }
            finally
            {
                request.Abort();
            }

            if (beatlogger != null) beatlogger.Close();
            return true;
        }

        public static string UrlEncode(string input)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < input.Length; i++)
                if (input[i] >= '0' && input[i] <= '9' || input[i] >= 'a' && input[i] <= 'z' ||
                    input[i] >= 'A' && input[i] <= 'Z' || input[i] == '-' || input[i] == '_' || input[i] == '.' ||
                    input[i] == '~')
                    stringBuilder.Append(input[i]);
                else if (Array.IndexOf(reservedChars, input[i]) != -1)
                    stringBuilder.Append('%').Append(((int) input[i]).ToString("X"));
            return stringBuilder.ToString();
        }
    }
}
