using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour
{
    [SerializeField] Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float time = 0.5f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            time -= 0.1f;
            //animator.dur
            animator.SetFloat("MotionTime", time / clips[0].length);

                Debug.Log(clips[0].length);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            time += 0.1f;
            animator.SetFloat("MotionTime", time / clips[0].length);
        }
    }
}
