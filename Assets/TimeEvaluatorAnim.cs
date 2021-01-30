using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ITimeChild))]
public class TimeEvaluatorAnim : MonoBehaviour, ITimeEvaluator
{
    [SerializeField] private Animator animator;

    public void Evaluate(float percentage)
    {
        animator.SetFloat("MotionTime", percentage);
    }
}

