using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goose : Enemy {
    [SerializeField] float _MovementSpeed;
    [SerializeField] float _DestroyTimer;
    bool _Charged = false;
    protected override void Attack() { // No attack they just launch themselves to the player
    }

    protected override void Move() { // No Idle move
    }

    protected override void PlayerDetected() {
        if (_Charged) return;
        transform.LookAt(LevelInterface.Instance.Player);
        ChargeTowardPlayer(_MovementSpeed);
        _Charged = true;
        Destroy(gameObject, _DestroyTimer);
    }
}
