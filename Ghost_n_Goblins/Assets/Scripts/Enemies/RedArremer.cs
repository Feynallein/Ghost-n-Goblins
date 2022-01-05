using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedArremer : Enemy
{
    [Header("Monster specifications")]
    [SerializeField] GameObject _RedFlame;
    [SerializeField] float _RedFlameSpeed;

    GameObject targetPlayer;

    protected override void Attack()
    {
        //todo: face player
        // Type of attack
        //ShootAtPlayer(_EyeBall, _ProjectileSpeed, _ProjectileSpawnPoint, _ProjectileOnScreen);
        if (!FacingPlayer()) FacePlayer();
        SpellAtPlayer();
    }

    protected override void Move()
    { 
        // Empty: this monster doesn't move
    }

    protected override void PlayerDetected()
    { 
        // Empty: this monster has no reaction near player

    }

    void SpellAtPlayer()
    {
        GameObject redFlame = Instantiate(_RedFlame, _ProjectileSpawnPoint.position, Quaternion.identity);
        targetPlayer = GameObject.FindWithTag("Player");
        Vector3 AttackAtPlayerPosition = (targetPlayer.transform.position - redFlame.transform.position).normalized * _RedFlameSpeed;
        redFlame.GetComponent<Rigidbody2D>().velocity = new Vector2(AttackAtPlayerPosition.x, AttackAtPlayerPosition.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
