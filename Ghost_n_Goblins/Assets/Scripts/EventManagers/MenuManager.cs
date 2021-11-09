namespace GhostsnGoblins {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using SDD.Events;

    public class MenuManager : Manager<MenuManager> {

        [Header("MenuManager")]

        #region Panels
        [Header("Panels")]
        [SerializeField] GameObject _MainMenuPanel;
        [SerializeField] GameObject _PausePanel;
        [SerializeField] GameObject _GameOverPanel;
        [SerializeField] GameObject _VictoryPanel;

        List<GameObject> _AllPanels;
        #endregion

        #region Events' subscription
        public override void SubscribeEvents() {
            base.SubscribeEvents();
        }

        public override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
        }
        #endregion

        #region Manager implementation
        protected override IEnumerator InitCoroutine() {
            yield break;
        }
        #endregion

        #region Monobehaviour lifecycle
        protected override void Awake() {
            base.Awake();
            RegisterPanels();
        }

        private void Update() {
            if (Input.GetButtonDown("Cancel")) { //ptete changer a escape?
                EscapeButtonHasBeenClicked();
            }
        }
        #endregion

        #region Panel Methods
        void RegisterPanels() {
            _AllPanels = new List<GameObject>();
            _AllPanels.Add(_MainMenuPanel);
            _AllPanels.Add(_PausePanel);
            _AllPanels.Add(_GameOverPanel);
            _AllPanels.Add(_VictoryPanel);
        }

        void OpenPanel(GameObject panel) {
            foreach (var item in _AllPanels)
                if (item) item.SetActive(item == panel);
        }
        #endregion

        #region UI OnClick Events
        public void EscapeButtonHasBeenClicked() {
            EventManager.Instance.Raise(new EscapeButtonClickedEvent());
        }

        public void PlayButtonHasBeenClicked() {
            EventManager.Instance.Raise(new PlayButtonClickedEvent());
        }

        public void ResumeButtonHasBeenClicked() {
            EventManager.Instance.Raise(new ResumeButtonClickedEvent());
        }

        public void MainMenuButtonHasBeenClicked() {
            EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
        }

        public void QuitButtonHasBeenClicked() {
            EventManager.Instance.Raise(new QuitButtonClickedEvent());
        }

        #endregion

        #region Callbacks to GameManager events
        protected override void GameMenu(GameMenuEvent e) {
            OpenPanel(_MainMenuPanel);
        }

        protected override void GamePlay(GamePlayEvent e) {
            OpenPanel(null);
        }

        protected override void GamePause(GamePauseEvent e) {
            OpenPanel(_PausePanel);
        }

        protected override void GameResume(GameResumeEvent e) {
            OpenPanel(null);
        }

        protected override void GameOver(GameOverEvent e) {
            OpenPanel(_GameOverPanel);
        }

        protected override void GameVictory(GameVictoryEvent e) {
            OpenPanel(_VictoryPanel);
        }
        #endregion
    }
}
