using IPA.Utilities;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DiscordIntegration
{
    class BeatmapData : MonoBehaviour
    {

        //public static delegate void beatmapDataEventHandler();
        public static event Action<GameplayCoreSceneSetupData> foundBeatmapData;

        public void Awake()
        {
            StartCoroutine(GrabBeatmap());
        }

        IEnumerator GrabBeatmap()
        {
            /*yield return new WaitUntil(() => Resources.FindObjectsOfTypeAll<StandardLevelDetailViewController>().Any());
            StandardLevelDetailViewController levelDetailViewController = Resources.FindObjectsOfTypeAll<StandardLevelDetailViewController>().First();
            IBeatmapLevel beatmapLevel = ReflectionUtil.GetField<IBeatmapLevel, StandardLevelDetailViewController>(levelDetailViewController, "_beatmapLevel");
            Logger.log.Critical("ENV NAME: " + beatmapLevel.environmentInfo.environmentType);
            foundBeatmapData.Invoke(beatmapLevel);*/

            Logger.log.Info("Grabbing Beatmap...");
            yield return new WaitUntil(() => Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().Any());
            GameplayCoreSceneSetup gameplaySceneSetup = Resources.FindObjectsOfTypeAll<GameplayCoreSceneSetup>().First();
            GameplayCoreSceneSetupData gameplaySceneSetupData = ReflectionUtil.GetField<GameplayCoreSceneSetupData, GameplayCoreSceneSetup>(gameplaySceneSetup, "_sceneSetupData");
            Logger.log.Info("Beatmap grabbed");
            Plugin.currentGameplayData = gameplaySceneSetupData;
            foundBeatmapData.Invoke(gameplaySceneSetupData);
        }
    }
}
