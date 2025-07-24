using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;

[System.Serializable]
public class PacknPath
{
    public int pathNumber;

    public List<Pack> packs;
}

[CreateAssetMenu(fileName = "Wave", menuName = "2020-09 - A&B Game jam IV/New wave", order = 0)]
public class Wave : ScriptableObject
{   
    public List<PacknPath> packnPaths;
}