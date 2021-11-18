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
    BoxCollider2D _BoxCollider2D;

    void Awake() {
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        _BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() {
        if (!GameManager.Instance.IsPlaying) return;
        CheckMapBounds();
        LeftRightMove();
        MoveOnLadder();
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
    
    void MoveOnLadder() {
        if (!Layers.Instance.IsOnLadder(_BoxCollider2D)) return;
        //todo: add a snap to the gameobject
        float vInput = Input.GetAxisRaw("Vertical");
        float moveValue = vInput * _ClimbingSpeed;
        _Rigidbody2D.AddForce(new Vector2(0, moveValue - _Rigidbody2D.velocity.y), ForceMode.VelocityChange);
        GoThroughPlateforms();
    }

    void GoThroughPlateforms() {
        // Allow the player to go through a plateform from below
        if (_Rigidbody2D.velocity.y > 0 || (Layers.Instance.IsOnLadder(_BoxCollider2D) && Input.GetAxisRaw("Vertical") < 0)) Physics2D.IgnoreLayerCollision(gameObject.layer, 7, true); //maybe find to change the 7
        else Physics2D.IgnoreLayerCollision(gameObject.layer, 7, false);
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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (Layers.Instance.CheckIfCollidedLayerIsMovingPlateform(collision.gameObject.layer)) transform.SetParent(collision.gameObject.transform);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if(Layers.Instance.CheckIfCollidedLayerIsMovingPlateform(collision.gameObject.layer)) collision.gameObject.transform.DetachChildren();
    }

    public bool RigidbodyIsKinematic { set { _Rigidbody2D.isKinematic = value; } }
}