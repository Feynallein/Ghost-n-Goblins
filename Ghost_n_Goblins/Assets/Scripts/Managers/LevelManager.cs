namespace EventsManager {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;
    using SDD.Events;
    using UnityEngine.SceneManagement;
    using System;
    using Kryz.Tweening;

    public class LevelManager : Manager<LevelManager> {
        #region Variables
        [Tooltip("Key drop's duration in s")]
        [SerializeField] float _DropDuration;

        [SerializeField] Transform _Player;

        // Boss of this level
        GameObject _Boss;

        // The key in the level
        Transform _Key;

        // Delegate to easing functions
        delegate float EasingFuntionDelegate(float t);
        #endregion

        #region Manager implementation
        protected override IEnumerator InitCoroutine() {
            yield break; // Nothing
        }

        //TEMPORARY
        private void Update() {
            if (GameManager.Instance.IsPlaying && _Boss == null) EventManager.Instance.Raise(new BossKilledEvent());
        }
        // END OF TEMPORARY
        #endregion

        #region Event's Subscription
        public override void SubscribeEvents() {
            base.SubscribeEvents();
            EventManager.Instance.AddListener<BossKilledEvent>(BossKilled);
        }

        public override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            EventManager.Instance.RemoveListener<BossKilledEvent>(BossKilled);
        }
        #endregion

        #region LevelManager Methods
        void SetKeyAndBoss(Transform key, GameObject boss) {
            _Key = key; // Set the key
            _Boss = boss;
        }

        IEnumerator LoadingAsyncScene(string name) {
            // Loading asynchronously a scene
            AsyncOperation load = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            while (!load.isDone) yield return null; // loading screen would be here
            EventManager.Instance.Raise(new SceneLoadedEvent() {
                ePlayer = _Player,
            });
        }

        // Dropping the key with a coroutine with easing function to have bouncing effect
        IEnumerator DropKey(float duration, Vector3 startPoint, Vector3 endPoint, EasingFuntionDelegate easingFuntion, Action startAction = null, Action endAction = null) {
            if (startAction != null) startAction(); //todo (start sfx)
            float elapsedTime = 0f;
            while (elapsedTime < duration) {
                float k = elapsedTime / duration;
                _Key.position = Vector3.Lerp(startPoint, endPoint, easingFuntion(k));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = endPoint;

            if (endAction != null) endAction(); //todo: SFX + particles
        }
        #endregion

        #region Event's Callbacks
        protected override void GameInitializeLevel(GameInitializeLevelEvent e) {
            StartCoroutine(LoadingAsyncScene(e.eSceneName));
        }

        void BossKilled(BossKilledEvent e) {
            StartCoroutine(DropKey(_DropDuration, _Key.position, new Vector3(_Key.position.x, 0.27f, _Key.position.z), EasingFunctions.OutBounce));
        }

        protected override void LevelReady(LevelReadyEvent e) {
            SetKeyAndBoss(e.eKey, e.eBoss);
        }

        protected override void GameMenu(GameMenuEvent e) {
            // Removing useless scenes
            Scene[] openScenes = GetOpenScenes();
            foreach(Scene s in openScenes) {
                if (s.name != "Game") SceneManager.UnloadSceneAsync(s);
            }
        }
        #endregion

        Scene[] GetOpenScenes() {
            int numberOfScenes = SceneManager.sceneCount;
            Scene[] openScenes = new Scene[numberOfScenes];

            for (int i = 0; i < numberOfScenes; i++) {
                openScenes[i] = SceneManager.GetSceneAt(i);
            }

            return openScenes;
        }
    }
}