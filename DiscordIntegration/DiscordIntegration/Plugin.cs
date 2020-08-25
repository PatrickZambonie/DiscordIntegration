using Discord;
using IPA;
using System;
using IPALogger = IPA.Logging.Logger;
using UnityEngine;
using BS_Utils.Utilities;
using BeatSaberMarkupLanguage.Settings;
using DiscordIntegration.Localization;

namespace DiscordIntegration
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {

        // i wrote like the majority of this code at like 2am
        // not a good idea, excuse the slight messiness
        // i tried to clean it up a little bit but whateva

        //Discord RP
        public static Discord.Discord discord;
        public static ActivityManager activityManager;
        public static UserManager userManager;
        public static User currentUser;
        public static bool isDiscordPresent = true;

        internal static Discord.Activity currentActivity;
        internal static GameplayCoreSceneSetupData currentGameplayData;
        internal static GameObject gameObj;

        [Init]
        public void Init(IPALogger logger)
        {
            Logger.log = logger;
            PluginConfig.Init();
        }

        [OnStart]
        public void OnApplicationStart()
        {
            try //if discord isn't open, the plugin will crash. this ensures it wont
            {
                discord = new Discord.Discord(645381705821061171, (UInt64)Discord.CreateFlags.NoRequireDiscord);
            } catch (Discord.ResultException)
            {
                Logger.log.Critical("Discord not found!");
                isDiscordPresent = false;
            }

            if (isDiscordPresent)
            {
                activityManager = discord.GetActivityManager();
                userManager = discord.GetUserManager();

                discord.SetLogHook(Discord.LogLevel.Debug, (level, message) =>
                {
                    Logger.log.Critical("Log[" + level + "] " + message);
                });

                // Get Current User

                userManager.OnCurrentUserUpdate += () =>
                {
                    try
                    {
                        currentUser = userManager.GetCurrentUser();
                        Logger.log.Info("Discord user found: " + currentUser.Username);
                    }
                    catch (Discord.ResultException e)
                    {
                        Logger.log.Error("Discord user not found!");
                        Logger.log.Error(e);
                    }
                };

                LocalizationHandler.setLocale(PluginConfig.languageSelected);

                gameObj = new GameObject();
                gameObj.AddComponent<Callbacks>();
                gameObj.SetActive(true);

                addSubscriptions();


                //Setup settings menu
                BSMLSettings.instance.AddSettingsMenu("DiscordIntegration", "DiscordIntegration.UI.SettingsUI.bsml", UI.SettingsUI.instance);
                Logger.log.Info("Added settings menu");
                //UI.SettingsUI.instance.updateLocale();
            }
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            removeSubscriptions();
        }

        public void addSubscriptions()
        {
            BSEvents.menuSceneActive += () => checkAndRestartGameObj();
            BSEvents.gameSceneActive += () => checkAndRestartGameObj();
            BSEvents.menuSceneActive += () => UpdateHandler.updateForMenu(); //set RP to Menu
            BSEvents.gameSceneActive += () => UpdateHandler.prepareUpdateForGame(); //call to grab bm name
            BeatmapData.foundBeatmapData += (gameplaySetup) => UpdateHandler.updateForGame(gameplaySetup); //set RP to Game
            BSEvents.songPaused += () => GamePauseHandler.gamePaused(); // set RP to paused and start a timer to keep track
            BSEvents.songUnpaused += () => GamePauseHandler.gameResumed(); // set RP back to normal and also adjust time remaining
        }

        public void removeSubscriptions()
        {
            BSEvents.menuSceneActive -= () => checkAndRestartGameObj();
            BSEvents.gameSceneActive -= () => checkAndRestartGameObj();
            BSEvents.menuSceneActive -= () => UpdateHandler.updateForMenu();
            BSEvents.gameSceneActive -= () => UpdateHandler.prepareUpdateForGame();
            BeatmapData.foundBeatmapData -= (gameplaySetup) => UpdateHandler.updateForGame(gameplaySetup);
            BSEvents.songPaused -= () => GamePauseHandler.gamePaused();
            BSEvents.songUnpaused -= () => GamePauseHandler.gameResumed();
        }

        public void checkAndRestartGameObj()
        {
            try
            {
                if (!gameObj.activeSelf)
                {
                    Logger.log.Critical("GameObject no longer exists, restarting...");
                    gameObj = new GameObject();
                    gameObj.AddComponent<Callbacks>();
                    gameObj.SetActive(true);
                }
            } catch(Exception e)
            {
                Logger.log.Critical("GameObject no longer exists, restarting...");
                gameObj = new GameObject();
                gameObj.AddComponent<Callbacks>();
                gameObj.SetActive(true);
            }
        }

        //Send an activity to update Discord
        public static void updateDiscord(Discord.Activity activity)
        {
            Logger.log.Info("Attempting to update Discord RP...");
            currentActivity = activity;

            if (!PluginConfig.pluginEnabled)
            {
                activity = new Discord.Activity { };
            }

            activity.Assets.LargeImage = "main";

            activityManager.UpdateActivity(activity, (result) =>
            {
                if (result == Discord.Result.Ok)
                {
                    Logger.log.Info("Discord RP updated");
                }
                else
                {
                    Logger.log.Notice("Failed to update Discord RP!");
                }
            });

            discord.RunCallbacks();
        }
    }
}
