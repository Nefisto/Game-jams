using System;
using System.Collections;
using System.Collections.Generic;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public class MothSpawn : MonoBehaviour
{
    public static int AmountOfMoths = 0;

    [TitleGroup("Settings")]
    [SerializeField]
    private int minAmountOfMoths;

    [TitleGroup("References")]
    [SerializeField]
    private List<Transform> locations;
    
    [TitleGroup("References")]
    [SerializeField]
    private Moth mothPrefab;

    private void Awake() => GameEvents.OnGameStart += () => StartCoroutine(SpawningCheckRoutine());

    private IEnumerator SpawningCheckRoutine()
    {
        while (true)
        {
            if (AmountOfMoths >= minAmountOfMoths)
            {
                yield return new WaitForSeconds(2f);
                continue;
            }

            var instance = Instantiate(mothPrefab, locations.GetRandom().position, Quaternion.identity, null);
            instance.Init();
            
            yield return new WaitForSeconds(3f);
        }
    }
}