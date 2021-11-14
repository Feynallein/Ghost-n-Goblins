using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventsManager;

[RequireComponent(typeof(Rigidbody2D))]
public class Move : MonoBehaviour {
    [Header("Player's movement stats")]
    [SerializeField] float _MovementSpeed;
    [SerializeField] float _ClimbingSpeed;

    Transform _MapBeginning;
    Transform _MapEnding;

    Rigidbody2D _Rigidbody2D;

    bool _IsOnLadder = false;

    [Header("Layer masks")]
    [Tooltip("Layer to detect stuff with ladder related behaviour")]
    [SerializeField] LayerMask _LadderLayerMask;

    void Awake() {
        _Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (!GameManager.Instance.IsPlaying) return;
        CheckMapBounds();
        LeftRightMove();
        MoveUp();
        _Rigidbody2D.angularVelocity = 0;
        _Rigidbody2D.MoveRotation(0);
    }

    void CheckMapBounds() {
        // Change the position if the player try to get out of map's bounds
        transform.position = transform.position.x < _MapBeginning.position.x ? 
            new Vector3(_MapBeginning.position.x, transform.position.y, transform.position.z) : transform.position;
        transform.position = transform.position.x > _MapEnding.position.x ? 
            new Vector3(_MapEnding.position.x, transform.position.y, transform.position.z) : transform.position;
    }
    
    void MoveUp() {
        if (!_IsOnLadder) return;
        float vInput = Input.GetAxis("Vertical");
        float moveValue = vInput * _ClimbingSpeed;
        _Rigidbody2D.velocity = new Vector2(_Rigidbody2D.velocity.x, _Rigidbody2D.velocity.y + moveValue);
    }

    void LeftRightMove() {
        float hInput = Input.GetAxisRaw("Horizontal");
        float targetVelocity = hInput * _MovementSpeed;
        _Rigidbody2D.AddForce(new Vector2(targetVelocity - _Rigidbody2D.velocity.x, 0), ForceMode.VelocityChange);
    }

    public void SetPositionAndMapBounds(Vector3 position, Transform mapBeginning, Transform mapEnding) {
        transform.position = position;
        _MapBeginning = mapBeginning;
        _MapEnding = mapEnding;
    }

    void IsOnLadder(bool b) {
        _IsOnLadder = b;
    }

    public bool RigidbodyIsKinematic { set { _Rigidbody2D.isKinematic = value; } }

    private void OnTriggerEnter2D(Collider2D collision) {
        if ((_LadderLayerMask.value & (1 << collision.gameObject.layer)) > 0) IsOnLadder(true);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if ((_LadderLayerMask.value & (1 << collision.gameObject.layer)) > 0) IsOnLadder(false);
    }
}

/*switch (forceMode) {
    case ForceMode.Force:
        return force;
    case ForceMode.Impulse:
        return force / Time.fixedDeltaTime;
    case ForceMode.Acceleration:
        return force * rigidbody2d.mass;
    case ForceMode.VelocityChange:
        return force * rigidbody2d.mass / Time.fixedDeltaTime;
}*/