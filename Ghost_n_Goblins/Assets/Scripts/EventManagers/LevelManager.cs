namespace GhostsnGoblins {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;
    using SDD.Events;
    using UnityEngine.SceneManagement;
    using System;
    using Kryz.Tweening;

    public class LevelManager : Manager<LevelManager> {
        [Tooltip("Key drop's duration in s")]
        [SerializeField] float _DropDuration;
        Transform Key;

        delegate float EasingFuntionDelegate(float t);

        #region Manager implementation
        protected override IEnumerator InitCoroutine() {
            yield break;
        }
        #endregion

        //TEMPORARY
        private void Update() {
            if (Input.GetKeyDown(KeyCode.V)) EventManager.Instance.Raise(new BossKilledEvent());
        }
        // END OF TEMPORARY

        public override void SubscribeEvents() {
            base.SubscribeEvents();
            EventManager.Instance.AddListener<BossKilledEvent>(BossKilled);

        }

        public override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            EventManager.Instance.RemoveListener<BossKilledEvent>(BossKilled);

        }

        IEnumerator LoadingAsyncScene(string name) {
            AsyncOperation load = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            while (!load.isDone) yield return null; // potentiel temps de chargement (si c'était + long mdr)
            EventManager.Instance.Raise(new LevelReadyEvent() {
                eBackground = GameObject.Find("Background"),
                eMapBeginning = GameObject.Find("MapBeginning").transform,
                eMapEnding = GameObject.Find("MapEnd").transform,
                ePlayerSpawnPoint = GameObject.Find("PlayerSpawnPoint").transform,
            });
            Key = GameObject.Find("KeyPref").transform;
        }

        IEnumerator DropKey(float duration, Vector3 startPoint, Vector3 endPoint, EasingFuntionDelegate easingFuntion, Action startAction = null, Action endAction = null) {
            if (startAction != null) startAction(); //todo (lancer le sfx)
            float elapsedTime = 0f;
            while(elapsedTime < duration) {
                float k = elapsedTime / duration;
                Key.position = Vector3.Lerp(startPoint, endPoint, easingFuntion(k));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = endPoint;

            if (endAction != null) endAction(); //todo: SFX + particles
        }

        protected override void GameInitializeLevel(GameInitializeLevelEvent e) {
            StartCoroutine(LoadingAsyncScene(e.eSceneName));
        }

        void BossKilled(BossKilledEvent e) {
            StartCoroutine(DropKey(_DropDuration, Key.position, new Vector3(Key.position.x, 0.27f, Key.position.z), EasingFunctions.OutBounce));
        }
    }
}