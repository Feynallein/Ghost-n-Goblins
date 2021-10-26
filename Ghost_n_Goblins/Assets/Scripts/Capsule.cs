using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
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

        if (m_state == State.IDLE)
        {
            // Movement
            Vector3 movement = xInput * transform.right * Time.deltaTime;
            transform.Translate(movement);

            // Jump
            if (Input.GetKeyDown(KeyCode.Space))
                isJump = true;            
        }     
    }

    private void FixedUpdate()
    {
        // Jump
        if (isJump)
            m_rigidbody2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);       
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
