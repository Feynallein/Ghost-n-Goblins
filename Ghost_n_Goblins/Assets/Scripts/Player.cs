using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    [SerializeField] float m_playerSpeed;
    [SerializeField] float m_jumpForce;

    Rigidbody2D m_rigidbody2D;

    [SerializeField] bool _IsGrounded = true;

    private void Awake() {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Movement
        Move();

        // Jump
        if (Input.GetKey(KeyCode.Space) && _IsGrounded) {
            m_rigidbody2D.AddForce(new Vector2(m_rigidbody2D.velocity.x, m_jumpForce), ForceMode2D.Impulse);
        }
    }

    void Move() {
        float hInput = Input.GetAxis("Horizontal");
        float moveValue = hInput * m_playerSpeed;
        m_rigidbody2D.velocity = new Vector2(moveValue, m_rigidbody2D.velocity.y);
        m_rigidbody2D.angularVelocity = 0;
        m_rigidbody2D.MoveRotation(0);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) _IsGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) _IsGrounded = false;
    }
}
