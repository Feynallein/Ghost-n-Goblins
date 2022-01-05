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

    [SerializeField] protected float _AttackRange;
    [SerializeField] protected float _DetectionRange;

    protected List<GameObject> _OnScreenProjectiles = new List<GameObject>();

    protected Rigidbody2D _Rigidbody2D;
    #endregion

    #region Enemy Implementation
    private void Awake() {
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(Layers.Instance.PlayerLayerMask, 9); // Player & Enemy
    }

    private void Update() {
        Move();
        if (DetectInRange(_DetectionRange)) PlayerDetected(); 
        if (DetectInRange(_AttackRange)) Attack();
    }
    #endregion

    #region Enemy methods
    protected bool FacingPlayer() {
        return (LevelInterface.Instance.PlayerGfx.position.x > transform.position.x && (int) transform.forward.x == 1) || (LevelInterface.Instance.PlayerGfx.position.x < transform.position.x && (int) transform.forward.x == -1);
    }

    protected void FacePlayer() {
        transform.Rotate(0, -180, 0);
    }

    public void TakeDamage(int damage) {
        _Health -= damage;
        if (_Health <= 0) Die();
    }

    protected void Die() {
        //todo: Start death animation
        Destroy(gameObject);
    }

    protected void GoForward(float speed) {
        _Rigidbody2D.AddForce(transform.forward * speed, ForceMode.Force);
        _Rigidbody2D.angularVelocity = 0;
        _Rigidbody2D.MoveRotation(0);
    }

    public void ProjectileDestroyed(GameObject projectile) {
        _OnScreenProjectiles.Remove(projectile);
    }

    bool DetectInRange(float radius) {
        Collider2D collider2D = Physics2D.OverlapCircle(transform.position, radius / 2, Layers.Instance.PlayerLayerMask);
        return collider2D != null;
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision) {
        if (Layers.Instance.IsPlayer(collision.collider)) LevelInterface.Instance.Player.GetComponent<Player>().TakeDamage(); 
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