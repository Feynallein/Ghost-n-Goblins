using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
    // Return truc if collider collided with something on the layer mask
    public static bool CollidedWithLayerMask(Collider2D collider, LayerMask layerMask) {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .2f, layerMask).collider != null;
    }

    // Return true if the collider is grounded
    public static bool IsGrounded(Collider2D collider) {
        return CollidedWithLayerMask(collider, Layers.Instance.GroundLayerMask);
    }

    // Return true if the collider is on a ladder
    public static bool IsOnLadder(Collider2D collider) {
        return CollidedWithLayerMask(collider, Layers.Instance.LadderLayerMask);
    }
}
