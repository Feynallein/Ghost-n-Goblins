using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layers : MonoBehaviour {
    [SerializeField] public LayerMask GroundLayerMask;
    [SerializeField] public LayerMask LadderLayerMask;
    [SerializeField] public LayerMask MovingPlateformLayerMask;
    [SerializeField] public LayerMask PlayerLayerMask;

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
    public bool IsFacingPlayer(Transform monster, float range) {
        return RayCastToLayer(monster.position, monster.forward, range, PlayerLayerMask);
    }

    // Return true if the raycast (length = range) hit an object of the targeted layer
    public bool RayCastToLayer(Vector3 position, Vector3 direction, float range, LayerMask targetedLayer) {
        return Physics2D.Raycast(position,direction, range, targetedLayer).collider != null;
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
}
