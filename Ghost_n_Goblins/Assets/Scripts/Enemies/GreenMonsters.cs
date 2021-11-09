using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMonsters : Enemy {
    [SerializeField] GameObject GreenMonsterProjectilePrefab;
    void Update() {
        // If player is in shooting range
        Attack();
    }

    protected override void Loot() { // Empty: not a pot bearer
    }

    protected override void Attack() {
        // Lauch a projectil at the player
    }

    protected override void Move() { // Empty: they don't move
    }
}
