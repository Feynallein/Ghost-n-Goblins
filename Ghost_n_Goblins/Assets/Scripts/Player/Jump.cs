using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventsManager;

public class Jump : MonoBehaviour {
    [SerializeField] float _JumpHeight;
    [SerializeField] float _GravityScale;
    [SerializeField] float _FallingGravityScale;

    Rigidbody2D _Rigidbody2D;
    BoxCollider2D _BoxCollider2D;

    [Tooltip("Layer to every stuff that is ground related behaviour")]
    [SerializeField] LayerMask _GroundedLayerMask;

    [Tooltip("Layer to detect stuff with ladder related behaviour")]
    [SerializeField] LayerMask _LadderLayerMask;

    void Awake() {
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        _BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        if (!GameManager.Instance.IsPlaying) return;

        //Correction slight velocity changes (sometimes little velocity like x10^-6)
        if (_Rigidbody2D.velocity.y < .1 && _Rigidbody2D.velocity.y > -.1) _Rigidbody2D.velocity = new Vector2(_Rigidbody2D.velocity.x, 0);

        // Jumping if spacebar pressed!
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) JumpMethod(); // Axis doesn't work for whatever reason... (it stacks jump and you jump too high)

        // Changing gravity scale based on state (jumping, falling or on ladder)
        if (IsOnLadder() && transform.position.y > .1) _Rigidbody2D.gravityScale = 0; // If we're grounded but not with == 0 => we're on a ladder
        else if (_Rigidbody2D.velocity.y >= 0) _Rigidbody2D.gravityScale = _GravityScale;
        else if (_Rigidbody2D.velocity.y < 0) _Rigidbody2D.gravityScale = _FallingGravityScale;
    }
    
    void JumpMethod() {
        if (!IsGrounded()) return;
        float gravity = Physics2D.gravity.y * _Rigidbody2D.gravityScale;
        float jumpForce = Mathf.Sqrt(-2 * gravity * _JumpHeight);
        _Rigidbody2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    bool IsGrounded() {
        RaycastHit2D rayCastHit2D = 
            Physics2D.BoxCast(_BoxCollider2D.bounds.center, _BoxCollider2D.bounds.size, 0f, Vector2.down, .2f, _GroundedLayerMask);
        return rayCastHit2D.collider != null;
    }

    // Try to optimize and encapsulate w/ move script (ie: abstract class "movements" which have common parmaters & methods)
    bool IsOnLadder() {
        RaycastHit2D rayCastHit2D =
            Physics2D.BoxCast(_BoxCollider2D.bounds.center, _BoxCollider2D.bounds.size, 0f, Vector2.down, .2f, _LadderLayerMask);
        return rayCastHit2D.collider != null;
    }
}

// Function to calculate velocity based on height :
// velocity = sqrt(-2 * gravity * height)
