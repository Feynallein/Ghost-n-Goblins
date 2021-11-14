using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Range : MonoBehaviour {
    [Tooltip("Range if this range (in meters")]
    [SerializeField] protected int _Range;

    // The enemy this range is attached to
    protected Enemy _Enemy;

    #region Collisions Methods
    private void OnTriggerEnter2D(Collider2D collision) {
        // Return true if the player enter the range
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null) InRange(true);
    }

    private void OnTriggerExit2D(Collider2D collision) {
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

