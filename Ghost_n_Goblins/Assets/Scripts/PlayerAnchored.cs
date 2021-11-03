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
        float ratio = Screen.width / Screen.height;
        float depth = transform.position.z - _Player.position.z;
        _HalfWidthSize = Mathf.Tan(0.5f * Camera.main.fieldOfView) * depth * ratio + 3.5f; //trouver un moyen de virer le 3.5!!
    }

    void Update() {
        if (transform.GetComponent<Light>() != null  || _Player.position.x - _HalfWidthSize > _MapBeginning.position.x && _Player.position.x + _HalfWidthSize < _MapEnding.position.x)
            transform.position = new Vector3(_Player.position.x, _Player.position.y + _yOffset, transform.position.z);
    }
}
