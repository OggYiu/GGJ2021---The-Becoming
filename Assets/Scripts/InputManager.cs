using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Ray ray;
    Vector3 hitPoint;

    IDraggable draggable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var screenPoint = Input.mousePosition;
        ray = Camera.main.ScreenPointToRay(screenPoint);

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("GetMouseButtonDown");
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                hitPoint = hit.point;
                draggable = hit.collider.gameObject.GetComponent<IDraggable>();
                //Debug.Log("draggable: " + draggable);
                if (draggable != null) draggable.OnDragBegin();
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            //Debug.Log("GetMouseButtonUp");
            if (draggable != null) {
                draggable.OnDragEnd();
                draggable = null;
            }
        }
        else
        {
            //Debug.Log("else");
            int CubeLayerMask = (1 << LayerMask.NameToLayer("Cube"));
            CubeLayerMask = ~CubeLayerMask;

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, CubeLayerMask))
            {
                hitPoint = hit.point;
            }

            if (draggable != null) draggable.OnDrag(hitPoint);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * 100f);
        Gizmos.DrawSphere(hitPoint, 0.2f);
    }
}
