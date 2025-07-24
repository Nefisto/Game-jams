using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EatSpot
{
    public Transform eatSpot;
    public Crow crow;
}

public class Pumpkin : LazyBehavior
{
    public event Action<float> OnUpdateLife;
    public event Action OnDie;
    
    private PumpkinLife pumpkinLife;

    // public List<PositionAndCrow> crowPosition;
    public EatSpot rightSpot;
    public EatSpot leftSpot;

    #region Properties

    public PumpkinState GetState => pumpkinLife.currentState;

    public List<Crow> crowsEatingMe;

    #endregion
    
    private void OnEnable()
    {
        pumpkinLife.OnUpdateLife += UpdateLife;
        pumpkinLife.OnDie += Die;
        
        PumpkinManager.AlivePumpkins.Add(this);
    }

    private void OnDisable()
    {
        pumpkinLife.OnUpdateLife -= UpdateLife;
        pumpkinLife.OnDie -= Die;
        
        PumpkinManager.AlivePumpkins.Remove(this);
    }

    private void Awake()
    {
        pumpkinLife = GetComponent<PumpkinLife>();
    }

    public void DisableGraphics()
    {
        spriteRenderer.enabled = false;
    }

    public void EnableGraphics()
    {
        spriteRenderer.enabled = true;
    }
    
    public void ChangeState(PumpkinState state)
    {
        pumpkinLife.ChangeState(state);
    }

    private void UpdateLife (float percentage)
    {
        OnUpdateLife?.Invoke(percentage);
    }

    private void Die()
    {
        if (rightSpot.crow != null)
        {
            rightSpot.crow.StopAllCoroutines();
            rightSpot.crow.RunAway();
        }
        
        if (leftSpot.crow != null)
        {
            leftSpot.crow.StopAllCoroutines();
            leftSpot.crow.RunAway();
        }
        
        OnDie?.Invoke();
    }

    public void AddCrow (Crow crow)
    {
        crowsEatingMe.Add(crow);
    }

    public void RemoveCrow (Crow crow)
    {
        crowsEatingMe.Remove(crow);
    }

    public void GetBit (float bitDamage)
    {
        pumpkinLife.GetBit(bitDamage);
    }
}