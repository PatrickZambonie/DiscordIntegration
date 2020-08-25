using BS_Utils.Utilities;
using DiscordIntegration.Localization;
using System;

namespace DiscordIntegration
{
    internal class PluginConfig
    {
        public bool RegenerateConfig = true;

        public static Config config;

        internal static void Init()
        {
            config = new Config("DiscordIntegration");
        }

        internal static bool pluginEnabled
        {
            get => config.GetBool("DiscordConfig", "pluginEnabled", true, true);
            set
            {
                config.SetBool("DiscordConfig", "pluginEnabled", value);
            }
        }

        internal static bool usePlayButtonEmoji
        {
            get => config.GetBool("DiscordConfig", "usePlayButtonEmoji", false, true);
            set
            {
                config.SetBool("DiscordConfig", "usePlayButtonEmoji", value);
            }
        }

        internal static bool showGameplayModifiers
        {
            get => config.GetBool("DiscordConfig", "showGameplayModifiers", true, true);
            set
            {
                config.SetBool("DiscordConfig", "showGameplayModifiers", value);
            }
        }

        internal static bool showPracticeMode
        {
            get => config.GetBool("DiscordConfig", "showPracticeMode", true, true);
            set
            {
                config.SetBool("DiscordConfig", "showPracticeMode", value);
            }
        }

        internal static bool showTimeRemaining
        {
            get => config.GetBool("DiscordConfig", "showTimeRemaining", true, true);
            set
            {
                config.SetBool("DiscordConfig", "showTimeRemaining", value);
            }
        }

        internal static String languageSelected
        {
            get => config.GetString("DiscordConfig", "languageSelected", "en-US", true);
            set
            {
                config.SetString("DiscordConfig", "languageSelected", value);
            }
        }

        internal static bool showDifficulty
        {
            get => config.GetBool("DiscordConfig", "showDifficulty", false, true);
            set
            {
                config.SetBool("DiscordConfig", "showDifficulty", value);
            }
        }

        internal static String getPreferedPlayingIndicator() // made this method for ease of use
        {
            if (PluginConfig.usePlayButtonEmoji)
            {
                return "▶️ ";
            }
            return LocalizationHandler.locale.playing;
        }
    }
}
