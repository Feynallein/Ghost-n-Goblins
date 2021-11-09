namespace GhostsnGoblins {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public class Shoot : MonoBehaviour {
        [Serializable]
        struct WeaponTable {
            [SerializeField] Weapon weapon;
            public GameObject weaponPrefab;
        }

        [SerializeField] Transform spawnPoint;
        [SerializeField] WeaponTable[] weaponTable;
        Weapon _CurrentWeapon;
        
        public Weapon CurrentWeapon { set { _CurrentWeapon = value; } }
        public Weapon GetCurrentWeapon { get; } //TEMPOPRARY

        void Update() {
            Debug.Log(_CurrentWeapon + " " + (int) _CurrentWeapon);
            if (Input.GetButtonDown("Fire1")) {
                ShootWeapon();
            }
        }

        void ShootWeapon() {
            Instantiate(weaponTable[(int) _CurrentWeapon].weaponPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
