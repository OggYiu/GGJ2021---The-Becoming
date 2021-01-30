using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        IGameTrigger gameTrigger = other.GetComponent<IGameTrigger>();
        gameTrigger.OnGameTriggerEnter(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        IGameTrigger gameTrigger = other.GetComponent<IGameTrigger>();
        gameTrigger.OnGameTriggerStay(this.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        IGameTrigger gameTrigger = other.GetComponent<IGameTrigger>();
        gameTrigger.OnGameTriggerExit(this.gameObject);
    }
}