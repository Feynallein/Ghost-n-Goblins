<<<<<<< Updated upstream
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
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State { IDLE, JUMP }; // States of character
    public State m_state = State.IDLE;
    
    [SerializeField]
    float playerSpeed = 10f;
    [SerializeField]
    float jumpForce = 10f;

    private bool isJump = false;
    Rigidbody2D m_rigidbody2D;

   
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal") * playerSpeed;

        //if (m_state == State.IDLE)
        {
            // Movement
            Vector3 movement = xInput * transform.right * playerSpeed * Time.deltaTime;
            transform.Translate(movement);
            //m_rigidbody2D.MovePosition(m_rigidbody2D.position + (Vector2)movement);

            // Jump
            if (Input.GetKeyDown(KeyCode.Space))
                isJump = true;
        }
    }

    private void FixedUpdate()
    {
        // Jump
        if (isJump)
        {
            m_rigidbody2D.velocity = Vector3.up * jumpForce;
           // m_rigidbody2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))       
            m_state = State.IDLE;
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            m_state = State.JUMP;
            isJump = false;
        }
        
    }
}
>>>>>>> Stashed changes
