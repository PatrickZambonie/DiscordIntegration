using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordIntegration.Localization
{
    class LocalizationHandler
    {
        internal static Locale locale;
        internal static Assembly assembly = Assembly.GetExecutingAssembly();

        internal static Dictionary<String, String> languageLookup = new Dictionary<String, String>()
            {
                {"English", "en-US"},
                {"Espanol / Spanish", "es-MX"}, // AWildGamer#6283
                {"Elliniki / Greek", "el-GR"}, // AC Nektarios#1241
                {"Francais / French", "fr-FR"}, // Hiremu#0476
                {"Svenska / Swedish", "sv-SE"}, // Simme63#4202
                {"Portugues / Portuguese", "pt-BR"}, // <usr>8x13b</usr>#8988
                {"Polski / Polish", "pl-PL"}, // done by me, there are probably errors
                {"Deutsche / German", "de-DE"}, // Syskoh#1337
                {"Nederlands / Dutch", "nl-NL"}, // ThiJNmEnS#8502
                {"Norsk / Norweigian", "nn-NO"}, // Jaiden#3410
                {"Čeština / Czech", "cz-CZ"}, // samiiikxd#0001
                {"Slovenský / Slovak", "sk-SK"}, // samiiikxd#0001
                {"Magyar Nyelv/ Hungarian", "hu-HU"}, // Yassan#0001
                {"Suomalainen / Finnish", "fi-FI"}, // AaltopahWi#7469
                {"中文 / Chinese", "zh-CN"}, // daveseo#6071
                {"Dansk / Danish", "da-DK"}, // IamZync#2360 twitch.tv/iamzync
                {"Italiano / Italian", "it-IT"} // Lorenzo3421#3173
            };

        public static void setLocale(String localeKeyword)
        {
            using (Stream stream = assembly.GetManifestResourceStream(@"DiscordIntegration.Localization." + localeKeyword + ".json"))
            using (StreamReader file = new StreamReader(stream))
            {
                JsonSerializer serializer = new JsonSerializer();
                locale = (Locale)serializer.Deserialize(file, typeof(Locale));
            }
        }

        public static String getLocalizedString(String textID)
        {
            return "";
        }

        public static String matchFullToLocale(String settingName)
        {
            return languageLookup[settingName];
        }

        public static String matchLocaleToFull(String localeName)
        {
            return languageLookup.FirstOrDefault(x => x.Value == localeName).Key;
        }
    }
}
