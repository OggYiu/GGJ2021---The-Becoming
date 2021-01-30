using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathGameTrigger : MonoBehaviour, IGameTrigger
{
    public void OnGameTriggerEnter(GameObject gameObject)
    {
        GameMgr gameMgr = FindObjectOfType<GameMgr>();
        gameMgr.KillFish();
    }

    public void OnGameTriggerStay(GameObject gameObject)
    {
    }

    public void OnGameTriggerExit(GameObject gameObject)
    {
    }
}
