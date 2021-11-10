namespace GhostsnGoblins {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class Range : MonoBehaviour {
        [Tooltip("Range if this range (in meters")]
        [SerializeField] int _Range;

        // The enemy this range is attached to
        protected Enemy _Enemy;

        #region Range Implementation
        private void Awake() {
            GetComponent<CircleCollider2D>().radius = _Range;
        }
        #endregion

        #region Collisions Methods
        private void OnCollisionEnter2D(Collision2D collision) {
            // Return true if the player enter the range
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null) InRange(true);
        }

        private void OnCollisionExit2D(Collision2D collision) {
            // Return false if the player exit the range
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null) InRange(false);
        }
        #endregion

        #region Abstract Methods
        protected abstract void InRange(bool b);
        #endregion

        #region Getters and Setters
        public Enemy SetEnemy { set { _Enemy = value; } }
        #endregion
    }
}
