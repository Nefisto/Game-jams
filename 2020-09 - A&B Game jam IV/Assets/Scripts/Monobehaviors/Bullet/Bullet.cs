using System.Collections;
using UnityEngine;

public class Bullet : LazyBehavior
{
    [Header("Come from tower")]
    [ReadOnly] public int damage = 1;
    public float speed = 2;
    private Coroutine followRoutine = null;

    private Enemy targetEnemy = null;

    private void SetEnemy(Enemy enemy)
    {
        targetEnemy = enemy;
        targetEnemy.onDie += Explode;
    }

    private void UnsetEnemy()
    {
        if (followRoutine == null)
            throw new System.Exception("Bullet exist but isn't following");

        StopCoroutine(followRoutine);

        targetEnemy.onDie -= Explode;
    }

    public void SetDamage(int dmg)
        => damage = dmg;

    public void StartFollowTarget(Enemy enemy)
    {
        SetEnemy(enemy);

        followRoutine = StartCoroutine(FollowRoutine());

        IEnumerator FollowRoutine()
        {
            while (true)
            {
                var dist = (Vector2)(enemy.transform.position - transform.position);
                var dir = dist.normalized;

                if (dist.sqrMagnitude < .05f)
                {
                    enemy.TakeDamage(damage);
                    Explode();

                    break;
                }

                var angle = dir.ToDegreeAngle();
                transform.eulerAngles = new Vector3(0f, 0f, angle);
                
                dir = dir * Time.deltaTime * speed;
                
                transform.Translate(dir, Space.World);

                yield return null;
            }
        }
    }

    public void Explode()
    {
        UnsetEnemy();

        animator.SetTrigger("Die");
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}