using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VRC.Dynamics;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace KOZ39.SetupHairForMA
{
    internal static class HairColliders
    {
        private const float MinStrandLength = 0.05f;
        private const float CenterLineRatio = 0.15f;

        private static readonly HumanBodyBones[] _colliderParts =
        {
            HumanBodyBones.Hips,
            HumanBodyBones.Spine,
            HumanBodyBones.Chest,
            HumanBodyBones.UpperChest,
            HumanBodyBones.LeftUpperArm,
            HumanBodyBones.RightUpperArm,
        };

        private static readonly HumanBodyBones[] _centerParts =
        {
            HumanBodyBones.Hips,
            HumanBodyBones.Spine,
            HumanBodyBones.Chest,
            HumanBodyBones.UpperChest,
            HumanBodyBones.Neck,
            HumanBodyBones.Head,
        };

        private static readonly (HumanBodyBones Part, HumanBodyBones[] Ends)[] _bodyParts =
        {
            (HumanBodyBones.Hips, new[] { HumanBodyBones.Spine }),
            (HumanBodyBones.Spine, new[] { HumanBodyBones.Chest }),
            (HumanBodyBones.Chest, new[] { HumanBodyBones.UpperChest, HumanBodyBones.Neck }),
            (HumanBodyBones.UpperChest, new[] { HumanBodyBones.Neck }),
            (HumanBodyBones.Neck, new[] { HumanBodyBones.Head }),
            (HumanBodyBones.Head, Array.Empty<HumanBodyBones>()),
            (HumanBodyBones.LeftShoulder, new[] { HumanBodyBones.LeftUpperArm }),
            (HumanBodyBones.RightShoulder, new[] { HumanBodyBones.RightUpperArm }),
            (HumanBodyBones.LeftUpperArm, new[] { HumanBodyBones.LeftLowerArm }),
            (HumanBodyBones.RightUpperArm, new[] { HumanBodyBones.RightLowerArm }),
            (HumanBodyBones.LeftLowerArm, new[] { HumanBodyBones.LeftHand }),
            (HumanBodyBones.RightLowerArm, new[] { HumanBodyBones.RightHand }),
            (HumanBodyBones.LeftHand, Array.Empty<HumanBodyBones>()),
            (HumanBodyBones.RightHand, Array.Empty<HumanBodyBones>()),
            (HumanBodyBones.LeftUpperLeg, new[] { HumanBodyBones.LeftLowerLeg }),
            (HumanBodyBones.RightUpperLeg, new[] { HumanBodyBones.RightLowerLeg }),
            (HumanBodyBones.LeftLowerLeg, new[] { HumanBodyBones.LeftFoot }),
            (HumanBodyBones.RightLowerLeg, new[] { HumanBodyBones.RightFoot }),
            (HumanBodyBones.LeftFoot, Array.Empty<HumanBodyBones>()),
            (HumanBodyBones.RightFoot, Array.Empty<HumanBodyBones>()),
        };

        internal static (int PhysBones, int Colliders) Assign(
            GameObject hair,
            Animator avatar,
            string undoName
        )
        {
            var bodyColliders = FindBodyColliders(avatar, hair.transform);
            var physBones = 0;
            var colliders = 0;

            foreach (var physBone in hair.GetComponentsInChildren<VRCPhysBone>(true))
            {
                if (physBone.colliders.Any(collider => collider != null))
                {
                    continue;
                }

                var reachable = FindReachable(physBone, bodyColliders);

                if (reachable.Length == 0)
                {
                    continue;
                }

                Undo.RecordObject(physBone, undoName);
                physBone.colliders = new List<VRCPhysBoneColliderBase>(reachable);
                PrefabUtility.RecordPrefabInstancePropertyModifications(physBone);
                physBones++;
                colliders += reachable.Length;
            }

            return (physBones, colliders);
        }

        private static BodyCollider[] FindBodyColliders(Animator avatar, Transform hair)
        {
            var humanBones = Enum.GetValues(typeof(HumanBodyBones))
                .Cast<HumanBodyBones>()
                .Where(bone => bone != HumanBodyBones.LastBone)
                .Select(avatar.GetBoneTransform)
                .Where(bone => bone != null)
                .ToHashSet();
            var hips = avatar.GetBoneTransform(HumanBodyBones.Hips);
            var hipWidth =
                avatar.GetBoneTransform(HumanBodyBones.RightUpperLeg).position
                - avatar.GetBoneTransform(HumanBodyBones.LeftUpperLeg).position;
            var segments = FindBodySegments(avatar);
            var centerSegments = segments
                .Where(segment => _centerParts.Contains(segment.Part))
                .ToArray();

            bool IsOnCenterLine(Vector3 point) =>
                Mathf.Abs(Vector3.Dot(point - hips.position, hipWidth.normalized))
                < hipWidth.magnitude * CenterLineRatio;

            return avatar
                .GetComponentsInChildren<VRCPhysBoneCollider>(true)
                .Where(collider => !collider.transform.IsChildOf(hair))
                .Where(collider => !collider.insideBounds)
                .Where(collider => MovesWithBody(collider, humanBones, hips.parent))
                .Select(collider => new BodyCollider(collider))
                .Where(body =>
                    _colliderParts.Contains(
                        FindBodyPart(
                            IsOnCenterLine(body.Center) ? centerSegments : segments,
                            body.Center
                        )
                    )
                )
                .ToArray();
        }

        private static bool MovesWithBody(
            VRCPhysBoneCollider collider,
            HashSet<Transform> humanBones,
            Transform armature
        )
        {
            var target = collider.GetRootTransform();

            // With no Root Transform, or its own object as one, the collider follows whatever
            // its object sits under.
            var anchor = target == collider.transform ? target.parent : target;

            return humanBones.Contains(target) || humanBones.Contains(anchor) || anchor == armature;
        }

        private static (HumanBodyBones Part, Vector3 Start, Vector3 End)[] FindBodySegments(
            Animator avatar
        ) =>
            _bodyParts
                .Select(entry =>
                    (
                        entry.Part,
                        Start: avatar.GetBoneTransform(entry.Part),
                        End: entry
                            .Ends.Select(avatar.GetBoneTransform)
                            .FirstOrDefault(bone => bone != null)
                    )
                )
                .Where(segment => segment.Start != null)
                .Select(segment =>
                    (
                        segment.Part,
                        segment.Start.position,
                        segment.End != null ? segment.End.position : segment.Start.position
                    )
                )
                .ToArray();

        private static HumanBodyBones FindBodyPart(
            (HumanBodyBones Part, Vector3 Start, Vector3 End)[] segments,
            Vector3 point
        ) =>
            segments
                .OrderBy(segment =>
                    HandleUtility.DistancePointLine(point, segment.Start, segment.End)
                )
                .First()
                .Part;

        private static VRCPhysBoneCollider[] FindReachable(
            VRCPhysBone physBone,
            IEnumerable<BodyCollider> colliders
        )
        {
            var strands = Strands.Of(physBone);

            return colliders
                .Where(collider => CanReach(collider, strands))
                .Select(collider => collider.Collider)
                .ToArray();
        }

        internal static bool CanReach(BodyCollider collider, Strands strands)
        {
            var root = strands.Root;

            if (collider.IsPlane)
            {
                var toAnchor = Vector3.Distance(root, collider.Center);

                return strands.Bones.All(bone =>
                        Vector3.Dot(bone - collider.Center, collider.Axis) >= 0f
                    ) && strands.Branches.Any(branch => toAnchor <= branch.Reach);
            }

            if (
                strands.Bones.Any(bone =>
                    HandleUtility.DistancePointLine(bone, collider.Start, collider.End)
                    < collider.Radius
                )
            )
            {
                return false;
            }

            var nearest = HandleUtility.ProjectPointLine(root, collider.Start, collider.End);
            var distance = Vector3.Distance(root, nearest) - collider.Radius;

            return strands.Branches.Any(branch =>
                distance <= branch.Reach && Vector3.Dot(branch.Hang, nearest - root) >= 0f
            );
        }

        internal readonly struct BodyCollider
        {
            internal readonly VRCPhysBoneCollider Collider;
            internal readonly bool IsPlane;
            internal readonly Vector3 Center;
            internal readonly Vector3 Axis;
            internal readonly Vector3 Start;
            internal readonly Vector3 End;
            internal readonly float Radius;

            internal BodyCollider(VRCPhysBoneCollider collider)
            {
                var target = collider.GetRootTransform();
                var scale = target.AverageLossyScale();
                var center = target.TransformPoint(collider.position);
                var axis = target.rotation * collider.rotation * Vector3.up;

                // Capsule height includes both rounded ends.
                var halfLength =
                    collider.shapeType == VRCPhysBoneColliderBase.ShapeType.Capsule
                        ? Mathf.Max(0f, collider.height * 0.5f - collider.radius) * scale
                        : 0f;

                Collider = collider;
                IsPlane = collider.shapeType == VRCPhysBoneColliderBase.ShapeType.Plane;
                Center = center;
                Axis = axis;
                Start = center - axis * halfLength;
                End = center + axis * halfLength;
                Radius = collider.radius * scale;
            }
        }

        internal readonly struct Strands
        {
            internal readonly Vector3 Root;
            internal readonly Vector3[] Bones;
            internal readonly (float Reach, Vector3 Hang)[] Branches;

            internal Strands(Vector3 root, Vector3[] bones, (float Reach, Vector3 Hang)[] branches)
            {
                Root = root;
                Bones = bones;
                Branches = branches;
            }

            internal static Strands Of(VRCPhysBone physBone)
            {
                var root = physBone.GetRootTransform();
                var ignored = physBone.ignoreTransforms.Where(bone => bone != null).ToArray();

                Transform[] MovingChildren(Transform bone) =>
                    bone.Cast<Transform>().Where(child => !ignored.Any(child.IsChildOf)).ToArray();

                IEnumerable<Transform> Chain(Transform bone) =>
                    MovingChildren(bone).SelectMany(Chain).Prepend(bone);

                // PhysBone adds a bone at endpointPosition past the last bone of each chain.
                IEnumerable<Vector3> Endpoints(IEnumerable<Transform> chain) =>
                    physBone.endpointPosition == Vector3.zero
                        ? Enumerable.Empty<Vector3>()
                        : chain
                            .Where(bone => MovingChildren(bone).Length == 0)
                            .Select(bone => bone.TransformPoint(physBone.endpointPosition));

                var rootChildren = MovingChildren(root);

                var chains =
                    rootChildren.Length > 0 ? rootChildren.Select(child => Chain(child).ToArray())
                    : ignored.Any(root.IsChildOf) ? Array.Empty<Transform[]>()
                    : new[] { new[] { root } };
                var padding = physBone.radius * root.AverageLossyScale();
                var branches = chains
                    .Select(chain =>
                        (
                            Bones: chain,
                            ToTip: chain
                                .Select(bone => bone.position)
                                .Concat(Endpoints(chain))
                                .Select(point => point - root.position)
                                .OrderByDescending(toPoint => toPoint.magnitude)
                                .First()
                        )
                    )
                    .Where(branch => branch.ToTip.magnitude >= MinStrandLength)
                    .ToArray();

                return new Strands(
                    root.position,
                    branches
                        .SelectMany(branch => branch.Bones)
                        .Where(bone => bone != root)
                        .Select(bone => bone.position)
                        .ToArray(),
                    branches
                        .Select(branch =>
                            (branch.ToTip.magnitude + padding, branch.ToTip.normalized)
                        )
                        .ToArray()
                );
            }
        }
    }
}
