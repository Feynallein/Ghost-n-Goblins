namespace GhostsnGoblins {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class DetectionRange : Range {
        private void Start() {
            _Enemy._DetectionRange.GetComponent<CircleCollider2D>().radius = _Range;
        }

        protected override void InRange(bool b) {
            // Player has been detected
            _Enemy.HasDetectedPlayer = b;
        }
    }
}
