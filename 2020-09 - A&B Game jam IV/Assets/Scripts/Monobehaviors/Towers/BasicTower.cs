using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream;

public partial class BasicTower : LazyBehavior
{
    [Header("Editor only")]
    public Color32 wireFrameColor = Color.white;

    [Space(3), Header("Status")]
    public int damage = 1;

    public float fireRate = .5f;

    public float range = 3f;

    public GameObject bulletPrefab;
    

    [Header("Debug")]
    public Enemy targetEnemy;
    public List<Enemy> enemiesInRange;

    public int cost = 50;
    public IntReference playerMoney;

    private Coroutine atackRoutine;

    private void ResetStatus()
    {
        circleCollider2D.radius = range;
    }

    private void Start()
    {
        ResetStatus();
    }

    public bool CanBuy()
        => playerMoney.Value >= cost;

    public void TryBuy()
    {
        if (!CanBuy())
            return;
        
        playerMoney.Value -= cost;
    }

    protected bool CanAttack()
    {
        if (!targetEnemy)
            targetEnemy = FindNereastTarget();

        return targetEnemy && atackRoutine == null;
    }

    public virtual void Attack()
    {
        if (!CanAttack())
            return;

        atackRoutine = StartCoroutine(SingleAtackCoroutine());

        IEnumerator SingleAtackCoroutine()
        {
            while (enemiesInRange.Count != 0)
            {
                var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
                bullet.SetDamage(damage);
                bullet.StartFollowTarget(targetEnemy);

                yield return new WaitForSeconds(fireRate);
            }
        }
    }

    public void StopAttack()
    {
        if (atackRoutine != null)
        {
            StopCoroutine(atackRoutine);

            atackRoutine = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();

        if (!enemy)
            throw new System.Exception("Tower is colliding with something that isn't enemy");

        enemiesInRange.Add(enemy);

        if (enemiesInRange.Count == 1)
        {
            // ChangeState(State.Locked);
            targetEnemy = enemy;

            Attack();
        }
    }

    private Enemy FindNereastTarget()
    {
        // ! For first
        return enemiesInRange.Count == 0 ? null : enemiesInRange[0];

        // ! For nereast
        // if (enemiesInRange.Count == 0)
        //     return null;

        // var myPosition = transform.position;

        // var possibleTarget = enemiesInRange[0];
        // var lowerDist = (myPosition - possibleTarget.transform.position).magnitude;
        // for (int i = 1; i < enemiesInRange.Count; i++)
        // {
        //     var curDist = (myPosition - enemiesInRange[i].transform.position).magnitude;
        //     if (lowerDist > curDist)
        //     {
        //         possibleTarget = enemiesInRange[i];
        //         lowerDist = curDist;
        //     }
        // }

        // return possibleTarget;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();

        if (!enemy)
            throw new System.Exception("Tower is colliding with something that isn't enemy");

        enemiesInRange.Remove(enemy);

        if (enemy == targetEnemy)
            targetEnemy = FindNereastTarget();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = wireFrameColor;
        Gizmos.DrawWireSphere(transform.position, range);

        if (enemiesInRange.Count == 0)
            return;

        Gizmos.color = Color.red;
        foreach (var enemy in enemiesInRange)
        {
            if (enemy == targetEnemy)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, enemy.transform.position);
                Gizmos.color = Color.red;
            }
            else
                Gizmos.DrawLine(transform.position, enemy.transform.position);
        }
    }
}