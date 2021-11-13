using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System;
using EventsManager;

// Enum of the different types of weapons
[Serializable] public enum Weapon { Lance, Dagger, Torch, Axe, Shield }

[RequireComponent(typeof(Moving))]
[RequireComponent(typeof(Shoot))]
public class Player : SimpleGameStateObserver {
    [Tooltip("List of player's behaviours")]
    [SerializeField] List<Behaviour> _Behaviours;
    Moving MovingScript;
    Shoot ShootingScript;

    int currentWeapon = 0; //temporary

    #region Player Implementation

    //TEMPORARY
    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) EventManager.Instance.Raise(new WeaponSwapEvent() { eWeapon = (Weapon)(++currentWeapon % 5) });
    }
    // END OF TEMPORARY

    protected override void Awake() {
        base.Awake();
        MovingScript = GetComponent<Moving>();
        ShootingScript = GetComponent<Shoot>();
    }
    #endregion

    #region Player methods
    void ActivatePlayer(bool active) {
        gameObject.SetActive(active);
        ActivateBehaviour(active);
        SetRigidbodyKinematic(!active);
    }

    public void TakeDamage() {
        //todo: perdre l'armure puis perdre un coeur & recommencer au debut du niveau
        Debug.Log("damage");
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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            MovingScript.IsGrounded = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        IScore score = collision.gameObject.GetComponent<IScore>();
        Key key = collision.gameObject.GetComponent<Key>();
        if (score != null) {
            EventManager.Instance.Raise(new ScoreItemEvent() { eScore = score.Score });
            Destroy(collision.gameObject);
        }
        if (key != null) {
            //TEMP
            EventManager.Instance.Raise(new GameVictoryEvent());
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        //LADDERS STUFF
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            MovingScript.IsGrounded = false;
    }

    #endregion
}