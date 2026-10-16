using UnityEngine;

namespace KOZ39.SetupHairForMA
{
    internal static class TransformExtensions
    {
        internal static float AverageLossyScale(this Transform transform)
        {
            var scale = transform.lossyScale;

            return (scale.x + scale.y + scale.z) / 3f;
        }
    }
}
