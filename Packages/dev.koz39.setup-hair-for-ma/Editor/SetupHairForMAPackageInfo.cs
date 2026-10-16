using UnityEditor.PackageManager;

namespace KOZ39.SetupHairForMA
{
    internal static class SetupHairForMAPackageInfo
    {
        internal const string Name = "dev.koz39.setup-hair-for-ma";
        internal const string DisplayName = "Setup Hair for MA";
        internal const string LogPrefix = "[" + DisplayName + "]";
        internal const string PreferencesPrefix = "KOZ39.SetupHairForMA.";

        private static readonly PackageInfo _info = PackageInfo.FindForAssembly(
            typeof(SetupHairForMAPackageInfo).Assembly
        );

        internal static string ResolvedPath => _info?.resolvedPath ?? "Packages/" + Name;
    }
}
