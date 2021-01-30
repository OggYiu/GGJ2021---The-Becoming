using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameTrigger
{
    void OnGameTriggerEnter(GameObject gameObject);
    void OnGameTriggerStay(GameObject gameObject);
    void OnGameTriggerExit(GameObject gameObject);
}
