using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

public class Notes : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private List<StudioEventEmitter> notes;

    [TitleGroup("References")]
    [SerializeField]
    private List<KeyCode> keys;
    
    private void Update()
    {
        var indexPressed = GetKey();

        if (indexPressed == -1)
            return;
        
        notes[indexPressed].Play();
    }

    private int GetKey()
    {
        foreach (var (k, i) in keys.Select((k, i) => (k, i)))
            if (Input.GetKeyDown(k))
                return i;

        return -1;
    }
}