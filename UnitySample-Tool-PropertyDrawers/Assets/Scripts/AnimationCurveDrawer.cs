using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationCurveDrawer
{
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private Vector2 xRange;
    [SerializeField] private Vector2 yRange;

    public float Evaluate(float time)
    {
        if (xRange != null && yRange != null)
            return animationCurve.Evaluate(time / xRange.y) * yRange.y;
        return -1;
    }
}