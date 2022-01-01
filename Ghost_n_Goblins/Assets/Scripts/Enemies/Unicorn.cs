using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unicorn : Enemy {
    [Header("Monster specifications")]
    [SerializeField] float _JumpHeight;
    [SerializeField] float _JumpCooldown;
    [SerializeField] float _MovementSpeed;
    float _ElapsedTime;
    BoxCollider2D _BoxCollider2D;

    private void Start() {
        _BoxCollider2D = GetComponentsInChildren<BoxCollider2D>()[0];
        Physics2D.IgnoreLayerCollision(11, 8);
    }

    protected override void Attack() {
        if (_ElapsedTime > _JumpCooldown && Layers.Instance.IsGrounded(_BoxCollider2D)) {
            Jump(_JumpHeight);
            _ElapsedTime = 0;
        }
        _ElapsedTime += Time.deltaTime;
    }

    protected void Jump(float jumpHeight) {
        float gravity = Physics2D.gravity.y * _Rigidbody2D.gravityScale;
        float jumpForce = Mathf.Sqrt(-2 * gravity * _JumpHeight);
        _Rigidbody2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    protected override void Move() {
    }

    protected override void PlayerDetected() {
        if (!FacingPlayer()) FacePlayer();
        
        GoForward(_MovementSpeed);
    }
}
