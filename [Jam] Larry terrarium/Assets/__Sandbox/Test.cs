using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float radius = 1.5f;
    public Transform finalPosition;

    public ContactFilter2D contactFilter2D;

    public Animator animator;

    private void Update()
    {
        var c = animator.GetCurrentAnimatorClipInfo(0)[0].clip;

        Debug.Log($"{c.length}");
        Debug.Log($"{c.frameRate}");
    }

    [Button]
    private void Go()
    {
        var foundLights = new Collider2D[5]; 

        var amount = Physics2D.OverlapCircle(finalPosition.position, radius, contactFilter2D, foundLights);
        Debug.Log($"AMOUNT: {amount}");

        for (var i = 0; i < amount; i++)
        {
            Debug.Log($"{foundLights[i].name}");
        }
    }

    [Button]
    private void Detect()
    {
        var cachedFoods = new Collider2D[10];
        
        var amount = Physics2D.OverlapBoxNonAlloc(transform.position, new Vector2(1f, 1f),
            0f, cachedFoods, LayerMask.GetMask("Food"));

        if (amount == 0)
            return;
        
        foreach (var collider2D1 in cachedFoods.Take(amount))
        {
            Debug.Log($"{collider2D1.name}");
        }
    }
}