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

    [SerializeField] public GameObject _AttackRange;
    [SerializeField] public GameObject _DetectionRange;

    [Tooltip("Monster's projectile spawn point")]
    [SerializeField] protected Transform _ProjectileSpawnPoint;

    // If the monster can attack
    bool _CanAttack;

    // If the player is in range
    bool _PlayerDected;

    //The player
    protected Transform _Player;

    List<GameObject> _OnScreenProjectiles = new List<GameObject>();
    #endregion

    #region Enemy Implementation
    protected override void Awake() {
        base.Awake();
        // Set this enemy for each ranges
        foreach (Range r in transform.GetComponentsInChildren<Range>()) {
            r.SetEnemy = this;
        }
    }

    private void Update() {
        // Update the monster : move, do action if player detected, attack if in range
        Move();
        if (_PlayerDected) PlayerDetected();
        if (_CanAttack) Attack();
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
        newProjectile.transform.LookAt(_Player);
        newProjectile.GetComponent<MonsterProjectile>().Speed = projectileSpeed;
        newProjectile.GetComponent<MonsterProjectile>().Enemy = this;
        _OnScreenProjectiles.Add(newProjectile);
    }

    public void ProjectileDestroyed(GameObject projectile) {
        _OnScreenProjectiles.Remove(projectile);
    }
    #endregion

    #region Event's Callbacks
    protected override void LevelReady(LevelReadyEvent e) {
        SetPlayer(e.ePlayer);
    }
    #endregion

    #region Abstract methods
    protected abstract void PlayerDetected(); // What to do if the player is detected

    protected abstract void Attack(); // How to attack if in range

    protected abstract void Move(); // Movement when player is not around
    #endregion

    #region Getter & Setters
    public bool CanAttack { set { _CanAttack = value; } }

    public bool HasDetectedPlayer { set { _PlayerDected = value; } }

    public int Score { get { return _Score; } }
    #endregion
}