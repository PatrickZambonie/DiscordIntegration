using DiscordIntegration.Localization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DiscordIntegration
{
    class UpdateHandler
    {
        public static void updateForMenu()
        {
            Discord.Activity activity = new Discord.Activity { };
            activity.Details = LocalizationHandler.locale.menu;

            Plugin.updateDiscord(activity);
        }

        public static void prepareUpdateForGame()
        {
            //Plugin.gameObj = new GameObject("DiscordIntegration.BeatmapData"); // fix later
            Plugin.gameObj.AddComponent<BeatmapData>(); // call to fetch beatmap info
        }

        public static void updateForGame(GameplayCoreSceneSetupData gameplaySetup)
        {
            Discord.Activity activity = new Discord.Activity { }; // setup new activity

            //config check
            if (PluginConfig.usePlayButtonEmoji)
            {
                activity.Details = "▶️ " + gameplaySetup.difficultyBeatmap.level.songName;
            }
            else
            {
                activity.Details = LocalizationHandler.locale.playing + gameplaySetup.difficultyBeatmap.level.songName;
            }

            if (PluginConfig.showDifficulty)
            {
                activity.State = LocalizationHandler.locale.difficulty + gameplaySetup.difficultyBeatmap.difficulty.Name();
            }
            else
            {
                activity.State = LocalizationHandler.locale.by + gameplaySetup.difficultyBeatmap.level.songAuthorName;
            }

            activity.Assets.LargeText = LocalizationHandler.locale.mappedBy + gameplaySetup.difficultyBeatmap.level.levelAuthorName;

            //time setup
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;
            float adjustedSpeedMul = 1 - (gameplaySetup.gameplayModifiers.songSpeedMul - 1);

            switch (PluginConfig.showTimeRemaining)
            {
                case true:
                    int timespan = (int)Math.Round(gameplaySetup.difficultyBeatmap.level.beatmapLevelData.audioClip.length * adjustedSpeedMul); // .songDuration always returns 0??? dunno not gonna question it, and this works 
                    GamePauseHandler.timeRemaining = secondsSinceEpoch + timespan;
                    activity.Timestamps.End = GamePauseHandler.timeRemaining;
                    break;
                case false:
                    activity.Timestamps.Start = (long)t.TotalSeconds;
                    break;
            }

            //create modifers text
            List<String> modifierList = new List<String>();
            if (gameplaySetup.gameplayModifiers.noFail) modifierList.Add("NF");
            if (gameplaySetup.gameplayModifiers.ghostNotes) modifierList.Add("GN");
            if (gameplaySetup.gameplayModifiers.disappearingArrows) modifierList.Add("DA");
            if (adjustedSpeedMul < 1) modifierList.Add("FS");
            if (adjustedSpeedMul > 1) modifierList.Add("SS");

            String modifierString = "";
            for (int i = 0; i < modifierList.Count; i++)
            {
                modifierString += modifierList[i];
                if (i == modifierList.Count-2)
                {
                    modifierString += " " + LocalizationHandler.locale.and + " ";
                } else if (i == modifierList.Count-1)
                {
                    modifierString += " " + LocalizationHandler.locale.enabled;
                } else if (modifierList.Count > 1)
                {
                    modifierString += ", ";
                }
            }

            //speed setup

            if (PluginConfig.showGameplayModifiers)
            {
                if (modifierList.Count == 1)
                {
                    switch (modifierList[0])
                    {
                        case "NF":
                            activity.Assets.SmallImage = "nofail";
                            break;
                        case "GN":
                            activity.Assets.SmallImage = "ghostnotes";
                            break;
                        case "DA":
                            activity.Assets.SmallImage = "disappearingarrows";
                            break;
                        case "FS":
                            activity.Assets.SmallImage = "fastsong";
                            break;
                        case "SS":
                            activity.Assets.SmallImage = "slowsong";
                            break;
                    }
                }
                else if (modifierList.Count > 1)
                {
                    activity.Assets.SmallImage = "multiplemods";
                }
                activity.Assets.SmallText = modifierString;
            }

            /*if (adjustedSpeedMul < 1 && PluginConfig.showGameplayModifiers) //faster song
            {
                activity.Assets.SmallImage = "fastsong";
                activity.Assets.SmallText = modifierString;
            }
            else if (adjustedSpeedMul > 1 && PluginConfig.showGameplayModifiers) //slower song
            {
                activity.Assets.SmallImage = "slowsong";
                activity.Assets.SmallText = LocalizationHandler.locale.slowerSong;
            }*/

            //practice mode check
            if (gameplaySetup.practiceSettings != null && PluginConfig.showPracticeMode)
            {
                activity.Assets.SmallImage = "practice";
                activity.Assets.SmallText = LocalizationHandler.locale.practiceMode;

                //switch timer to elasped instead of remaining
                activity.Timestamps.End = 0;
                TimeSpan timeNow = DateTime.UtcNow - new DateTime(1970, 1, 1);
                activity.Timestamps.Start = (long)t.TotalSeconds;
            }

            Plugin.updateDiscord(activity);
        }
    }
}
