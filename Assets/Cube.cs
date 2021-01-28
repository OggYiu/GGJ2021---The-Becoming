using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cube : MonoBehaviour, IDraggable
{
    Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnDragBegin()
    {
        body.isKinematic = true;
    }

    public void OnDragEnd()
    {
        body.isKinematic = false;
    }

    public void OnDrag(Vector3 pos)
    {
        transform.position = pos + Vector3.up * 1.5f;
    }
}
