using System;
using Unity.Collections;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] private AgentEventGroup eventGroup;

    public Action<float> OnTimeChange;

    public float Time
    {
        get { return _time; }
        set
        {
            if (!Helper.equal(value, _time))
            {
                OnTimeChange?.Invoke(value);
            }
        }
    }

    public AgentEventScript CurrentEventScript() => GetEventScript(Time);

    public AgentEventScript GetEventScript(float time)
    {
        if (eventGroup.scripts.Length == 0) return null;

        float t = time;
        for(int i = 0; i < eventGroup.scripts.Length; ++i)
        {
            t -= eventGroup.scripts[i].duration;
            if(t < 0) { return eventGroup.scripts[i]; }
        }

        return eventGroup.scripts[eventGroup.scripts.Length - 1];
    }

    private float _time = 0;
}
