using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMonster : Enemy {
    [Header("Monster specifications")]
    [Tooltip("Monter's projectile prefab")]
    [SerializeField] GameObject _EyeBall;
    [SerializeField] float _ProjectileSpeed;
    [SerializeField] int _ProjectileOnScreen;

    protected override void Attack() {
        // Type of attack
        ShootAtPlayer(_EyeBall, _ProjectileSpeed, _ProjectileSpawnPoint, _ProjectileOnScreen);
    }

    protected override void Move() { // Empty: this monster doesn't move
    }

    protected override void PlayerDetected() { // Empty: this monster has no reaction near player
    }
}