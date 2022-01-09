namespace EventsManager {
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using SDD.Events;
    using System.Linq;


    // TODO: commenting

    public enum GameState { gameMenu, gamePlay, initializingLevel, gamePause, gameOver, gameVictory }

    public class GameManager : Manager<GameManager> {
        #region Game State
        private GameState _GameState;
        public bool IsPlaying { get { return _GameState == GameState.gamePlay; } }

        public bool isInMainMenu { get { return _GameState == GameState.gameMenu; } }

        public bool IsPausing { get { return _GameState == GameState.gamePause; } }

        public bool IsVictory { get { return _GameState == GameState.gameVictory; } }

        public bool IsGameOver { get { return _GameState == GameState.gameOver; } }
        #endregion

        #region Lives
        [Header("GameManager")]
        [SerializeField] private int _NStartLives;

        private int _NLives;
        public int NLives { get { return _NLives; } }

        void DecrementNLives(int decrement) {
            SetNLives(_NLives - decrement);
            if (_NLives <= 0) EventManager.Instance.Raise(new GameOverEvent());
        }

        void IncrementNLives(int decrement) {
            SetNLives(_NLives + decrement);
        }

        void SetNLives(int nLives) {
            _NLives = nLives;
            EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = _Score, eNLives = _NLives, eTimer = _Timer });
        }
        #endregion

        #region Score
        private int _Score;
        public int Score {
            get { return _Score; }
            set {
                _Score = value;
                BestScore = Mathf.Max(BestScore, value);
            }
        }

        public float BestScore {
            get { return PlayerPrefs.GetFloat("BEST_SCORE", 0); } //regarder pour gerer le best score!
            set { PlayerPrefs.SetFloat("BEST_SCORE", value); }
        }

        void IncrementScore(int increment) {
            SetScore(_Score + increment);
        }

        void SetScore(int score, bool raiseEvent = true) {
            Score = score;

            if (raiseEvent) EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = _Score, eNLives = _NLives, eTimer = _Timer });
        }
        #endregion

        #region Time
        [SerializeField] float _GameDuration;
        float _Timer;

        void SetTimeScale(float newTimeScale) {
            Time.timeScale = newTimeScale;
        }

        void SetTimer(float timer, bool raiseEvent = true) {
            _Timer = timer;
            if (raiseEvent) EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eScore = _Score, eNLives = _NLives, eTimer = _Timer });
        }
        #endregion

        #region Events' subscription
        public override void SubscribeEvents() {
            base.SubscribeEvents();

            //MainMenuManager
            EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
            EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
            EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
            EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
            EventManager.Instance.AddListener<QuitButtonClickedEvent>(QuitButtonClicked);

            //Score Item
            EventManager.Instance.AddListener<ScoreItemEvent>(ScoreHasBeenGained);

            // Other
            EventManager.Instance.AddListener<GainLifeEvent>(GainLife);
        }

        public override void UnsubscribeEvents() {
            base.UnsubscribeEvents();

            //MainMenuManager
            EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
            EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
            EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
            EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
            EventManager.Instance.RemoveListener<QuitButtonClickedEvent>(QuitButtonClicked);

            //Score Item
            EventManager.Instance.RemoveListener<ScoreItemEvent>(ScoreHasBeenGained);

            // Other
            EventManager.Instance.RemoveListener<GainLifeEvent>(GainLife);
        }
        #endregion

        #region Manager implementation
        protected override IEnumerator InitCoroutine() {
            Menu();
            yield break;
        }

        private void Update() {
            if (IsPlaying) {
                _Timer = Mathf.Max(0, _Timer - Time.deltaTime);
                EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eScore = _Score, eTimer = _Timer, eNLives = _NLives, eBestScore = BestScore });
                if (_Timer == 0 || _NLives == 0) Over();
            }
        }
        #endregion

        #region Game flow & Gameplay
        //Game initialization
        void InitNewGame(bool raiseStatsEvent = true) {
            SetScore(0);
            SetTimer(_GameDuration);
            SetNLives(_NStartLives);
        }
        #endregion

        #region Callbacks to events issued by Score items
        private void ScoreHasBeenGained(ScoreItemEvent e) {
            if (IsPlaying) IncrementScore(e.eScore);
            //play SFX from score gained
        }
        #endregion

        #region Callbacks to Events issued by MenuManager
        private void MainMenuButtonClicked(MainMenuButtonClickedEvent e) {
            if(IsPausing || IsVictory || IsGameOver) Menu();
        }

        private void PlayButtonClicked(PlayButtonClickedEvent e) {
            if(isInMainMenu || IsVictory || IsGameOver) InitializeLevel(true);
        }

        private void ResumeButtonClicked(ResumeButtonClickedEvent e) {
            Resume();
        }

        private void EscapeButtonClicked(EscapeButtonClickedEvent e) {
            if (IsPlaying) Pause();
            else if (IsPausing) EventManager.Instance.Raise(new ResumeButtonClickedEvent());
            else if (isInMainMenu) EventManager.Instance.Raise(new QuitButtonClickedEvent());
        }

        private void QuitButtonClicked(QuitButtonClickedEvent e) {
            Application.Quit();
        }

        protected override void LevelReady(LevelReadyEvent e) {
            if(e.isNewGame) InitNewGame(); // essentiellement pour que les statistiques du jeu soient mise à jour en HUD
            Play();
        }

        protected override void GameVictory(GameVictoryEvent e) {
            _GameState = GameState.gameVictory;
        }

        protected override void PlayerDied(DieEvent e) {
            _NLives--;
            //if(SfxManager.Instance) SfxManager.Instance.PlaySfx2D(Constants.GAMEOVER_SFX); => joue le SFX de mort
            InitializeLevel(false);
        }
        #endregion

        #region GameState methode
        private void GainLife(GainLifeEvent e) {
            SetNLives(_NLives + 1);
        }


        private void Menu() {
            SetTimeScale(1);
            _GameState = GameState.gameMenu;
            //if(MusicLoopsManager.Instance)MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC); => joue la musique du menu
            EventManager.Instance.Raise(new GameMenuEvent());
        }

        private void Play() {
            SetTimeScale(1);
            _GameState = GameState.gamePlay;
            //if (MusicLoopsManager.Instance) MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC); => joue la musique de jeu
            EventManager.Instance.Raise(new GamePlayEvent());
        }

        private void InitializeLevel(bool isNewGame) { //on devra rajouter un parametre ici qui prend en compte la scene
            _GameState = GameState.initializingLevel;
            string name = "Scenes/Level1"; // qui sera la variable name
            EventManager.Instance.Raise(new GameInitializeLevelEvent() { eSceneName = name, isNewGame = isNewGame}); // a remplacer par la scene choisi avec la surcouche de selection de niveau
        }

        private void Pause() {
            if (!IsPlaying) return;
            SetTimeScale(0);
            _GameState = GameState.gamePause;
            EventManager.Instance.Raise(new GamePauseEvent());
        }

        private void Resume() {
            if (IsPlaying) return;
            SetTimeScale(1);
            _GameState = GameState.gamePlay;
            EventManager.Instance.Raise(new GameResumeEvent());
        }

        private void Over() {
            SetTimeScale(0);
            _GameState = GameState.gameOver;
            EventManager.Instance.Raise(new GameOverEvent());
            //if(SfxManager.Instance) SfxManager.Instance.PlaySfx2D(Constants.GAMEOVER_SFX); => joue le SFX de mort
        }
        #endregion
    }
}

