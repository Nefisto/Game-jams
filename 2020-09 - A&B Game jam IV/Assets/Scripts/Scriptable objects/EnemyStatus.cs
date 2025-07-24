using UnityEngine;

[CreateAssetMenu(fileName = "New enemy", menuName = "2020-09 - A&B Game jam IV/Enemy Status", order = 0)]
public class EnemyStatus : ScriptableObject
{
    [MinMaxRange(1, 30)]
    public IntRange initialLife;

    [MinMaxRange(1, 30)]
    public FloatRange initialSpeed;

    public int damage = 1;

    [MinMaxRange(1, 100)]
    public IntRange money;

    [HideInInspector] public int life;
    [HideInInspector] public float speed;
}