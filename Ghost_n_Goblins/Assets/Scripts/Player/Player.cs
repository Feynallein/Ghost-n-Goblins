using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System;
using EventsManager;

// Enum of the different types of weapons
[Serializable] public enum Weapon { Lance, Dagger, Torch, Axe, Shield }

[RequireComponent(typeof(Move))]
[RequireComponent(typeof(Shoot))]
public class Player : SimpleGameStateObserver {
    [Tooltip("List of player's behaviours")]
    [SerializeField] List<Behaviour> _Behaviours;
    Move MovingScript;
    Shoot ShootingScript;
    bool _isInvicible;
    [SerializeField] int _InvicibilityDuration;
    [SerializeField] float _BlinkDuration;

    int currentWeapon = 0; //temporary
    MeshRenderer _MeshRenderer;

    bool _HasArmor = true;

    #region Player Implementation

    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) EventManager.Instance.Raise(new WeaponSwapEvent() { eWeapon = (Weapon)(++currentWeapon % 5) });
        if (Input.GetKeyDown(KeyCode.L)) EventManager.Instance.Raise(new DieEvent());
        if (Input.GetKeyDown(KeyCode.M)) EventManager.Instance.Raise(new GainLifeEvent());
        if (Input.GetKeyDown(KeyCode.I)) _isInvicible = !_isInvicible;
    }

    protected override void Awake() {
        base.Awake();
        MovingScript = GetComponent<Move>();
        ShootingScript = GetComponent<Shoot>();
        _MeshRenderer = GetComponentInChildren<MeshRenderer>();
    }
    #endregion

    #region Player methods
    void ActivatePlayer(bool active) {
        gameObject.SetActive(active);
        ActivateBehaviour(active);
        SetRigidbodyKinematic(!active);
    }

    public void TakeDamage() {
        if (!_isInvicible) {
            if (_HasArmor) LoseArmor();
            else Die();
            _isInvicible = true;
            StartCoroutine(DamageCoroutine());
        }
    }

    void LoseArmor() {
        _HasArmor = false;
        //todo: just lose armor: component remove (todo when the playerh as armour)
    }

    void Die() {
        EventManager.Instance.Raise(new DieEvent());
    }

    IEnumerator DamageCoroutine() {
        float duration = 0;
        while(duration < _InvicibilityDuration) {
            _MeshRenderer.enabled = !_MeshRenderer.enabled;
            duration += _BlinkDuration;
            yield return new WaitForSeconds(_BlinkDuration);
        }
        _isInvicible = false;
    }

    void SetRigidbodyKinematic(bool isKinematic) {
        MovingScript.RigidbodyIsKinematic = isKinematic;
    }

    void ActivateBehaviour(bool active) {
        foreach (var c in _Behaviours) c.enabled = active;
    }

    void ChangeCurrentWeapon(Weapon weapon) {
        ShootingScript.CurrentWeapon = weapon;
    }
    #endregion

    #region Event's subscription
    public override void SubscribeEvents() {
        base.SubscribeEvents();
        EventManager.Instance.AddListener<WeaponSwapEvent>(WeaponSwap);
    }

    public override void UnsubscribeEvents() {
        base.UnsubscribeEvents();
        EventManager.Instance.RemoveListener<WeaponSwapEvent>(WeaponSwap);
    }
    #endregion

    #region Callbacks to Events
    protected override void GamePlay(GamePlayEvent e) {
        ActivatePlayer(true);
    }

    protected override void GameMenu(GameMenuEvent e) {
        ActivatePlayer(false);
    }

    protected override void GameOver(GameOverEvent e) {
        ActivatePlayer(false);
    }

    protected override void GameVictory(GameVictoryEvent e) {
        ActivatePlayer(false);
    }

    protected override void LevelReady(LevelReadyEvent e) {
        MovingScript.SetPositionAndMapBounds(e.ePlayerSpawnPoint.position, e.eMapBeginning, e.eMapEnding);
    }

    void WeaponSwap(WeaponSwapEvent e) {
        ChangeCurrentWeapon(e.eWeapon);
    }
    #endregion

    #region Collision stuff
    private void OnTriggerEnter2D(Collider2D collision) {
        IScore score = collision.gameObject.GetComponent<IScore>();
        Key key = collision.gameObject.GetComponent<Key>();

        if (score != null) {
            EventManager.Instance.Raise(new ScoreItemEvent() { eScore = score.Score });
            Destroy(collision.gameObject);
        }

        if (key != null) {
            EventManager.Instance.Raise(new GameVictoryEvent());
        }

        if (Layers.Instance.Drowned(collision.gameObject.layer)) {
            Die();
            StartCoroutine(DamageCoroutine());
        }
    }
    #endregion
}