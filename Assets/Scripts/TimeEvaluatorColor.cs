using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ITimeChild))]
public class TimeEvaluatorColor : MonoBehaviour, ITimeEvaluator
{
    [SerializeField] private Color minColor;
    [SerializeField] private Color maxColor;
    [SerializeField] private MeshRenderer meshRenderer;

    public void Evaluate(float percentage)
    {
        meshRenderer.material.color = Color.Lerp(minColor, maxColor, percentage);
    }
}
