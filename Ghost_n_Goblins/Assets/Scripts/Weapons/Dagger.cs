using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Projectile
{
    protected override void Move()
    {
        _Rigidbody2D.AddForce(transform.right * _Speed, ForceMode2D.Impulse);
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
