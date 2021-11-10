namespace GhostsnGoblins {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GreenMonsters : Enemy {
        [Header("Monster specifications")]
        [Tooltip("Monter's projectile prefab")]
        [SerializeField] GameObject GreenMonsterProjectilePrefab;

        protected override void Attack() {
            //todo
            // Lauch a projectile at the player
        }

        protected override void Move() { // Empty: this monster doesn't move
        }

        protected override void PlayerDetected() { // Empty: this monster has no reaction near player
        }
    }
}
