
using UnityEngine;

internal interface IDraggable
{
    void OnDrag(Vector3 pos);
    void OnDragBegin();
    void OnDragEnd();
}