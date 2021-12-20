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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (Layers.Instance.IsDestroyedByTerrain(collision.collider) || Layers.Instance.IsPlayer(collision.collider)) Die();
    }

    protected override void PlayerDetected() {
        if (_Charged) return;
        if (!FacingPlayer()) FacePlayer();
        transform.LookAt(LevelInterface.Instance.PlayerGfx);
        ChargeForward(_MovementSpeed);
        _Charged = true;
        Destroy(gameObject, _DestroyTimer);
    }
}
