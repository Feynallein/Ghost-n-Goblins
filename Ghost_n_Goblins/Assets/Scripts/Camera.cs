using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
    [SerializeField] GameObject _Player;
    [SerializeField] float _yOffset;

    void Update() {
        transform.position = new Vector3(_Player.transform.position.x, _Player.transform.position.y, transform.position.z);
    }
}
