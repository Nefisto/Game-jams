using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Group
{
    public GameObject enemy;
    public int amount;
    public float timeBetweenSpawns;
    public float timeAfterFinish;

}

[CreateAssetMenu(fileName = "Pack", menuName = "2020-09 - A&B Game jam IV/New pack", order = 0)]
public class Pack : ScriptableObject
{
    public List<Group> groups;
}