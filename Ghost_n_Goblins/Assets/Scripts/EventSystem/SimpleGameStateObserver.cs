﻿namespace GhostsnGoblins {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using SDD.Events;

    public abstract class SimpleGameStateObserver : MonoBehaviour, IEventHandler {
        public virtual void SubscribeEvents() {
            EventManager.Instance.AddListener<GameMenuEvent>(GameMenu);
            EventManager.Instance.AddListener<GamePlayEvent>(GamePlay);
            EventManager.Instance.AddListener<GamePauseEvent>(GamePause);
            EventManager.Instance.AddListener<GameResumeEvent>(GameResume);
            EventManager.Instance.AddListener<GameOverEvent>(GameOver);
            EventManager.Instance.AddListener<GameVictoryEvent>(GameVictory);
            EventManager.Instance.AddListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
            EventManager.Instance.AddListener<GameInitializeLevelEvent>(GameInitializeLevel);
            EventManager.Instance.AddListener<LevelReadyEvent>(LevelReady);
            EventManager.Instance.AddListener<BossKilledEvent>(BossKilled);
        }

        public virtual void UnsubscribeEvents() {
            EventManager.Instance.RemoveListener<GameMenuEvent>(GameMenu);
            EventManager.Instance.RemoveListener<GamePlayEvent>(GamePlay);
            EventManager.Instance.RemoveListener<GamePauseEvent>(GamePause);
            EventManager.Instance.RemoveListener<GameResumeEvent>(GameResume);
            EventManager.Instance.RemoveListener<GameOverEvent>(GameOver);
            EventManager.Instance.RemoveListener<GameVictoryEvent>(GameVictory);
            EventManager.Instance.RemoveListener<GameInitializeLevelEvent>(GameInitializeLevel);
            EventManager.Instance.RemoveListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
            EventManager.Instance.RemoveListener<LevelReadyEvent>(LevelReady);
            EventManager.Instance.RemoveListener<BossKilledEvent>(BossKilled);
        }

        protected virtual void Awake() {
            SubscribeEvents();
        }

        protected virtual void OnDestroy() {
            UnsubscribeEvents();
        }

        protected virtual void GameMenu(GameMenuEvent e) {
        }

        protected virtual void GamePlay(GamePlayEvent e) {
        }

        protected virtual void GamePause(GamePauseEvent e) {
        }

        protected virtual void GameResume(GameResumeEvent e) {
        }

        protected virtual void GameOver(GameOverEvent e) {
        }

        protected virtual void GameVictory(GameVictoryEvent e) {
        }

        protected virtual void GameStatisticsChanged(GameStatisticsChangedEvent e) {
        }

        protected virtual void GameInitializeLevel(GameInitializeLevelEvent e) {
        }

        protected virtual void LevelReady(LevelReadyEvent e) {
        }

        protected virtual void BossKilled(BossKilledEvent e) {
        }
    }
}
