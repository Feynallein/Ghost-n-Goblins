using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy {
    [SerializeField] float _MovementSpeed;

    protected override void Attack() { //No Attack only run & damage if hit the player
    }

    protected override void Move() { // No idle move
    }

    protected override void PlayerDetected() {
        GoForward(_MovementSpeed);
        //todo: les faire sauter un obstacle: raycast pour detecter un obstacle + saut
    }
}
