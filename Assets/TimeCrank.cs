using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCrank : MonoBehaviour, ITimeCrank, ITimeChildEventReceiver
{
    virtual public void OnTimeChildChanged(float percentage)
    {
        throw new System.NotImplementedException();
    }

    virtual public void RegisterEvent(ITimeChild tc)
    {
        throw new System.NotImplementedException();
    }

    virtual public void UnRegisterEvent(ITimeChild tc)
    {
        throw new System.NotImplementedException();
    }
}
