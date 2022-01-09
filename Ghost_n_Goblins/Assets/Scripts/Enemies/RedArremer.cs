using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedArremer : Enemy
{
    [Header("Monster specifications")]
    [SerializeField] GameObject _RedFlame;
    [SerializeField] float _RedFlameSpeed;
    [SerializeField] int _RedFlamesOnScreen;

    GameObject targetPlayer;

    protected override void Attack()
    {
        if (!FacingPlayer()) FacePlayer();
        SpellAtPlayer(_RedFlame, _RedFlameSpeed, _ProjectileSpawnPoint, _RedFlamesOnScreen);
    }

    protected override void Move()
    { 
        // Empty: this monster doesn't move
    }

    protected override void PlayerDetected()
    { 
        // Empty: this monster has no reaction near player

    }

    void SpellAtPlayer(GameObject projectile, float projectileSpeed, Transform projectileSpawnPoint, int numberOfProjectileOnScreen)
    {
        if (_OnScreenProjectiles.Count > numberOfProjectileOnScreen) return;
        GameObject redFlame = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
        Physics2D.IgnoreCollision(redFlame.GetComponent<Collider2D>(), GetComponentsInChildren<Collider2D>()[0]);
        redFlame.transform.LookAt(LevelInterface.Instance.PlayerGfx);
        redFlame.GetComponent<MonsterProjectile>().Speed = projectileSpeed;
        redFlame.GetComponent<MonsterProjectile>().Enemy = this;
        redFlame.GetComponent<MonsterProjectile>().Shoot();
        _OnScreenProjectiles.Add(redFlame); 
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(9, 7); //Enemy & obstacle
    }
}
