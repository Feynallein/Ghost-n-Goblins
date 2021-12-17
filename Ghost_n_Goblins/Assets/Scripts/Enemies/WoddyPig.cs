using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoddyPig : Enemy {
    [Header("Monster specifications")]
    [Tooltip("Monter's projectile prefab")]
    [SerializeField] GameObject _Spear;
    [SerializeField] float _SpearSpeed;
    [SerializeField] int _SpearsOnScreen;
    [SerializeField] float _Speed;

    protected override void Attack() {
        //todo: front attack / below attack if player below
    }

    protected override void Move() {
    }

    protected override void PlayerDetected() {
        GoForward(_Speed);
        //todo: move toward player, if player is on the other direction : drop from 1 height & turn around
    }
}
