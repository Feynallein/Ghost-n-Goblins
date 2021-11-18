using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventsManager;

public abstract class Enemy : SimpleGameStateObserver, IScore {
    #region Variables
    [Header("Stats")]
    [Tooltip("Score value when killed")]
    [SerializeField] int _Score;

    [Tooltip("Monster's health")]
    [SerializeField] int _Health;

    [Tooltip("If the monster loots a pot")]
    [SerializeField] bool _IsLooter;

    [Tooltip("Monster's projectile spawn point")]
    [SerializeField] protected Transform _ProjectileSpawnPoint;


    //Todo tooltips
    [SerializeField] float _AttackRange;
    [SerializeField] float _DetectionRange;

    //The player
    protected Transform _Player;

    List<GameObject> _OnScreenProjectiles = new List<GameObject>();

    Rigidbody2D _Rigidbody2D;
    #endregion

    #region Enemy Implementation
    protected override void Awake() {
        base.Awake();
        _Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        // Update the monster : move, do action if player detected, attack if in range
        Move();
        if (DetectInRange(_DetectionRange)) PlayerDetected();
        if (DetectInRange(_AttackRange)) Attack();
    }
    #endregion

    #region Enemy methods
    public void TakeDamage(int damage) {
        // Taking damage from the player
        _Health -= damage;
        if (_Health <= 0) Die();
    }

    void Die() {
        // What to do when dying

        //todo: Start death animation
        if (_IsLooter) Loot();
        Destroy(gameObject);
    }

    void Loot() {
        //todo : loot a pot
    }

    void SetPlayer(Transform player) {
        _Player = player;
    }

    protected void ShootAtPlayer(GameObject projectile, float projectileSpeed, Transform projectileSpawnPoint, int numberOfProjectileOnScreen) {
        if (_OnScreenProjectiles.Count > numberOfProjectileOnScreen) return;
        GameObject newProjectile = Instantiate(projectile, projectileSpawnPoint);
        Physics2D.IgnoreCollision(newProjectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        newProjectile.transform.LookAt(_Player);
        newProjectile.GetComponent<MonsterProjectile>().Speed = projectileSpeed;
        newProjectile.GetComponent<MonsterProjectile>().Enemy = this;
        _OnScreenProjectiles.Add(newProjectile);
    }

    protected void JumpTowardPlayer(float jumpHeight, float forward) {
        JumpForward(jumpHeight, Layers.Instance.IsFacingPlayer(transform, _AttackRange) ? forward : -forward);
    }

    void JumpForward(float jumpHeight, float forward) {
        float gravity = Physics2D.gravity.y * _Rigidbody2D.gravityScale;
        float verticalJumpForce = Mathf.Sqrt(-2 * gravity * jumpHeight);
        float horizontalJumpForce = Mathf.Sqrt(-2 * gravity * forward);
        _Rigidbody2D.AddForce(new Vector2(horizontalJumpForce, verticalJumpForce), ForceMode2D.Impulse);
    }

    public void ProjectileDestroyed(GameObject projectile) {
        _OnScreenProjectiles.Remove(projectile);
    }
    #endregion

    #region Event's Callbacks
    protected override void LevelReady(LevelReadyEvent e) {
        SetPlayer(e.ePlayer);
    }

    bool DetectInRange(float radius) {
        Collider2D collider2D = Physics2D.OverlapCircle(transform.position, radius, Layers.Instance.PlayerLayerMask);
        return collider2D != null;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, _AttackRange);
    }
    #endregion

    #region Abstract methods
    protected abstract void PlayerDetected(); // What to do if the player is detected

    protected abstract void Attack(); // How to attack if in range

    protected abstract void Move(); // Movement when player is not around
    #endregion

    #region Getter & Setters
    public int Score { get { return _Score; } }
    #endregion
}