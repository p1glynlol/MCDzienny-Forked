using System.Collections.Generic;
using MCDzienny.Commands;

namespace MCDzienny
{
    public abstract class Command
    {
        public static CommandList all = new CommandList();

        public static CommandList core = new CommandList();

        public abstract string name { get; }

        public abstract string shortcut { get; }

        public abstract string type { get; }

        public abstract bool museumUsable { get; }

        public abstract LevelPermission defaultRank { get; }

        public virtual bool ConsoleAccess { get { return true; } }

        public virtual CommandScope Scope { get { return CommandScope.All; } }

        public virtual bool HighSecurity { get { return false; } }

        public virtual string CustomName { get { return null; } }

        public abstract void Use(Player p, string message);

        public abstract void Help(Player p);

        public virtual void Init() {}

        protected bool StopConsoleUse(Player p)
        {
            if (p == null)
            {
                Player.SendMessage(p, "You can't use this command from console.");
                return true;
            }
            return false;
        }

        public bool IsWithinScope(Player p)
        {
            if (Scope == CommandScope.All)
            {
                return true;
            }
            if (p.level.mapType == MapType.Lava && (Scope & CommandScope.Lava) == CommandScope.Lava)
            {
                return true;
            }
            if ((p.level.mapType == MapType.Freebuild || p.level.mapType == MapType.Home || p.level.mapType == MapType.MyMap) &&
                (Scope & CommandScope.Freebuild) == CommandScope.Freebuild)
            {
                return true;
            }
            if (p.level.mapType == MapType.Zombie && (Scope & CommandScope.Zombie) == CommandScope.Zombie)
            {
                return true;
            }
            if (p.level.mapType == MapType.Home && (Scope & CommandScope.Home) == CommandScope.Home)
            {
                return true;
            }
            if (p.level.mapType == MapType.MyMap && (Scope & CommandScope.MyMap) == CommandScope.MyMap)
            {
                return true;
            }
            return false;
        }

        public static void InitAll()
        {
            all.Add(new CmdAbort());
            all.Add(new CmdAbout());
            all.Add(new CmdAfk());
            all.Add(new CmdBan());
            all.Add(new CmdBanip());
            all.Add(new CmdBind());
            all.Add(new CmdBlocks());
            all.Add(new CmdBlockSet());
            all.Add(new CmdBotAdd());
            all.Add(new CmdBotAI());
            all.Add(new CmdBotRemove());
            all.Add(new CmdBots());
            all.Add(new CmdBotSet());
            all.Add(new CmdBotSummon());
            all.Add(new CmdClearBlockChanges());
            all.Add(new CmdClick());
            all.Add(new CmdClones());
            all.Add(new CmdCmdBind());
            all.Add(new CmdCmdSet());
            all.Add(new CmdCmdUnload());
            all.Add(new CmdColor());
            all.Add(new CmdCopy());
            all.Add(new CmdCuboid());
            all.Add(new CmdDelete());
            all.Add(new CmdDeleteLvl());
            all.Add(new CmdDrop());
            all.Add(new CmdEmote());
            all.Add(new CmdFill());
            all.Add(new CmdFixGrass());
            all.Add(new CmdFlipHeads());
            all.Add(new CmdFly());
            all.Add(new CmdFollow());
            all.Add(new CmdFreeze());
            all.Add(new CmdGive());
            all.Add(new CmdGoto());
            all.Add(new CmdGun());
            all.Add(new CmdHacks());
            all.Add(new CmdHasirc());
            all.Add(new CmdHelp());
            all.Add(new CmdHide());
            all.Add(new CmdHighlight());
            all.Add(new CmdImport());
            all.Add(new CmdImageprint());
            all.Add(new CmdInbox());
            all.Add(new CmdInfo());
            all.Add(new CmdInvincible());
            all.Add(new CmdJail());
            all.Add(new CmdJoker());
            all.Add(new CmdKick());
            all.Add(new CmdKickban());
            all.Add(new CmdKill());
            all.Add(new CmdLastCmd());
            all.Add(new CmdLevels());
            all.Add(new CmdLimit());
            all.Add(new CmdLine());
            all.Add(new CmdLoad());
            all.Add(new CmdLowlag());
            all.Add(new CmdMap());
            all.Add(new CmdMapInfo());
            all.Add(new CmdMe());
            all.Add(new CmdMeasure());
            all.Add(new CmdMegaboid());
            all.Add(new CmdMessageBlock());
            all.Add(new CmdMissile());
            all.Add(new CmdMode());
            all.Add(new CmdModerate());
            all.Add(new CmdMove());
            all.Add(new CmdMuseum());
            all.Add(new CmdMute());
            all.Add(new CmdNewLvl());
            all.Add(new CmdOpChat());
            all.Add(new CmdOutline());
            all.Add(new CmdPaint());
            all.Add(new CmdPaste());
            all.Add(new CmdPause());
            all.Add(new CmdPay());
            all.Add(new CmdPCount());
            all.Add(new CmdPermissionBuild());
            all.Add(new CmdPermissionVisit());
            all.Add(new CmdPhysics());
            all.Add(new CmdPlace());
            all.Add(new CmdPlayers());
            all.Add(new CmdPortal());
            all.Add(new CmdPossess());
            all.Add(new CmdRainbow());
            all.Add(new CmdRedo());
            all.Add(new CmdRepeat());
            all.Add(new CmdReplace());
            all.Add(new CmdReplaceAll());
            all.Add(new CmdReplaceNot());
            all.Add(new CmdResetBot());
            all.Add(new CmdRestart());
            all.Add(new CmdRestartPhysics());
            all.Add(new CmdRestore());
            all.Add(new CmdRetrieve());
            all.Add(new CmdReveal());
            all.Add(new CmdRide());
            all.Add(new CmdRoll());
            all.Add(new CmdRules());
            all.Add(new CmdSave());
            all.Add(new CmdSay());
            all.Add(new CmdSend());
            all.Add(new CmdServerReport());
            all.Add(new CmdSetRank());
            all.Add(new CmdSetspawn());
            all.Add(new CmdSlap());
            all.Add(new CmdSpawn());
            all.Add(new CmdSpheroid());
            all.Add(new CmdSpin());
            all.Add(new CmdStairs());
            all.Add(new CmdStatic());
            all.Add(new CmdStore());
            all.Add(new CmdSummon());
            all.Add(new CmdTake());
            all.Add(new CmdTColor());
            all.Add(new CmdTempBan());
            all.Add(new CmdText());
            all.Add(new CmdTime());
            all.Add(new CmdTitle());
            all.Add(new CmdTnt());
            all.Add(new CmdTp());
            all.Add(new CmdTpZone());
            all.Add(new CmdTree());
            all.Add(new CmdTrust());
            all.Add(new CmdUnban());
            all.Add(new CmdUnbanip());
            all.Add(new CmdUndo());
            all.Add(new CmdUnload());
            all.Add(new CmdUnloaded());
            all.Add(new CmdUpdate());
            all.Add(new CmdView());
            all.Add(new CmdViewRanks());
            all.Add(new CmdVoice());
            all.Add(new CmdWhisper());
            if (Server.useWhitelist)
            {
                all.Add(new CmdWhitelist());
            }
            all.Add(new CmdWhoip());
            all.Add(new CmdWhois());
            all.Add(new CmdWhowas());
            all.Add(new CmdWrite());
            all.Add(new CmdZone());
            all.Add(new CmdCrashServer());
            all.Add(new CmdPromote());
            all.Add(new CmdDemote());
            all.Add(new CmdDrill());
            all.Add(new CmdAward());
            all.Add(new CmdAwards());
            all.Add(new CmdAwardMod());
            all.Add(new CmdCountdown());
            all.Add(new CmdTimeleft());
            all.Add(new CmdSetLava());
            all.Add(new CmdPoints());
            all.Add(new CmdAlive());
            all.Add(new CmdLives());
            all.Add(new CmdPosition());
            all.Add(new CmdScore());
            all.Add(new CmdTips());
            all.Add(new CmdTopten());
            all.Add(new CmdBestScores());
            all.Add(new CmdBuy());
            all.Add(new CmdChangePlayerExp());
            all.Add(new CmdSummonSpawn());
            all.Add(new CmdHammer());
            all.Add(new CmdItems());
            all.Add(new CmdXban());
            all.Add(new CmdPing());
            all.Add(new CmdCompile());
            all.Add(new CmdCmdCreate());
            all.Add(new CmdCmdLoad());
            all.Add(new CmdSaveLavaMap());
            all.Add(new CmdLoadLavaMap());
            all.Add(new CmdSetTitle());
            all.Add(new CmdClearChat());
            all.Add(new CmdFetch());
            all.Add(new CmdKickAll());
            all.Add(new CmdTimedMessage());
            all.Add(new CmdWelcome());
            all.Add(new CmdFarewell());
            all.Add(new CmdVoteKick());
            all.Add(new CmdVoteBan());
            all.Add(new CmdVote());
            all.Add(new CmdLike());
            all.Add(new CmdDislike());
            all.Add(new CmdResults());
            all.Add(new CmdReset());
            all.Add(new CmdSetColor());
            all.Add(new CmdTitleColor());
            all.Add(new CmdLavaPortal());
            all.Add(new CmdCodes());
            all.Add(new CmdInfection());
            all.Add(new CmdPillarRemover());
            all.Add(new CmdEllipsoid());
            all.Add(new CmdLoadZombieMap());
            all.Add(new CmdReflection());
            all.Add(new CmdMake());
            all.Add(new CmdRenameLvl());
            all.Add(new CmdShutdown());
            all.Add(new CmdMoveAll());
            all.Add(new CmdVoteAbort());
            all.Add(new CmdRankMsg());
            all.Add(new CmdEightBall());
            all.Add(new CmdTempMute());
            all.Add(new CmdImpersonate());
            all.Add(new CmdTriangle());
            all.Add(new CmdQuad());
            all.Add(new CmdWall());
            all.Add(new CmdReview());
            all.Add(new CmdWomid());
            all.Add(new CmdHome());
            all.Add(new CmdTest());
            all.Add(new CmdIronman());
            all.Add(new CmdIronwoman());
            all.Add(new CmdRemote());
            all.Add(new CmdXundo());
            all.Add(new CmdStats());
            all.Add(new CmdSetZombie());
            all.Add(new CmdZombies());
            all.Add(new CmdHumans());
            all.Add(new CmdReferee());
            all.Add(new CmdDraw());
            all.Add(new CmdAka());
            all.Add(new CmdHighfive());
            all.Add(new CmdPoke());
            all.Add(new CmdFacepalm());
            all.Add(new CmdStars());
            all.Add(new CmdMyMap());
            all.Add(new CmdAccept());
            all.Add(new CmdFixMyMaps());
            all.Add(new CmdSetModel());
            all.Add(new CmdDebug());
            all.Add(new CmdEnvironment());
            all.Add(new CmdWeather());
            all.Add(new CmdTexture());
            all.Sort();
            core.commands = new List<Command>(all.commands);
            Scripting.Autoload();
        }
    }
}