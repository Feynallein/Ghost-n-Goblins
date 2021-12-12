using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unicorn : Enemy {
    [Header("Monster specifications")]
    [SerializeField] float _JumpHeight;
    [SerializeField] float _JumpCooldown;
    [SerializeField] float _HorizontalJumpOffset;
    float _ElapsedTime;
    BoxCollider2D _BoxCollider2D;

    private void Awake() {
        _BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    protected override void Attack() {
        if (_ElapsedTime > _JumpCooldown && Layers.Instance.IsGrounded(_BoxCollider2D)) {
            Jump(_JumpHeight, _HorizontalJumpOffset);
            //jump opposite of player is also viable if too far away from spawn point
            _ElapsedTime = 0;
        }
        _ElapsedTime += Time.deltaTime;

        //they also can do a big horizontal jump & throw projectiles but not for now
    }

    protected override void Move() {
        _Rigidbody2D.angularVelocity = 0;
        _Rigidbody2D.MoveRotation(0);
    }

    protected override void PlayerDetected() {
        //todo: ca marche pas...
        //if (!Layers.Instance.IsFacingPlayer(transform, _DetectionRange, yOffset/2)) transform.Rotate(Vector3.up, 180, Space.Self);
    }
}
