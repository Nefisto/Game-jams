using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameLoader : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private Image background;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

        var fadeIn = background.DOFade(0f, 2f);
        GameEvents.OnGameStart?.Invoke();
        
        yield return fadeIn.WaitForCompletion();
        
        GameEvents.OnUnlockPlayerMovement?.Invoke();
    }
}