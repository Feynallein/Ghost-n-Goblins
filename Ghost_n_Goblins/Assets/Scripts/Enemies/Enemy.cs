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

    [Tooltip("If the monster loots a pot")]
    [SerializeField] bool _IsLooter;

    [Tooltip("Monster's projectile spawn point")]
    [SerializeField] protected Transform _ProjectileSpawnPoint;


    //Todo tooltips
    [SerializeField] protected float _AttackRange;
    [SerializeField] protected float _DetectionRange;

    List<GameObject> _OnScreenProjectiles = new List<GameObject>();

    protected Rigidbody2D _Rigidbody2D;

    protected float yOffset = 1.5f; //TMP: 1.5 = (gfx) scale.y /2 
    #endregion

    //TMP: faudra p'tet changer tt les prefabs pour que la facing dir soit forward, faudra adapter le code dans ce cas la (et comme ca pour face le player on peut faire transform.LookAt();

    #region Enemy Implementation
    private void Awake() {
        _Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        // Update the monster : move, do action if player detected, attack if in range
        Move();
        if (DetectInRange(_DetectionRange)) PlayerDetected();
        if (DetectInRange(_AttackRange)) Attack();
    }
    #endregion

    //TODO: every attack/movement methods that are used only once -> put in the specific class, those who are called multiple times stay here

    //Todo: each enemy has to face the player after detecting it + each enemy has to go THROUGH the player (ie no collisions but still damages)
    // (enemies' collider = trigger?)

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

    protected void ShootAtPlayer(GameObject projectile, float projectileSpeed, Transform projectileSpawnPoint, int numberOfProjectileOnScreen) {
        if (_OnScreenProjectiles.Count > numberOfProjectileOnScreen) return;
        GameObject newProjectile = Instantiate(projectile, projectileSpawnPoint);
        Physics2D.IgnoreCollision(newProjectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        newProjectile.transform.LookAt(LevelInterface.Instance.Player);
        newProjectile.GetComponent<MonsterProjectile>().Speed = projectileSpeed;
        newProjectile.GetComponent<MonsterProjectile>().Enemy = this;
        _OnScreenProjectiles.Add(newProjectile);
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

    protected void RunTowardPlayer(float speed) {
        _Rigidbody2D.AddForce(new Vector2(speed - _Rigidbody2D.velocity.x, 0), ForceMode.VelocityChange);
        _Rigidbody2D.angularVelocity = 0;
        _Rigidbody2D.MoveRotation(0);
    }

    protected void ChargeTowardPlayer(float speed) {
        _Rigidbody2D.AddForce(transform.forward * speed, ForceMode2D.Impulse);
    }

    public void ProjectileDestroyed(GameObject projectile) {
        _OnScreenProjectiles.Remove(projectile);
    }
    #endregion

    #region Event's Callbacks

    bool DetectInRange(float radius) { //TMP: to move in layers class
        Collider2D collider2D = Physics2D.OverlapCircle(transform.position + Vector3.up * yOffset, radius/2, Layers.Instance.PlayerLayerMask);
        return collider2D != null;
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