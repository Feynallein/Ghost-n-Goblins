using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : Range {
    private void Start() {
        _Enemy._AttackRange.GetComponent<CircleCollider2D>().radius = _Range;
    }

    protected override void InRange(bool b) {
        // Is in attack range
        _Enemy.CanAttack = b;
    }
}