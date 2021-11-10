namespace GhostsnGoblins {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(AttackRange))]
    [RequireComponent(typeof(DetectionRange))]
    public abstract class Enemy : MonoBehaviour, IScore {
        #region Variables
        [Header("Stats")]
        [Tooltip("Score value when killed")]
        [SerializeField] int _Score;

        [Tooltip("Monster's health")]
        [SerializeField] int _Health;

        [Tooltip("Monster's damage")]
        [SerializeField] int _DamageDealt;

        [Tooltip("If the monster loots a pot")]
        [SerializeField] bool _IsLooter;

        [SerializeField] public GameObject _AttackRange;
        [SerializeField] public GameObject _DetectionRange;

        // If the monster can attack
        bool _CanAttack;

        // If the player is in range
        bool _PlayerDected;
        #endregion

        #region Enemy Implementation
        private void Awake() {
            // Set this enemy for each ranges
            foreach (Range r in GetComponents<Range>()) {
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
        #endregion

        #region Abstract methods
        protected abstract void PlayerDetected(); // What to do if the player is detected

        protected abstract void Attack(); // How to attack if in range

        protected abstract void Move(); // Movement when player is not around
        #endregion

        #region Getter & Setters
        public bool CanAttack { get; set; }

        public bool HasDetectedPlayer { get; set; }

        public int Score { get { return _Score; } }
        #endregion
    }
}
