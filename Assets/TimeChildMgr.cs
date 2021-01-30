using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeChildMgr : MonoBehaviour
{
    public static TimeChild lastClickedTC;
    public List<TimeChild> timeChildren = new List<TimeChild>();

    public void Add(TimeChild tc)
    {
        timeChildren.Add(tc);
    }

    public void Remove(TimeChild tc)
    {
        timeChildren.Remove(tc);
    }

    public void OnMouseDown(TimeChild tc)
    {
        if(lastClickedTC != null)
        {
            lastClickedTC.OnDeSelected();
        }

        tc.OnSelected();

        lastClickedTC = tc;
    }
}
