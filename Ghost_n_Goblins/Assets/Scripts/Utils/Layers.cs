using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layers : MonoBehaviour {
    [SerializeField] public LayerMask GroundLayerMask;
    [SerializeField] public LayerMask LadderLayerMask;
    [SerializeField] public LayerMask MovingPlateformLayerMask;
    [SerializeField] public LayerMask PlayerLayerMask;
    [SerializeField] public LayerMask DestroyProjectile;

    static Layers _Instance;

    public static Layers Instance { get { return _Instance; } }

    private void Awake() {
        if (_Instance != null) Destroy(gameObject);
        else _Instance = this;
    }

    // Return true if the collided layer is within selected layer mask
    public bool CheckIfCollidedLayerIsSelectedLayer(LayerMask selectedLayer, LayerMask collidedLayer) {
        return (selectedLayer & (1 << collidedLayer)) > 0;
    }

    // Return true if the collided layer is within MovingPlateformLayerMask
    public bool CheckIfCollidedLayerIsMovingPlateform(LayerMask collidedLayer) {
        return CheckIfCollidedLayerIsSelectedLayer(MovingPlateformLayerMask, collidedLayer);
    }

    // Return true if monster is facing player within the range
    public bool IsFacingPlayer(Transform monster, float range, float yOffset = 0f) {
        return RayCastToLayer(monster.position + Vector3.up * yOffset, monster.right + Vector3.up * yOffset, range, PlayerLayerMask);
    }

    // Return true if the raycast (length = range) hit an object of the targeted layer
    public bool RayCastToLayer(Vector3 position, Vector3 direction, float range, LayerMask targetedLayer) {
        return Physics2D.Raycast(position, direction, range, targetedLayer).collider != null;
    }

    // Return truc if collider collided with something on the layer mask
    public bool BoxCastToLayer(Collider2D collider, LayerMask targetedLayer) {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .2f, targetedLayer).collider != null;
    }

    // Return true if the collider is grounded
    public bool IsGrounded(Collider2D collider) {
        return BoxCastToLayer(collider, GroundLayerMask);
    }

    // Return true if the collider is on a ladder
    public bool IsOnLadder(Collider2D collider) {
        return BoxCastToLayer(collider, LadderLayerMask);
    }

    // Return true if the collider collided with a layers that destroyes projectile
    public bool IsDestroyedByTerrain(Collider2D collider) {
        return CheckIfCollidedLayerIsSelectedLayer(DestroyProjectile, collider.gameObject.layer);
    }

    // Return true if hitting the player
    public bool IsPlayer(Collider2D collider) {
        return CheckIfCollidedLayerIsSelectedLayer(PlayerLayerMask, collider.gameObject.layer);
    }
}
