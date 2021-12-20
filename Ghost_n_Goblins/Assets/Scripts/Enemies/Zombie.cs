using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy {
    [SerializeField] float _MovementSpeed;

    bool _WentForward = false;

    private void Start() {
        Physics2D.IgnoreLayerCollision(11, 8);
    }

    protected override void Attack() { //No Attack only run & damage if hit the player
    }

    protected override void Move() { // No idle move
    }

    protected override void PlayerDetected() {
        if (!FacingPlayer()) {
            FacePlayer();
            _Rigidbody2D.velocity = Vector2.zero;
            GoForward(_MovementSpeed);
        }

        if (_WentForward) return;
        if(_Rigidbody2D.velocity != Vector2.zero) GoForward(_MovementSpeed);
        StartCoroutine(RemoveAngularMovementCoroutine());
        _WentForward = true;
    }
}
