using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using EventsManager;

    public class LevelInterface : SingletonGameStateObserver<LevelInterface> {
    [SerializeField] private GameObject _Background;
    [SerializeField] private Transform _MapBeginning;
    [SerializeField] private Transform _MapEnding;
    [SerializeField] private Transform _PlayerSpawnPoint;
    [SerializeField] private Transform _Key;
    [SerializeField] private GameObject _Boss;

    public Transform Player;

    public override void SubscribeEvents() {
        base.SubscribeEvents();
        EventManager.Instance.AddListener<SceneLoadedEvent>(SceneLoaded);
    }

    public override void UnsubscribeEvents() {
        base.UnsubscribeEvents();
        EventManager.Instance.RemoveListener<SceneLoadedEvent>(SceneLoaded);
    }

    void SceneLoaded(SceneLoadedEvent e) {
        Player = e.ePlayer;
        EventManager.Instance.Raise(new LevelReadyEvent() {
            eBackground = _Background,
            eMapBeginning = _MapBeginning,
            eMapEnding = _MapEnding,
            ePlayerSpawnPoint = _PlayerSpawnPoint,
            eKey = _Key,
            ePlayer = e.ePlayer,
            eBoss = _Boss,
        });
    }
}