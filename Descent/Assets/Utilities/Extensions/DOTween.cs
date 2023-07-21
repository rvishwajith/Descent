using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Utilities.Extensions
{
    public static class DOTween
    {
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveInTargetLocalSpace(
            this Transform transform,
            Transform target,
            Vector3 targetLocalEndPosition,
            float duration = 2f)
        {
            var t = DG.Tweening.DOTween.To(
                () => transform.position - target.transform.position, // Value getter
                x => transform.position = x + target.transform.position, // Value setter
                targetLocalEndPosition,
                duration);
            t.SetTarget(transform);
            return t;
        }
    }
}