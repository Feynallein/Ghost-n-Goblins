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

    float _WaitingTime;
    bool _FacedPlayer = false;

    private void Start() {
        _WaitingTime = _ShootCooldown - 1;
    }


    protected override void Attack() {
        if (_WaitingTime > _ShootCooldown) {
            Shoot(_Spear, _SpearSpeed, _ProjectileSpawnPoint, _SpearsOnScreen, PlayerBelow());
            _WaitingTime = 0;
        }
        else _WaitingTime += Time.deltaTime;
    }

    protected override void Move() {
    }

    protected override void PlayerDetected() {
        if (!FacingPlayer() && !_FacedPlayer) FacePlayer();

        GoForward(_Speed);
        _FacedPlayer = true;
    }

    bool PlayerBelow() {
        return LevelInterface.Instance.PlayerGfx.transform.position.x < transform.position.x + _BelowOffset && LevelInterface.Instance.PlayerGfx.transform.position.x > transform.position.x - _BelowOffset;
    }

    void Shoot(GameObject projectile, float projectileSpeed, Transform projectileSpawnPoint, int numberOfProjectileOnScreen, bool downward) {
        if (_OnScreenProjectiles.Count > numberOfProjectileOnScreen) return;
        GameObject newProjectile = Instantiate(_Spear, projectileSpawnPoint);
        Physics2D.IgnoreCollision(newProjectile.GetComponent<Collider2D>(), GetComponentsInChildren<Collider2D>()[0]);
        if (downward) newProjectile.transform.Rotate(new Vector3(90, 0, 0));
        newProjectile.GetComponent<MonsterProjectile>().Speed = projectileSpeed;
        newProjectile.GetComponent<MonsterProjectile>().Enemy = this;
        newProjectile.GetComponent<MonsterProjectile>().Shoot();
        _OnScreenProjectiles.Add(newProjectile);
    }
}
