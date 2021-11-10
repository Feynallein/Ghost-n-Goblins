using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnSelf : MonoBehaviour {
    [SerializeField] float _RotationSpeed;
    Transform _RotatingObject;

    private void Start() {
        _RotatingObject = transform.GetChild(0).transform;
        StartCoroutine(RotateOnSelfCoroutine());
    }

    IEnumerator RotateOnSelfCoroutine() {
        // Rotation on self
        while (true) {
            _RotatingObject.RotateAround(_RotatingObject.position, transform.up, Time.deltaTime * _RotationSpeed);
            yield return null;
        }
    }
}
