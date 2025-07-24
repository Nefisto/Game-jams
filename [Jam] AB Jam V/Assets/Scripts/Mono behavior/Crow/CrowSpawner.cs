using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrowSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject crowPrefab;
    public BoxCollider2D leftSpawnerArea;
    public BoxCollider2D rightSpawnerArea;
    
    private GameSettings gameSettings;
    private Coroutine spawnerCoroutine;
    
    #region Monobehaviour callbacks

    private void Awake()
    {
        gameSettings = GameSettings.Instance;
    }

    private void OnEnable()
    {
        GameEvents.OnStartGame += StartSpawnCoroutine;
    }

    private void OnDisable()
    {
        GameEvents.OnStartGame -= StartSpawnCoroutine;
    }

    #endregion

    #region Private Methods

    private void StartSpawnCoroutine()
    {
        if (spawnerCoroutine != null)
            StopSpawnCoroutine();

        spawnerCoroutine = StartCoroutine(SpawnerCoroutine());
    }

    private void StopSpawnCoroutine()
    {
        StopCoroutine(spawnerCoroutine);
    }

    private IEnumerator SpawnerCoroutine()
    {
        while (true)
        {
            if (CrowManager.CrowsInGame.Count >= gameSettings.crow.maxAmountOfCrowsAtSameTime)
            {
                yield return new WaitForSeconds(2f);
                continue;
            }
            
            SpawnCrow();

            yield return new WaitForSeconds(Random.Range(gameSettings.crow.timeBetweenSpawn.x, gameSettings.crow.timeBetweenSpawn.y));
        }
    }
    
    [Button, DisableInEditorMode]
    private void SpawnCrow()
    {
        var boundOfChosenSide = Random.value < .5f
            ? rightSpawnerArea.bounds
            : leftSpawnerArea.bounds;

        var randomX = Random.Range(boundOfChosenSide.min.x, boundOfChosenSide.max.x);
        var randomY = Random.Range(boundOfChosenSide.min.y, boundOfChosenSide.max.y);

        var crow = Instantiate(crowPrefab, new Vector3(randomX, randomY, 0f), Quaternion.identity, transform);
        
        crow.GetComponent<Crow>().ChooseAPumpkin();
    }
    
    #endregion
}