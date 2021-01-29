using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameMgr : MonoBehaviour
{
    private Entity focusedEntity;

    public void OnMouseDown(Entity entity)
    {
        if (focusedEntity != null)
        {
            focusedEntity.OnDeselected();
        }
        if (entity != null)
        {
            entity.OnSelected();
        }
        focusedEntity = entity;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
