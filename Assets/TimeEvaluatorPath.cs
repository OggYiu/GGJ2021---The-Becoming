using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEvaluatorPath : MonoBehaviour, ITimeEvaluator
{
    [SerializeField] CubicHermiteSpline spline;

    public void Evaluate(float percentage)
    {
        transform.position = spline.GetPosWithDistance(spline.TotalDistance() * percentage);
    }
}
