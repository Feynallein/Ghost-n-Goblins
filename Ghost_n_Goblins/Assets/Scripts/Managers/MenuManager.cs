﻿namespace EventsManager {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using SDD.Events;
    using UnityEngine.UI;

    public class MenuManager : Manager<MenuManager> {
        #region Variables
        [Header("Panels")]
        [Tooltip("Panel displayed when in the main menu")]
        [SerializeField] GameObject _MainMenuPanel;

        [Tooltip("Panel displayed when in pause")]
        [SerializeField] GameObject _PausePanel;

        [Tooltip("Panel displayed when game over")]
        [SerializeField] GameObject _GameOverPanel;

        [Tooltip("Panel displayed when victory")]
        [SerializeField] GameObject _VictoryPanel;

        [SerializeField] TextAsset _HighScoreFile;
        [SerializeField] int _HighScoreTextWidth;

        // List of all panels
        List<GameObject> _AllPanels;
        #endregion

        #region Manager implementation
        protected override IEnumerator InitCoroutine() {
            yield break; // nothing
        }

        #endregion

        #region Monobehaviour lifecycle
        protected override void Awake() {
            // Register every panels
            base.Awake();
            RegisterPanels();
            //HighScore.GetHighScore(_HighScoreFile, _HighScoreTextWidth);
        }

        private void Update() {
            // Starting the game if in the main menu
            if (Input.GetMouseButtonDown(0) || Input.GetAxisRaw("Start") == 1) PlayButtonHasBeenClicked();

            // Restarting
            if (Input.GetAxisRaw("Start") == 1) PlayButtonHasBeenClicked();

            // Victory!!
            if (Input.GetAxisRaw("Jump") == 1) MainMenuButtonHasBeenClicked();

            // Returning to the main menu from other states
            if (Input.GetAxisRaw("Select") == 1) MainMenuButtonHasBeenClicked();

            // What to do if escape is pressed
            if (Input.GetButtonDown("Cancel")) EscapeButtonHasBeenClicked();
        }
        #endregion

        #region Panel Methods
        void RegisterPanels() {
            // Add every panels to the list
            _AllPanels = new List<GameObject>();
            _AllPanels.Add(_MainMenuPanel);
            _AllPanels.Add(_PausePanel);
            _AllPanels.Add(_GameOverPanel);
            _AllPanels.Add(_VictoryPanel);
        }

        void OpenPanel(GameObject panel) {
            // Open a specific panel
            foreach (var item in _AllPanels)
                if (item) item.SetActive(item == panel);
        }

        void SetNewHighScore(int newScore, string name) {
            HighScore.WriteHighScore(_HighScoreFile, newScore, name);
        }
        #endregion

        #region UI OnClick Events
        /* Raising correspondent event */
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
        /* Displaying correspondent panel */
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
            SetNewHighScore(GameManager.Instance.Score, "Joueur " + new System.Random().Next(100));
        }

        protected override void GameVictory(GameVictoryEvent e) {
            OpenPanel(_VictoryPanel);           
        }
        #endregion
    }
}
