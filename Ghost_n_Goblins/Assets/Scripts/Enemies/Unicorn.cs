using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unicorn : Enemy {
    [Header("Monster specifications")]
    [SerializeField] float _JumpHeight;
    [SerializeField] float _JumpCooldown;
    [SerializeField] float _HorizontalJumpOffset;
    [SerializeField] float _MovementSpeed;
    float _ElapsedTime;
    BoxCollider2D _BoxCollider2D;
    bool _WentForward = false;

    private void Start() {
        _BoxCollider2D = GetComponentsInChildren<BoxCollider2D>()[0];
    }

    protected override void Attack() {/*
        if (_ElapsedTime > _JumpCooldown && Layers.Instance.IsGrounded(_BoxCollider2D)) {
            Jump(_JumpHeight, _HorizontalJumpOffset);
            //jump opposite of player is also viable if too far away from spawn point
            _ElapsedTime = 0;
        }
        _ElapsedTime += Time.deltaTime;*/

        //they also can do a big horizontal jump & throw projectiles but not for now
    }

    protected override void Move() {
    }

    protected override void PlayerDetected() {
        if (!FacingPlayer()) {
            FacePlayer();
            _Rigidbody2D.velocity = Vector2.zero;
            GoForward(_MovementSpeed);
        }
        
        if (_WentForward) return;
        if (_Rigidbody2D.velocity != Vector2.zero) GoForward(_MovementSpeed);
        StartCoroutine(RemoveAngularMovementCoroutine());
        _WentForward = true;
    }
}
