namespace EventsManager {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using SDD.Events;

    public class HudManager : Manager<HudManager> {
        #region Variables
        [Tooltip("HUD GameObject (global parent)")]
        [SerializeField] GameObject _HUD;

        [Tooltip("Score textfield")]
        [SerializeField] Text _Score;

        [Tooltip("Timer textfield")]
        [SerializeField] Text _Timer;

        [Tooltip("Life UI Image prefab")]
        [SerializeField] GameObject _LifePrefab;

        [Tooltip("Lance UI Image")]
        [SerializeField] GameObject _LanceUI;

        [Tooltip("Dagger UI Image")]
        [SerializeField] GameObject _DaggerUI;

        [Tooltip("Torch UI Image")]
        [SerializeField] GameObject _TorchUI;

        [Tooltip("Axe UI Image")]
        [SerializeField] GameObject _AxeUI;

        [Tooltip("Shield UI Image")]
        [SerializeField] GameObject _ShieldUI;

        // List of gameobject who represents lives in the HUD
        List<GameObject> _Lives = new List<GameObject>();
        List<GameObject> _WeaponUiImages = new List<GameObject>();
        #endregion

        #region Manager implementation
        protected override void Awake() {
            base.Awake();
            SetHudActive(false);
            RegisterWeapons();
        }

        protected override IEnumerator InitCoroutine() {
            yield break; // nothing
        }
        #endregion

        public override void SubscribeEvents() {
            base.SubscribeEvents();
            EventManager.Instance.AddListener<WeaponSwapEvent>(WeaponSwap);
        }

        public override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            EventManager.Instance.RemoveListener<WeaponSwapEvent>(WeaponSwap);
        }

        #region HudManager Methods
        void ActivateWeapon(GameObject weapon) {
            // activate the specific weapon
            foreach (var item in _WeaponUiImages)
                if (item) item.SetActive(item == weapon);
        }

        void RegisterWeapons() {
            _WeaponUiImages.Add(_LanceUI);
            _WeaponUiImages.Add(_DaggerUI);
            _WeaponUiImages.Add(_TorchUI);
            _WeaponUiImages.Add(_AxeUI);
            _WeaponUiImages.Add(_ShieldUI);
        }
        void ChangeUiWeapon(Weapon weapon) {
            switch (weapon) {
                case Weapon.Lance:
                    ActivateWeapon(_LanceUI);
                    break;
                case Weapon.Dagger:
                    ActivateWeapon(_DaggerUI);
                    break;
                case Weapon.Torch:
                    ActivateWeapon(_TorchUI);
                    break;
                case Weapon.Axe:
                    ActivateWeapon(_AxeUI);
                    break;
                case Weapon.Shield:
                    ActivateWeapon(_ShieldUI);
                    break;
                default:
                    break;
            }
        }

        void SetScoreAndTimerUI(int score, float timer) {
            // Set the score and the timer
            _Score.text = score.ToString();
            _Timer.text = timer.ToString("N01");
        }

        void SetHudActive(bool active) {
            // Disable or Enable the HUD component
            _HUD.SetActive(active);
        }

        void SetLivesUI(int NLives) { //bug -> pas le bon scaling
            // Change the number of lives displayed on the UI
            int actualLives = _Lives.Count;
            float imageWidth = _LifePrefab.transform.GetComponent<Image>().preferredWidth * _LifePrefab.transform.localScale.x;
            if (NLives == actualLives) return;

            else if(NLives > actualLives) {
                for (int i = _Lives.Count; i < NLives; i++) {
                    GameObject live = Instantiate(_LifePrefab) as GameObject;
                    live.transform.position = new Vector3(live.transform.position.x + i * imageWidth, live.transform.position.y, live.transform.position.y);
                    live.transform.SetParent(_HUD.transform);
                    _Lives.Add(live);
                }
            }

            else if(NLives < actualLives) {
                for(int i = actualLives; i > NLives - actualLives; i--) {
                    _Lives.RemoveAt(i);
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

         void WeaponSwap(WeaponSwapEvent e) {
            ChangeUiWeapon(e.eWeapon);
         }
        #endregion
    }
}