using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy {
    protected override void Attack() { //No Attack only run & damage if hit the player
    }

    protected override void Move() { // No idle move
    }

    protected override void PlayerDetected() { 
        //todo: Run Toward Player
    }
}
