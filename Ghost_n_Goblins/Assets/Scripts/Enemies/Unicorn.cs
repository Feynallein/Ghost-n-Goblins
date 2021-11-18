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

    protected override void Awake() {
        base.Awake();
        _BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    protected override void Attack() {
        Debug.Log(_ElapsedTime);
        if(_ElapsedTime > _JumpCooldown && Utils.IsGrounded(_BoxCollider2D)) {
            JumpForward(_JumpHeight, _HorizontalJumpOffset);
            //jumpbackward is also viable if too far away from spawn point
            _ElapsedTime = 0;
        }
        _ElapsedTime += Time.deltaTime;

        //they also can charge & throw projectiles but not for now
    }

    protected override void Move() {
        
    }

    protected override void PlayerDetected() {
        //todo: face player
    }
}
