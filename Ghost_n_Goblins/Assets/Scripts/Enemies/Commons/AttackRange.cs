namespace GhostsnGoblins {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AttackRange : Range {
        protected override void InRange(bool b) {
            // Is in attack range
            _Enemy.CanAttack = b;
        }
    }
}
