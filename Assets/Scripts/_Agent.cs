using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Agent : MonoBehaviour
{
    [SerializeField] float currentDistance = 0;
    [SerializeField] float direction = 1f;
    [SerializeField] float maxDistance = 0;
    [SerializeField] float speed = 1f;
    [SerializeField] CubicHermiteSpline spline;

    void Start()
    {
        //spline = GetComponent<CubicHermiteSpline>();

        maxDistance = spline.TotalDistance();
    }

    // Update is called once per frame
    void Update()
    {
        currentDistance += Time.deltaTime * speed * direction;

        if (currentDistance < 0)
        {
            currentDistance = -currentDistance;
            direction = 1f;
        }

        if (currentDistance > maxDistance)
        {
            currentDistance = maxDistance - (currentDistance - maxDistance);
            direction = -1f;
        }

        Vector3 pos = spline.GetPosWithDistance(currentDistance);
        transform.position = pos;
    }
}
