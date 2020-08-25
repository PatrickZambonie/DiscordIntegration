using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using DiscordIntegration.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace DiscordIntegration.UI
{
    public class SettingsUI : PersistentSingleton<SettingsUI>
    {
        [UIValue("languageList")]
        private List<object> languageList = new object[] {"English", "Čeština / Czech", "Dansk / Danish", "Deutsche / German", "Espanol / Spanish", "Elliniki / Greek", "Francais / French", "Italiano / Italian", "Magyar Nyelv/ Hungarian", "Nederlands / Dutch", "Norsk / Norweigian", "Polski / Polish", "Portugues / Portuguese", "Slovenský / Slovak", "Suomalainen / Finnish", "Svenska / Swedish", "中文 / Chinese" }.ToList();

        [UIValue("pluginEnabled")]
        public bool pluginEnabled
        {
            get => PluginConfig.pluginEnabled;
            set
            {
                PluginConfig.pluginEnabled = value;
            }
        }

        [UIValue("usePlayButtonEmoji")]
        public bool usePlayButtonEmoji
        {
            get => PluginConfig.usePlayButtonEmoji;
            set
            {
                PluginConfig.usePlayButtonEmoji = value;
            }
        }

        [UIValue("showGameplayModifiers")]
        public bool showGameplayModifiers
        {
            get => PluginConfig.showGameplayModifiers;
            set
            {
                PluginConfig.showGameplayModifiers = value;
            }
        }

        [UIValue("showPracticeMode")]
        public bool showPracticeMode
        {
            get => PluginConfig.showPracticeMode;
            set
            {
                PluginConfig.showPracticeMode = value;
            }
        }

        [UIValue("showTimeRemaining")]
        public bool showTimeRemaining
        {
            get => PluginConfig.showTimeRemaining;
            set
            {
                PluginConfig.showTimeRemaining = value;
            }
        }

        [UIValue("showDifficulty")]
        public bool showDifficulty
        {
            get => PluginConfig.showDifficulty;
            set
            {
                PluginConfig.showDifficulty = value;
            }
        }

        [UIValue("languageSelected")]
        public String languageSelected
        {
            get
            {
                updateSettingsLocale();
                return LocalizationHandler.matchLocaleToFull(PluginConfig.languageSelected);
            }
            set
            {
                PluginConfig.languageSelected = LocalizationHandler.matchFullToLocale(value);
                LocalizationHandler.setLocale(LocalizationHandler.matchFullToLocale(value));
                updateSettingsLocale();
            }
        }

        //localization logic

        [UIComponent("pluginEnabledText")]
        private TextMeshProUGUI pluginEnabledText;

        [UIComponent("showGameplayModifiersText")]
        private TextMeshProUGUI showGameplayModifiersText;

        [UIComponent("useEmojiText")]
        private TextMeshProUGUI useEmojiText;

        [UIComponent("showPracticeModeText")]
        private TextMeshProUGUI showPracticeModeText;

        [UIComponent("showTimeRemainingText")]
        private TextMeshProUGUI showTimeRemainingText;

        [UIComponent("showDifficultyText")]
        private TextMeshProUGUI showDifficultyText;

        [UIComponent("languageSelector")]
        private TextMeshProUGUI languageSelectorText;

        public void updateSettingsLocale()
        {
            pluginEnabledText.text = LocalizationHandler.locale.pluginEnabled;

            showGameplayModifiersText.text = LocalizationHandler.locale.showGameplayModifiers;

            useEmojiText.text = LocalizationHandler.locale.usePlayButtonEmoji;

            showPracticeModeText.text = LocalizationHandler.locale.showPracticeMode;
            
            showTimeRemainingText.text = LocalizationHandler.locale.showTimeRemaining;

            showDifficultyText.text = LocalizationHandler.locale.showDifficulty;

            languageSelectorText.text = LocalizationHandler.locale.languageSelector;
        }


    }
}
