﻿using System.Diagnostics;
using System.Timers;
using MCDzienny.Gui;
using MCDzienny.Settings;
using MCDzienny_.Gui;

namespace MCDzienny
{
    internal class CmdShutdown : Command
    {
        private static readonly Timer shutdownTimer = new Timer();

        private static int lastSeconds = 3;

        private static bool shutdownInProgress;

        public override string name
        {
            get { return "shutdown"; }
        }
        
        public override string shortcut
        {
            get { return ""; }
        }

        public override string type
        {
            get { return "mod"; }
        }

        public override bool museumUsable
        {
            get { return true; }
        }

        public override LevelPermission defaultRank
        {
            get { return LevelPermission.Admin; }
        }

        public override void Use(Player p, string message)
        {
            if (message != "")
            {
                Help(p);
                return;
            }

            if (shutdownInProgress)
            {
                Player.SendMessage(p, "@The shutdown timer was stopped.");
                Player.GlobalChatWorld(null, "%cShutdown aborted!", false);
                Server.s.Log("Shutdown aborted.");
                shutdownInProgress = false;
                Server.shuttingDown = false;
                shutdownTimer.Elapsed -= RestartTimerCallback1;
                shutdownTimer.Elapsed -= RestartTimerCallback2;
                return;
            }

            lastSeconds = 3;
            Server.s.Log("The server will be shut down in 20 seconds.");
            Player.GlobalChatWorld(null, "%cThe server will be shut down in 20 seconds.", false);
            shutdownTimer.Interval = 10000.0;
            shutdownTimer.Elapsed += RestartTimerCallback1;
            shutdownTimer.Start();
            shutdownInProgress = true;
            Server.shuttingDown = true;
        }

        public void RestartTimerCallback1(object sender, ElapsedEventArgs e)
        {
            if (!shutdownInProgress) return;
            Player.GlobalChatWorld(null, "%c10 seconds to shutdown.", false);
            Server.s.Log("10 seconds to shutdown.");
            shutdownTimer.Interval = 7000.0;
            shutdownTimer.Elapsed -= RestartTimerCallback1;
            shutdownTimer.Elapsed += RestartTimerCallback2;
        }

        public void RestartTimerCallback2(object sender, ElapsedEventArgs e)
        {
            if (!shutdownInProgress) return;
            shutdownTimer.Interval = 1000.0;
            Player.GlobalChatWorld(null, lastSeconds == 0 ? "%cShutdown!" : "%c" + lastSeconds, false);
            lastSeconds--;
            if (lastSeconds == 0)
            {
                shutdownTimer.Elapsed -= RestartTimerCallback2;
                Shutdown();
            }
        }
        
        public void Shutdown()
        {
            GuiSettings.All.Save();
            if (!Program.SaveAll(false)) return;
            try
            {
                if (!Server.CLI && Window.thisWindow.notifyIcon1 != null)
                {
                    Window.thisWindow.notifyIcon1.Icon = null;
                    Window.thisWindow.notifyIcon1.Visible = false;
                }
            }
            catch
            {
            }

            Process.GetCurrentProcess().Kill();
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, "/shutdown - shuts down the server within 20 seconds,");
            Player.SendMessage(p, "/shutdown - when used again it aborts the shutdown.");
        }
    }
}
