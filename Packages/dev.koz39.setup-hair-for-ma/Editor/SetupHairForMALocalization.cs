using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using nadena.dev.ndmf.localization;
using Newtonsoft.Json.Linq;

namespace KOZ39.SetupHairForMA
{
    internal static class SetupHairForMALocalization
    {
        private const string Folder = "Editor/Localization";

        private static readonly Localizer _localizer = new("en-US", LoadLocales);

        internal static string Text(string key, params object[] arguments)
        {
            var text = _localizer.GetLocalizedString(key);

            return arguments.Length == 0
                ? text
                : string.Format(CultureInfo.CurrentCulture, text, arguments);
        }

        private static List<(string, Func<string, string>)> LoadLocales() =>
            Directory
                .EnumerateFiles(
                    Path.Combine(SetupHairForMAPackageInfo.ResolvedPath, Folder),
                    "*.jsonc"
                )
                .Select(path =>
                {
                    var strings = ReadStrings(path);

                    return (
                        Path.GetFileNameWithoutExtension(path),
                        (Func<string, string>)(key => strings.GetValueOrDefault(key))
                    );
                })
                .ToList();

        private static Dictionary<string, string> ReadStrings(string path) =>
            JObject
                .Parse(File.ReadAllText(path))
                .Properties()
                .SelectMany(group => ((JObject)group.Value).Properties())
                .ToDictionary(
                    property => property.Path,
                    property => property.Value.Value<string>()
                );
    }
}
