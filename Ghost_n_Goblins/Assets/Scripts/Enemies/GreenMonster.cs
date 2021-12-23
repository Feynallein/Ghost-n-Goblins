using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMonster : Enemy {
    [Header("Monster specifications")]
    [Tooltip("Monter's projectile prefab")]
    [SerializeField] GameObject _EyeBall;
    [SerializeField] float _EyeBallSpeed;
    [SerializeField] int _EyeBallsOnScreen;

    protected override void Attack() {
        if (!FacingPlayer()) FacePlayer();
        ShootAtPlayer(_EyeBall, _EyeBallSpeed, _ProjectileSpawnPoint, _EyeBallsOnScreen);
    }

    protected override void Move() { // Empty: this monster doesn't have idle move
    }

    protected override void PlayerDetected() { // Empty: this monster has no reaction near player
    }

    void ShootAtPlayer(GameObject projectile, float projectileSpeed, Transform projectileSpawnPoint, int numberOfProjectileOnScreen) {
        if (_OnScreenProjectiles.Count > numberOfProjectileOnScreen) return;
        GameObject newProjectile = Instantiate(projectile, projectileSpawnPoint);
        Physics2D.IgnoreCollision(newProjectile.GetComponent<Collider2D>(), GetComponentsInChildren<Collider2D>()[0]);
        newProjectile.transform.LookAt(LevelInterface.Instance.PlayerGfx);
        newProjectile.GetComponent<MonsterProjectile>().Speed = projectileSpeed;
        newProjectile.GetComponent<MonsterProjectile>().Enemy = this;
        newProjectile.GetComponent<MonsterProjectile>().Shoot();
        _OnScreenProjectiles.Add(newProjectile);
    }
}