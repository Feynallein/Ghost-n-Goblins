namespace GhostsnGoblins {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using SDD.Events;

    [RequireComponent(typeof(Moving))]
    public class Player : SimpleGameStateObserver {
        [SerializeField] List<Behaviour> _Behaviours;
        Moving MovingScript;

        protected override void Awake() {
            base.Awake();
            MovingScript = GetComponent<Moving>();
        }

        void ActivatePlayer(bool active) {
            gameObject.SetActive(active);
            ActivateBehaviour(active);
            SetRigidbodyKinematic(!active);
        }

        void SetRigidbodyKinematic(bool isKinematic) {
            MovingScript.RigidbodyIsKinematic = isKinematic;
        }

        void ActivateBehaviour(bool active) {
            foreach (var c in _Behaviours) c.enabled = active;
        }

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
            if(key != null) {
                //TEMP
                EventManager.Instance.Raise(new GameVictoryEvent());
            }
        }

        private void OnCollisionExit2D(Collision2D collision) {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
                MovingScript.IsGrounded = false;
        }

        #endregion
    }
}
