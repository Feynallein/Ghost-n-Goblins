using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventsManager;

[RequireComponent(typeof(Rigidbody2D))]
public class Moving : MonoBehaviour {
    [Header("Player's movement stats")]
    [SerializeField] float m_playerSpeed;
    [SerializeField] float m_jumpForce;
    [SerializeField] float _ClimbingSpeed;

    Transform _MapBeginning;
    Transform _MapEnding;

    Rigidbody2D m_rigidbody2D;
    BoxCollider2D _BoxCollider2D;
    public bool RigidbodyIsKinematic { set { m_rigidbody2D.isKinematic = value; } }

    bool _IsGrounded = true;

    bool _IsOnLadder = false;

    [Header("Layer masks")]
    [Tooltip("Layer to detect stuff with ladder related behaviour")]
    [SerializeField] LayerMask _LadderLayerMask;

    [Tooltip("Layer to every stuff that is ground related behaviour")]
    [SerializeField] LayerMask _GroundedLayerMask;

    void Awake() {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        _BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() {
        if (!GameManager.Instance.IsPlaying) return;

        // Movement
        transform.position = transform.position.x < _MapBeginning.position.x ? new Vector3(_MapBeginning.position.x, transform.position.y, transform.position.z) : transform.position;
        transform.position = transform.position.x > _MapEnding.position.x ? new Vector3(_MapEnding.position.x, transform.position.y, transform.position.z) : transform.position;
        Move();

        if (_IsOnLadder) MoveUp();
        
        // Jump
        if (Input.GetKey(KeyCode.Space) && IsGrounded()) {
            //m_rigidbody2D.AddForce(new Vector2(m_rigidbody2D.velocity.x, m_jumpForce), ForceMode2D.Impulse);
            m_rigidbody2D.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
        }
    }

    void MoveUp() {
        float vInput = Input.GetAxis("Vertical");
        float moveValue = vInput * _ClimbingSpeed;
        m_rigidbody2D.velocity = new Vector2(m_rigidbody2D.velocity.x, m_rigidbody2D.velocity.y + moveValue);
        m_rigidbody2D.angularVelocity = 0;
        m_rigidbody2D.MoveRotation(0);
    }

    void Move() {
        float hInput = Input.GetAxisRaw("Horizontal");
        float moveValue = hInput * m_playerSpeed;
        //m_rigidbody2D.velocity = new Vector2(moveValue, m_rigidbody2D.velocity.y);
        m_rigidbody2D.AddForce(new Vector2(moveValue, m_rigidbody2D.velocity.y));
        m_rigidbody2D.angularVelocity = 0;
        m_rigidbody2D.MoveRotation(0);
    }

    public void SetPositionAndMapBounds(Vector3 position, Transform mapBeginning, Transform mapEnding) {
        transform.position = position;
        _MapBeginning = mapBeginning;
        _MapEnding = mapEnding;
    }

    void IsOnLadder(bool b) {
        _IsOnLadder = b;
    }

    bool IsGrounded() {
        RaycastHit2D rayCastHit2D = Physics2D.BoxCast(_BoxCollider2D.bounds.center, _BoxCollider2D.bounds.size, 0f, Vector2.down, .1f, _GroundedLayerMask);
        return rayCastHit2D.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        //Is grounded : on devrait vérifier le point de contact (transform) et non les bordures aussi!
        //donc c'est pas une histoire de collision
        //mais ca fonctionne, de manière temporaire
        //if ((_GroundLayerMask.value & (1 << collision.gameObject.layer)) > 0) IsGrounded(true);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        //if ((_GroundLayerMask.value & (1 << collision.gameObject.layer)) > 0) IsGrounded(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if ((_LadderLayerMask.value & (1 << collision.gameObject.layer)) > 0) IsOnLadder(true);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if ((_LadderLayerMask.value & (1 << collision.gameObject.layer)) > 0) IsOnLadder(false);
    }
}