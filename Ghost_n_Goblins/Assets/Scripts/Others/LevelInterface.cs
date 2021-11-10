namespace GhostsnGoblins {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using SDD.Events;

    public class LevelInterface : SimpleGameStateObserver {
        public GameObject _Background;
        public Transform _MapBeginning;
        public Transform _MapEnding;
        public Transform _PlayerSpawnPoint;
        public Transform _Key;

        public override void SubscribeEvents() {
            base.SubscribeEvents();
            EventManager.Instance.AddListener<SceneLoadedEvent>(SceneLoaded);
        }

        public override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            EventManager.Instance.RemoveListener<SceneLoadedEvent>(SceneLoaded);
        }

        void SceneLoaded(SceneLoadedEvent e) {
            EventManager.Instance.Raise(new LevelReadyEvent() {
                eBackground = _Background,
                eMapBeginning = _MapBeginning,
                eMapEnding = _MapEnding,
                ePlayerSpawnPoint = _PlayerSpawnPoint,
                eKey = _Key,
            });
        }
    }
}
