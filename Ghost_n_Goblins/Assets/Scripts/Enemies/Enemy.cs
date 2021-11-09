using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IScore {
    [SerializeField] int _Score;
    [SerializeField] int _Health;
    [SerializeField] int _DamageDealt;

    public int Score { get { return _Score; } }

    public void TakeDamage(int damage) {
        _Health -= damage;
        if (_Health <= 0) Die();
    }

    void Die() {
        // Start death animation
        Loot();
        Destroy(gameObject);
    }

    protected abstract void Loot();

    protected abstract void Attack();

    protected abstract void Move();
}
