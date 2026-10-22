using System;
using System.Collections.Generic;
using System.Linq;
using nadena.dev.modular_avatar.core;
using nadena.dev.modular_avatar.core.editor;
using nadena.dev.ndmf.runtime;
using UnityEditor;
using UnityEngine;
using static KOZ39.SetupHairForMA.SetupHairForMALocalization;

namespace KOZ39.SetupHairForMA
{
    internal static class SetupHairForMAMenu
    {
        private const string MenuName = "Setup Hair or Outfit";
        private const string GameObjectMenuPath = "GameObject/Modular Avatar/" + MenuName;

        // Same as Setup Outfit. Equal priorities sort by name, which puts this directly above it.
        private const int MenuPriority = -1000;

        // Not 1, so float error in an unscaled head doesn't turn on Match Scale.
        private const float MatchScaleThreshold = 0.99f;

        private static readonly string[] _headBoneNames = { "Head", "Hair" };

        [MenuItem(GameObjectMenuPath, true, MenuPriority)]
        private static bool ValidateSetup() => GetTargets().Length > 0;

        [MenuItem(GameObjectMenuPath, false, MenuPriority)]
        private static void Setup(MenuCommand command)
        {
            var context = command.context as GameObject;

            // With multiple objects selected, Unity invokes the menu once per object.
            if (context != null && context != Selection.activeGameObject)
            {
                return;
            }

            var selected = GetTargets();

            // An avatar selected together with something inside it is skipped without a message.
            var targets = selected
                .Where(target => !IsAvatarOfOtherTarget(target, selected))
                .ToArray();

            if (targets.Length == 0)
            {
                return;
            }

            Undo.IncrementCurrentGroup();
            var undoGroup = Undo.GetCurrentGroup();

            var messages = new List<string>();

            foreach (var target in targets)
            {
                var message = SetupTarget(target);

                if (message == null)
                {
                    continue;
                }

                Debug.LogWarning($"{SetupHairForMAPackageInfo.LogPrefix} {message}", target);
                messages.Add(message);
            }

            // Set after the loop because Setup Outfit renames the group.
            Undo.SetCurrentGroupName(MenuName);
            Undo.CollapseUndoOperations(undoGroup);

            if (messages.Count > 0)
            {
                EditorUtility.DisplayDialog(
                    SetupHairForMAPackageInfo.DisplayName,
                    string.Join("\n\n", messages),
                    Text("buttons.OK")
                );
            }
        }

        private static GameObject[] GetTargets() =>
            Selection
                .gameObjects.Where(target => !PrefabUtility.IsPartOfPrefabAsset(target))
                .ToArray();

        private static bool IsAvatarOfOtherTarget(GameObject target, GameObject[] selected) =>
            RuntimeUtil.IsAvatarRoot(target.transform)
            && selected.Any(other =>
                other != target && other.transform.IsChildOf(target.transform)
            );

        private static string SetupTarget(GameObject target)
        {
            var (avatar, avatarHead) = FindAvatar(target);

            if (avatar == null)
            {
                return Text("errors.NoAvatar", target.name);
            }

            if (avatar.transform == target.transform)
            {
                return Text("errors.IsAvatar", target.name);
            }

            if (!IsOutfit(target))
            {
                return SetupHair(target, avatar, avatarHead);
            }

            // Setup Outfit shows its own error window, so there is no message to collect.
            SetupOutfit.SetupOutfitUI(target);

            return null;
        }

        private static string SetupHair(GameObject hair, Animator avatar, Transform avatarHead)
        {
            if (hair.GetComponentInChildren<ModularAvatarMergeArmature>(true) != null)
            {
                return Text("errors.MergeArmatureExists", hair.name);
            }

            var armature = Children(hair.transform)
                .FirstOrDefault(child => NameContains(child, "Armature"));

            if (armature == null)
            {
                return Text("errors.NoArmature", hair.name);
            }

            var bones = Children(armature);
            var hairHead = bones.FirstOrDefault(bone =>
                _headBoneNames.Contains(bone.name, StringComparer.OrdinalIgnoreCase)
            );

            if (hairHead == null)
            {
                return Text("errors.NoHeadBone", hair.name, armature.name);
            }

            var otherProxy = hair.GetComponentsInChildren<ModularAvatarBoneProxy>(true)
                .FirstOrDefault(proxy => proxy.transform != hairHead);

            if (otherProxy != null)
            {
                return Text("errors.BoneProxyExists", hair.name, otherProxy.name);
            }

            // Against the avatar root, not the hair: a hair the user scaled to fit keeps that scale, and once Match
            // Scale has scaled the bone it already matches the head, which would turn Match Scale off on a rerun.
            AttachToHead(
                hairHead,
                avatarHead.AverageLossyScale() / avatar.transform.AverageLossyScale()
                    < MatchScaleThreshold
            );

            if (SetupHairForMASettings.AutoAddColliders)
            {
                AddColliders(hair, avatar);
            }

            var unattachedNames = bones
                .Where(bone => bone != hairHead)
                .Select(bone => bone.name)
                .ToArray();

            return unattachedNames.Length == 0
                ? null
                : Text(
                    "warnings.NotFollowingHead",
                    hair.name,
                    string.Join(Text("formats.ListSeparator"), unattachedNames)
                );
        }

        private static (Animator Avatar, Transform Head) FindAvatar(GameObject hair)
        {
            var avatarRoot = RuntimeUtil.FindAvatarInParents(hair.transform);

            if (avatarRoot == null)
            {
                return default;
            }

            var animator = avatarRoot.GetComponent<Animator>();
            var head =
                animator != null && animator.isHuman
                    ? animator.GetBoneTransform(HumanBodyBones.Head)
                    : null;

            return head != null ? (animator, head) : default;
        }

        // Looks for Hips one level below each child of the root, as Setup Outfit does, so an armature with another
        // name still counts.
        private static bool IsOutfit(GameObject target) =>
            target.TryGetComponent<Animator>(out var animator) && animator.isHuman
            || Children(target.transform)
                .SelectMany(Children)
                .Any(bone => NameContains(bone, "Hips"));

        private static void AttachToHead(Transform hairHead, bool matchScale)
        {
            // Not on the armature: Match Scale scales around the proxy's origin, and the armature's origin sits at the
            // avatar's feet, so the whole hair would sink.
            var proxy = hairHead.GetComponent<ModularAvatarBoneProxy>();

            if (proxy == null)
            {
                proxy = Undo.AddComponent<ModularAvatarBoneProxy>(hairHead.gameObject);
            }
            else
            {
                Undo.RecordObject(proxy, MenuName);
            }

            // Other modes snap the bone onto the avatar's head bone, which shifts hair that is already in place.
            proxy.boneReference = HumanBodyBones.Head;
            proxy.subPath = null;
            proxy.attachmentMode = BoneProxyAttachmentMode.AsChildKeepWorldPose;
            proxy.matchScale = matchScale;

            PrefabUtility.RecordPrefabInstancePropertyModifications(proxy);
        }

        private static void AddColliders(GameObject hair, Animator avatar)
        {
            var (physBones, colliders) = HairColliders.Assign(hair, avatar, MenuName);

            if (colliders == 0)
            {
                return;
            }

            var message = Text("messages.CollidersAdded", hair.name, physBones, colliders);
            Debug.Log($"{SetupHairForMAPackageInfo.LogPrefix} {message}", hair);
        }

        private static Transform[] Children(Transform parent) => parent.Cast<Transform>().ToArray();

        private static bool NameContains(Transform transform, string value) =>
            transform.name.Contains(value, StringComparison.OrdinalIgnoreCase);
    }
}
