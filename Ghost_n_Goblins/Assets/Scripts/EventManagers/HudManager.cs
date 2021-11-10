namespace GhostsnGoblins {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using SDD.Events;

    public class HudManager : Manager<HudManager> {
        #region Variables
        [Tooltip("HUD GameObject (global parent)")]
        [SerializeField] GameObject HUD;

        [Tooltip("Score textfield")]
        [SerializeField] Text _Score;

        [Tooltip("Timer textfield")]
        [SerializeField] Text _Timer;

        [Tooltip("Life UI Image prefab")]
        [SerializeField] GameObject LifePrefab;

        // List of gameobject who represents lives in the HUD
        List<GameObject> Lives = new List<GameObject>();
        #endregion

        #region Manager implementation
        protected override void Awake() {
            base.Awake();
            SetHudActive(false);
        }

        protected override IEnumerator InitCoroutine() {
            yield break; // nothing
        }
        #endregion

        #region HudManager Methods
        void SetScoreAndTimerUI(int score, float timer) {
            // Set the score and the timer
            _Score.text = score.ToString();
            _Timer.text = timer.ToString("N01");
        }

        void SetHudActive(bool active) {
            // Disable or Enable the HUD component
            HUD.SetActive(active);
        }

        void SetLivesUI(int NLives) { //bug -> pas le bon scaling
            // Change the number of lives displayed on the UI
            int actualLives = Lives.Count;
            float imageWidth = LifePrefab.transform.GetComponent<Image>().preferredWidth * LifePrefab.transform.localScale.x;
            if (NLives == actualLives) return;

            else if(NLives > actualLives) {
                for (int i = Lives.Count; i < NLives; i++) {
                    GameObject live = Instantiate(LifePrefab) as GameObject;
                    live.transform.position = new Vector3(live.transform.position.x + i * imageWidth, live.transform.position.y, live.transform.position.y);
                    live.transform.SetParent(HUD.transform);
                    Lives.Add(live);
                }
            }

            else if(NLives < actualLives) {
                for(int i = actualLives; i > NLives - actualLives; i--) {
                    Lives.RemoveAt(i);
                }
            }
        }
        #endregion

        #region Event's Callbacks
        protected override void GamePlay(GamePlayEvent e) {
            SetHudActive(true);
        }

        protected override void GameStatisticsChanged(GameStatisticsChangedEvent e) {
            SetScoreAndTimerUI(e.eScore, e.eTimer);
            SetLivesUI(e.eNLives);
        }
        #endregion
    }
}