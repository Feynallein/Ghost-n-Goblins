using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy {
    [SerializeField] float _MovementSpeed;

    private void Start() {
        Physics2D.IgnoreLayerCollision(9, 7); //Enemy & obstacle
    }

    protected override void Attack() { //No Attack only run & damage if hit the player
    }

    protected override void Move() { // No idle move
    }

    protected override void PlayerDetected() {
        if (!FacingPlayer()) FacePlayer();

        GoForward(_MovementSpeed);
    }
}
