using UnityEngine;

public partial class Enemy
{
    public void TakeDamage(int dmg)
    {
        status.life -= dmg;

        if (status.life < 0)
            Die();
    }

    private void Die(bool giveDamage = false)
    {
        if (onDie != null)
            onDie();

        if (giveDamage)
            loseLife.Raise(status.damage);
        else
            giveMoney.Raise(status.money.GetRandom());

        Destroy(gameObject);
    }
}