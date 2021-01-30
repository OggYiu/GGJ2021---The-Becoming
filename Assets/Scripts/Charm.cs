using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Charm : MonoBehaviour
{// Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            List<Vector3> path = new List<Vector3>();
            path.Clear();

            Fish fish = FindObjectOfType<Fish>();
            
            //path[1] = Vector3.Lerp(transform.position, fish.transform.position, 0.5f);

            int pointCount = Random.Range(2, 4);
            for (int i = 1; i <= pointCount; ++i)
            {
                Vector3 pos;
                pos = Vector3.Lerp(transform.position, fish.transform.position, (float)(i-1) / pointCount);
                path.Add(pos);

                pos = Vector3.Lerp(transform.position, fish.transform.position, (float)i / pointCount);
                path.Add(pos + Vector3.one * Random.Range(0.3f, 0.5f));
                
                pos = Vector3.Lerp(transform.position, fish.transform.position, (float)(i - 1) / pointCount);
                path.Add(pos + Vector3.one * Random.Range(0.3f, 0.5f));
            }

            transform.DOPath(path.ToArray(), 2f, PathType.CubicBezier, PathMode.Full3D);
        }
    }
}
