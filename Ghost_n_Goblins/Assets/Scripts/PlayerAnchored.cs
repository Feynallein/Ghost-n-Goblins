using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnchored : MonoBehaviour {
    [SerializeField] Transform _Player;
    [SerializeField] float _yOffset;

    [SerializeField] Transform _MapBeginning;
    [SerializeField] Transform _MapEnding;

    float _HalfWidthSize;

    private void Start() {
        _HalfWidthSize = Camera.main.orthographicSize * Camera.main.aspect;
    }

    void Update() {
        if (transform.GetComponent<Light>() != null  || _Player.position.x - _HalfWidthSize > _MapBeginning.position.x && _Player.position.x + _HalfWidthSize < _MapEnding.position.x)
            transform.position = new Vector3(_Player.position.x, _Player.position.y + _yOffset, transform.position.z);
    }
}
