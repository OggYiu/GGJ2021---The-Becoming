using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static bool equal(float v1, float v2)
    {
        return Mathf.Abs(v1 - v2) < float.Epsilon;
    }
}
