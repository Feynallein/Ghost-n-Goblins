namespace GhostsnGoblins {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class DetectionRange : Range {
        protected override void InRange(bool b) {
            // Player has been detected
            _Enemy.HasDetectedPlayer = b;
        }
    }
}
