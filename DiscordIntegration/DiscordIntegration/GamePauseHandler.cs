using Discord;
using DiscordIntegration.Localization;
using System;
using System.Diagnostics;
using UnityEngine.SceneManagement;

namespace DiscordIntegration
{
    class GamePauseHandler
    {
        private static Stopwatch pauseTimer = new Stopwatch();
        internal static int timeRemaining; // used to temporarily store the epoch time of which the song ends until song is unpaused

        public static void gamePaused()
        {
            if (SceneManager.GetActiveScene().name == "GameCore")
            {
                pauseTimer.Reset();
                pauseTimer.Start();
                Activity activity = Plugin.currentActivity;
                if (Plugin.currentGameplayData.practiceSettings == null && PluginConfig.showTimeRemaining) activity.Timestamps.End = 0;
                activity.Details = "[" + LocalizationHandler.locale.paused + "] " + Plugin.currentGameplayData.difficultyBeatmap.level.songName;
                Plugin.updateDiscord(activity);
            }
        }

        public static void gameResumed()
        {
            if (pauseTimer.IsRunning)
            {
                pauseTimer.Stop();
                Activity activity = Plugin.currentActivity;
                activity.Details = PluginConfig.getPreferedPlayingIndicator() + Plugin.currentGameplayData.difficultyBeatmap.level.songName;
                timeRemaining += (int)(Math.Round(pauseTimer.Elapsed.TotalMilliseconds / 1000)); // did this in hopes of being more accurate possibly (mecha senku here it works pretty fuckn well)
                if (Plugin.currentGameplayData.practiceSettings == null && PluginConfig.showTimeRemaining) activity.Timestamps.End = timeRemaining;
                Plugin.updateDiscord(activity);
            }
        }
    }
}
