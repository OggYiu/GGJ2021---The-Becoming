using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ITimeChildEventReceiver))]
public class TimeBridge : MonoBehaviour
{
    [SerializeField] private TimeChild tc;

    public ITimeChildEventReceiver GetReceiver()
    {
        return GetComponent<ITimeChildEventReceiver>();
    }

    public ITimeChild GetTimeChild()
    {
        return tc;
    }

    private void OnEnable()
    {
        tc.RegisterEvent(GetReceiver());
    }

    private void OnDisable()
    {
        tc.UnRegisterEvent(GetReceiver());
    }
}
