using UnityEngine;
using System.Reflection;
using System;

public partial class Enemy : MonoBehaviour
{
    public EnemyStatus blueprintStatus;

    [Header("Events")]
    public GameEventInt loseLife;
    public GameEventInt giveMoney;

    [Header("Debug")]
    public Waypoints pathToFollow = null;    

    private EnemyStatus status;

    // Tell to towers that i've died
    public event Action onDie;
    
    private void Start()
    {
        if (!blueprintStatus)
            Debug.LogError("This enemy does not have a blueprint status", this);

        status = ScriptableObject.CreateInstance<EnemyStatus>();
        ReflectionStatus();

        InitStatus();
    }

    private void InitStatus()
    {
        status.life = blueprintStatus.initialLife.GetRandom();
        status.speed = blueprintStatus.initialSpeed.GetRandom();
    }

    //* PROUD 
    private void ReflectionStatus()
    {
        var myType = typeof(EnemyStatus);

        foreach (var field in myType.GetFields())
        {
            var currentValue = field.GetValue(blueprintStatus);
            field.SetValue(status, currentValue);
        }
    }
}