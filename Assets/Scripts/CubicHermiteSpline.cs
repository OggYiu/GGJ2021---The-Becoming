using Shapes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class CubicHermiteSpline : MonoBehaviour
{
    [SerializeField]
    List<Transform> points = new List<Transform>();

    [SerializeField]
    [Range(1, 18)]
    int resolution;

    [Header("Settings")]
    [SerializeField]
    bool isEvenSpeed = false;

    [SerializeField]
    float evenSpeed;

    [SerializeField]
    float tension;

    [Header("Debug")]

    [SerializeField]
    //[Range(0f, 1f)]
    float gizmos_tvalue;

    [SerializeField]
    //[Range(0f, 1f)]
    float gizmos_distance;

    public static Vector3 U(Vector3 startDir, float velocity) => Vector3.Normalize(startDir) * velocity;
    public static Vector3 V(Vector3 endDir, float velocity) => Vector3.Normalize(endDir) * velocity;

    public static Vector3 A(Vector3 start) => start;
    public static Vector3 B(Vector3 start, Vector3 dir, float velocity) => start + U(dir, velocity);
    public static Vector3 C(Vector3 end, Vector3 dir, float velocity) => end - V(dir, velocity);
    public static Vector3 D(Vector3 end) => end;

    public int PointCount() => points.Count;

    public float TotalDistance() => GetTotalDistance(points.ToArray(), resolution);

    public Vector3 GetPosWithDistance(float distance) => GetPosWithDistance(points.ToArray(), distance, resolution);

    public static void GetABCDUV(Transform p1, Transform p2, out Vector3 a, out Vector3 b, out Vector3 c, out Vector3 d, out Vector3 u, out Vector3 v)
    {
        GetABCD(p1, p2, out a, out b, out c, out d);
        GetUV(p1, p2, out u, out v);
    }

    public static void GetABCD(Transform p1, Transform p2, out Vector3 a, out Vector3 b, out Vector3 c, out Vector3 d)
    {
        a = A(p1.position);
        b = B(p1.position, p1.forward, p1.localScale.z);
        c = C(p2.position, p2.forward, p2.localScale.z);
        d = D(p2.position);
    }

    public static void GetUV(Transform p1, Transform p2, out Vector3 u, out Vector3 v)
    {
        u = U(p1.forward, p1.transform.localScale.z);
        v = V(p2.forward, p2.transform.localScale.z);
    }

    private void Update()
    {
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Count - 1; ++i)
        {
            var p0 = points[i];
            var p1 = points[i + 1];

            DrawGizmosCurve(p0, p1);
            DrawGizmosUV(p0, p1);
            DrawGizmosDot(p0, p1, 0);
        }

        if(points.Count >= 2)
        {
            var dotPos = GetPos(gizmos_tvalue);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(dotPos, 0.2f);
        }

        //{
        //    Vector3[] path = TransformToPoints(this.points, gizmos_resolution);
        //    //Gizmos.color = Color.yellow;
        //    //for (int i = 0; i < path.Length; ++i)
        //    //{
        //    //    Gizmos.DrawSphere(path[i], 0.1f);
        //    //}

        //    Gizmos.color = Color.red;
        //    for (int i = 0; i < path.Length - 1; ++i)
        //    {
        //        var p1 = path[i];
        //        var p2 = path[i + 1];
        //        Gizmos.DrawLine(p1, p2);
        //    }
        //}

        if(this.points.Count > 0)
        {
            // draw distance
            var pos = GetPosWithDistance(this.points.ToArray(), gizmos_distance, resolution);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pos, 0.2f);
        }
    }

    public static void DrawGizmosUV(Transform p1, Transform p2)
    {
        var s = p1.transform.position;
        GetUV(p1, p2, out Vector3 u, out Vector3 v);
        var e = p2.transform.position;

        Draw.Color = Color.red;
        Draw.Line(s, s + u);

        Draw.Color = Color.blue;
        Draw.Line(e, e + v);
    }

    public static Vector3 GetPosWithDistance(Transform[] path, float distance, int resolution)
    {
        if (path.Length == 0) throw new ArgumentException();
        if (path.Length < 2) return path[0].transform.position;

        float totalDistance = GetTotalDistance(path, resolution);
        if (path.Length < 2) throw new ArgumentException();
        if (distance < 0) return path[0].transform.position;
        if(distance > totalDistance) return path[path.Length - 1].transform.position;

        Vector3[] points = TransformToPoints(path, resolution);
        for(int i = 0; i < points.Length - 1; ++i)
        {
            var p1 = points[i];
            var p2 = points[i+1];
            float pointDistance = Vector3.Distance(p1, p2);
            if((distance - pointDistance) < 0)
            {
                return Vector3.Lerp(p1, p2, distance / pointDistance);
            }
            else
            {
                distance -= pointDistance;
            }
        }

        return path[0].transform.position;
    }

    public static float GetTotalDistance(Transform[] path, int resolution)
    {
        if (path.Length == 0) return 0;

        Vector3[] points = TransformToPoints(path, resolution);

        float sum = 0;
        for(int i = 0; i < points.Length - 1; ++i)
        {
            var p1 = points[i];
            var p2 = points[i+1];
            sum += Vector3.Distance(p1, p2);
        }

        return sum;
    }

    public static Vector3[] TransformToPoints(Transform[] path, int resolution)
    {
        if (path.Length == 0) return null;

        Vector3[] points = new Vector3[(resolution * (path.Length - 1)) + 1];

        for (int i = 0; i < path.Length - 1; ++i)
        {
            Transform p1 = path[i];
            Transform p2 = path[i+1];
            Vector3[] positions = TransformToPoints(p1, p2, resolution);
            positions.CopyTo(points, resolution * i);
        }

        points[points.Length - 1] = path[path.Length - 1].position;

        return points;
    }

    public static Vector3[] TransformToPoints(Transform p1, Transform p2, int resolution)
    {
        Vector3[] points = new Vector3[resolution + 1];

        for (int i = 0; i <= resolution; ++i)
        {
            points[i] = GetPos(p1, p2, (float)i / resolution);
        }

        return points;
    }

    public static void DrawGizmosCurve(Transform p1, Transform p2)
    {
        var points = TransformToPoints(p1, p2, 16);

        for(int i = 0; i < points.Length - 1; ++i)
        {
            var pos1 = points[i];
            var pos2 = points[i+1];

            Draw.Line(pos1, pos2, 0.05f, Color.grey);
        }
    }
    public static void DrawGizmosDot(Transform p1, Transform p2, float tvalue)
    {
        var pos = GetPos(p1, p2, tvalue);
        Draw.Sphere(pos, 0.1f, Color.white);
    }
    
    public static Vector3 GetPos(Transform point1, Transform point2, float t)
    {
        GetABCDUV(point1, point2, out Vector3 a, out Vector3 b, out Vector3 c, out Vector3 d, out Vector3 u, out Vector3 v);

        var p0 = a;
        var p1 = d;
        var m0 = 3f * (b - a);
        var m1 = 3f * (d - c);

        return 
            (2.0f * t * t * t - 3.0f * t * t + 1.0f) * p0
            + (t * t * t - 2.0f * t * t + t) * m0
            + (-2.0f * t * t * t + 3.0f * t * t) * p1
            + (t * t * t - t * t) * m1;
    }

    public Vector3 GetPos(float tvalue)
    {
        if (tvalue < 0 || tvalue > points.Count - 1) throw new IndexOutOfRangeException();

        int index01 = Mathf.FloorToInt(tvalue);
        int index02 = index01 + 1;

        var p0 = points[index01];
        var p1 = points[index02];

        return GetPos(p0, p1, tvalue - index01);
    }

    [ContextMenu("Cardinal Splines")]
    public void MakeCardinalSplines()
    {
        var firstTangent = points[0];
        firstTangent.localScale = Vector3.zero;

        for (int i = 0; i < points.Count - 2; ++i)
        {
            var p1 = points[i];
            var p2 = points[i+1];
            var p3 = points[i+2];

            var dir = Vector3.Normalize(p3.position - p1.position);
            var halfDistance = Vector3.Distance(p3.position, p1.position) / 2f;
            p2.forward = dir;
            p2.localScale = new Vector3(1f, 1f, isEvenSpeed ? evenSpeed : halfDistance * tension);
        }

        var lastTangent = points[points.Count - 1];
        lastTangent.localScale = Vector3.zero;
    }

}