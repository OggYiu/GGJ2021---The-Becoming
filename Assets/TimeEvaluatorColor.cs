using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ITimeChild))]
public class TimeEvaluatorColor : MonoBehaviour, ITimeEvaluator
{
    [SerializeField] private Color minColor;
    [SerializeField] private Color maxColor;

    public void Evaluate(float percentage)
    {
        GetComponent<MeshRenderer>().material.color = Color.Lerp(minColor, maxColor, percentage);
    }
}
