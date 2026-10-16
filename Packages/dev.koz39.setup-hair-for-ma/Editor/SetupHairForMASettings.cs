using UnityEditor;

namespace KOZ39.SetupHairForMA
{
    internal static class SetupHairForMASettings
    {
        private const string AutoAddCollidersMenuPath =
            "Tools/"
            + SetupHairForMAPackageInfo.DisplayName
            + "/Auto-Add PhysBone Colliders (Experimental)";

        private const string AutoAddCollidersKey =
            SetupHairForMAPackageInfo.PreferencesPrefix + "AutoAddColliders";

        internal static bool AutoAddColliders
        {
            get => EditorPrefs.GetBool(AutoAddCollidersKey, true);
            private set => EditorPrefs.SetBool(AutoAddCollidersKey, value);
        }

        [MenuItem(AutoAddCollidersMenuPath, true)]
        private static bool ValidateToggleAutoAddColliders()
        {
            Menu.SetChecked(AutoAddCollidersMenuPath, AutoAddColliders);

            return true;
        }

        [MenuItem(AutoAddCollidersMenuPath)]
        private static void ToggleAutoAddColliders() => AutoAddColliders = !AutoAddColliders;
    }
}
