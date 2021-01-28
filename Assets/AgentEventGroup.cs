using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentEventGroup", menuName = "ScriptableObjects/AgentEventGroup", order = 1)]
public class AgentEventGroup : ScriptableObject
{
    public AgentEventScript[] scripts;
}