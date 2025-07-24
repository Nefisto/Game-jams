#pragma warning disable 0649

using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Pooler", menuName = "Framework/Manager/Pool")]
public class PoolManager : ScriptableSingleton<PoolManager>
{
    // FRAME-TODO: Pensar num jeito melhor para implementar isso
    // [System.Serializable]
    // public class PossiblePools
    // {
    //     public Pool explosion;
    //     public Pool basicBomb;
    //     public Pool spikeBomb;
    // }

    // public PossiblePools possiblePools;

    [SerializeField]
    private List<Pool> poolsGroup;

    public Pool this[string name]
    {
        get
        {
            var res = poolsGroup.Find(pool => pool.name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (!res)
                Debug.LogError("O pooler que vc esta tentando acessar nao existe");

            return res;
        }
    }

    public void InitAllPools()
    {
        var folder = new GameObject("Pools");
        foreach (var pool in poolsGroup)
            pool.InitPool().parent = folder.transform;
    }

    public void CloseAllPools()
    {
        foreach (var pool in poolsGroup)
            pool.ClosePool();
    }
}
