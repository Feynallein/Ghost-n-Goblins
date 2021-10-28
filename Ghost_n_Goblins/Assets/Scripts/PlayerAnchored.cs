using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnchored : MonoBehaviour {
    [SerializeField] GameObject _Player;
    [SerializeField] float _yOffset;

    void Update() {
        transform.position = new Vector3(_Player.transform.position.x, _Player.transform.position.y + _yOffset, transform.position.z);
    }
}
