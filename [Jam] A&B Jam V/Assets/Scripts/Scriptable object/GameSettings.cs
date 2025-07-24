using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class CrowSettings
{
    public float speed = 5f;
    public float maxHungry = 100f;
    public float biteDamage = 10f;
    [MinMaxSlider(0.5f, 2.5f)] public Vector2 timeBetweenBite;
    public int maxAmountOfCrowsAtSameTime = 5;
    [MinMaxSlider(1.5f, 4f)] public Vector2 timeBetweenSpawn;
    public float clickDamage = 10f;
}

[Serializable]
public class PumpkinSettings
{
    public float maxLife = 100;
    [MinMaxSlider(0f, 100f)] public Vector2 initialLife;
    public float lifeGainedPerSecond = 7f;
    public float lifeLostPerSecond = 15f;
    public bool healWhenHaveCrows = false;
}

[Serializable]
public class Inputs
{
    public KeyCode changeHead = KeyCode.Space;
    public KeyCode cheatMode = KeyCode.R;
}

public class GameSettings : SingletonScriptableObject<GameSettings>
{
    [HideLabel, Header("Pumpkin")]
    public PumpkinSettings pumpkin;
    
    [HideLabel, Header("Crow")]
    public CrowSettings crow;

    [HideLabel, Header("Inputs")]
    public Inputs inputs;
}