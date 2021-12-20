using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodyPig : Enemy {
    [Header("Monster specifications")]
    [Tooltip("Monter's projectile prefab")]
    [SerializeField] GameObject _Spear;
    [SerializeField] float _SpearSpeed;
    [SerializeField] int _SpearsOnScreen;
    [SerializeField] float _Speed;
    [SerializeField] float _ShootCooldown;
    [SerializeField] int _BelowOffset;

    float waitingTime;

    protected override void Attack() {
        if (waitingTime > _ShootCooldown) {
            Shoot(_Spear, _SpearSpeed, _ProjectileSpawnPoint, _SpearsOnScreen, PlayerBelow());
            waitingTime = 0;
        }
        else waitingTime += Time.deltaTime;
    }

    protected override void Move() {
    }

    protected override void PlayerDetected() {
        GoForward(_Speed);
    }

    bool PlayerBelow() {
        return LevelInterface.Instance.PlayerGfx.transform.position.x < transform.position.x + _BelowOffset && LevelInterface.Instance.PlayerGfx.transform.position.x > transform.position.x - _BelowOffset;
    }

    void Shoot(GameObject projectile, float projectileSpeed, Transform projectileSpawnPoint, int numberOfProjectileOnScreen, bool downward) {
        if (_OnScreenProjectiles.Count > numberOfProjectileOnScreen) return;
        GameObject newProjectile = Instantiate(_Spear, projectileSpawnPoint);
        if (downward) newProjectile.transform.LookAt(Vector3.down); //todo: correct
        Physics2D.IgnoreCollision(newProjectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        newProjectile.GetComponent<MonsterProjectile>().Speed = projectileSpeed;
        newProjectile.GetComponent<MonsterProjectile>().Enemy = this;
        _OnScreenProjectiles.Add(newProjectile);
    }
}
