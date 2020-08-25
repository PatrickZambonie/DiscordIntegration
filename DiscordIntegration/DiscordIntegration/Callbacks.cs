using System.Collections;
using UnityEngine;

namespace DiscordIntegration
{
    class Callbacks : MonoBehaviour
    {
        public void Awake()
        {
            Logger.log.Info("Discord callback runner awoken");
            DontDestroyOnLoad(Plugin.gameObj);
            StartCoroutine(customUpdate());
        }

        public IEnumerator customUpdate() //WMR breaks monobehaviour's update function for some reason so this should hopefully work
        {
            Logger.log.Info("Discord callback runner running");
            while (true)
            {
                Plugin.discord.RunCallbacks();
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}
