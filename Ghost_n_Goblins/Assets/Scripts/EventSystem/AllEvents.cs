namespace EventsManager {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using SDD.Events;

    #region GameManager Events
    public class GameMenuEvent : SDD.Events.Event {
    }

    public class GamePlayEvent : SDD.Events.Event {
    }

    public class GamePauseEvent : SDD.Events.Event {
    }

    public class GameResumeEvent : SDD.Events.Event {
    }

    public class GameOverEvent : SDD.Events.Event {
    }

    public class GameVictoryEvent : SDD.Events.Event {
    }
    public class GameInitializeLevelEvent : SDD.Events.Event {
        public string eSceneName;
    }

    public class LevelReadyEvent : SDD.Events.Event {
        public Transform ePlayerSpawnPoint;
        public GameObject eBackground;
        public Transform eMapBeginning;
        public Transform eMapEnding;
        public Transform eKey;
        public Transform ePlayer;
        public GameObject eBoss;
    }

    public class BossKilledEvent : SDD.Events.Event {
    }

    public class GameStatisticsChangedEvent : SDD.Events.Event {
        public float eBestScore { get; set; }

        public int eScore { get; set; }

        public int eNLives { get; set; }

        public float eTimer { get; set; }
    }

    public class SceneLoadedEvent : SDD.Events.Event {
        public Transform ePlayer;
    }

    public class WeaponSwapEvent : SDD.Events.Event {
        public Weapon eWeapon;
    }
    #endregion

    #region MenuManager Events
    public class EscapeButtonClickedEvent : SDD.Events.Event {
    }

    public class PlayButtonClickedEvent : SDD.Events.Event {
    }

    public class ResumeButtonClickedEvent : SDD.Events.Event {
    }

    public class MainMenuButtonClickedEvent : SDD.Events.Event {
    }

    public class QuitButtonClickedEvent : SDD.Events.Event { }
    #endregion

    #region Score Event
    public class ScoreItemEvent : SDD.Events.Event {
        public int eScore;
    }
    #endregion
}
