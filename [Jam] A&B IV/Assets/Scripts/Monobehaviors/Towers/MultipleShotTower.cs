using System.Collections;
using UnityEngine;

public class MultipleShotTower : BasicTower
{
    public override void Attack()
    {   
        if (!CanAttack())
            return;

        StartCoroutine(MultipleAttackRoutine());

        IEnumerator MultipleAttackRoutine()
        {
            while (enemiesInRange.Count != 0)
            {
                foreach (var enemy in enemiesInRange)
                {
                    var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
                    bullet.StartFollowTarget(enemy);
                }

                yield return new WaitForSeconds(fireRate);
            }
        }
    }
}