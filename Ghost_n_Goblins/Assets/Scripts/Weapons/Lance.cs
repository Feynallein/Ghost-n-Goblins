using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : Projectile {
    protected override void Move() {
        //How the lance moves
        _Rigidbody2D.AddForce(transform.right * _Speed, ForceMode2D.Impulse);
    }
}