using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventsManager;

public abstract class Enemy : MonoBehaviour, IScore {
    #region Variables
    [Header("Stats")]
    [Tooltip("Score value when killed")]
    [SerializeField] int _Score;

    [Tooltip("Monster's health")]
    [SerializeField] int _Health;

    [Tooltip("Monster's projectile spawn point")]
    [SerializeField] protected Transform _ProjectileSpawnPoint;


    //Todo tooltips
    [SerializeField] protected float _AttackRange;
    [SerializeField] protected float _DetectionRange;

    protected List<GameObject> _OnScreenProjectiles = new List<GameObject>();

    protected Rigidbody2D _Rigidbody2D;

    protected float yOffset = 1.5f; //TMP: 1.5 = (gfx) scale.y /2  
    #endregion

    #region Enemy Implementation
    private void Awake() {
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        //Physics2D.IgnoreCollision(LevelInterface.Instance.Player.GetComponent<Collider2D>(), GetComponentsInChildren<Collider2D>()[0]);
    }

    private void Update() {
        // Update the monster : move, do action if player detected, attack if in range
        Move();
        if (DetectInRange(_DetectionRange)) PlayerDetected(); 
        if (DetectInRange(_AttackRange)) Attack();
    }
    #endregion

    #region Enemy methods
    protected bool FacingPlayer() {
        return (LevelInterface.Instance.PlayerGfx.position.x > transform.position.x && transform.forward == Vector3.right) || (LevelInterface.Instance.PlayerGfx.position.x < transform.position.x && transform.forward == Vector3.left);
    }

    protected void FacePlayer() {
        transform.Rotate(0, -180, 0);
    }

    public void TakeDamage(int damage) {
        // Taking damage from the player
        _Health -= damage;
        if (_Health <= 0) Die();
    }

    // What to do when dying
    protected void Die() {
        //todo: Start death animation
        Destroy(gameObject);
    }

    protected void Jump(float jumpHeight, float forward) {
        JumpForward(jumpHeight, forward);
    }

    void JumpForward(float jumpHeight, float forward) {
        //todo: change horizontal input
        float gravity = Physics2D.gravity.y * _Rigidbody2D.gravityScale;
        float verticalJumpForce = Mathf.Sqrt(-2 * gravity * jumpHeight);
        float horizontalJumpForce = Mathf.Sqrt(-2 * gravity * (forward/2));
        _Rigidbody2D.AddForce(new Vector2(horizontalJumpForce, verticalJumpForce), ForceMode2D.Impulse);
    }

    protected void GoForward(float speed) {
        _Rigidbody2D.AddForce(new Vector2(speed - _Rigidbody2D.velocity.x, 0) * transform.forward, ForceMode.VelocityChange);
        _Rigidbody2D.angularVelocity = 0;
        _Rigidbody2D.MoveRotation(0);
    }

    protected void ChargeForward(float speed) {
        _Rigidbody2D.AddForce(transform.forward * speed, ForceMode2D.Impulse);
    }

    public void ProjectileDestroyed(GameObject projectile) {
        _OnScreenProjectiles.Remove(projectile);
    }

    bool DetectInRange(float radius) { //todo: move in layers class
        Collider2D collider2D = Physics2D.OverlapCircle(transform.position /*+ Vector3.up * yOffset*/, radius / 2, Layers.Instance.PlayerLayerMask);
        return collider2D != null;
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision) {
        if (Layers.Instance.IsPlayer(collision.collider)) print("ok");//LevelInterface.Instance.Player.GetComponent<Player>().TakeDamage(); 
        //todo: fix
    }

    #region Abstract methods
    protected abstract void PlayerDetected(); // What to do if the player is detected

    protected abstract void Attack(); // How to attack if in range

    protected abstract void Move(); // Movement when player is not around
    #endregion

    #region Getter & Setters
    public int Score { get { return _Score; } }
    #endregion
}