using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedArremer : Enemy
{

    protected override void Attack()
    {
        //todo: face player
        // Type of attack
        //ShootAtPlayer(_EyeBall, _ProjectileSpeed, _ProjectileSpawnPoint, _ProjectileOnScreen);
    }

    protected override void Move()
    { 
        // Empty: this monster doesn't move
    }

    protected override void PlayerDetected()
    { 
        // Empty: this monster has no reaction near player
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
