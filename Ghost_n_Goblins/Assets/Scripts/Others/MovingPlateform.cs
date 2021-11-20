using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlateform : MonoBehaviour {
    [Tooltip("The number of cases that the plateform has to travel")]
    [SerializeField] int _DestinationCase;
    [Tooltip("Speed of the platerform in m/s")]
    [SerializeField] float _Speed;
    [Tooltip("How many seconds the plateform waits (in seconds)")]
    [SerializeField] float _WaitDuration;

    // X origin of the plateform
    float _xOrigin;

    // X final destination of the plateform
    float _xDestination;

    // Y origin of the plateform
    float _yPosition;

    // Start method, set up the variable and start the coroutine
    private void Start() {
        _xOrigin = transform.position.x;
        _xDestination = _xOrigin + _DestinationCase * 1.2f;
        _yPosition = transform.position.y;
        StartCoroutine(MoveLeftRightCoroutine(true));
    }

    IEnumerator MoveLeftRightCoroutine(bool isFacingRight) {
        int boolMultiplier = isFacingRight ? 1 : -1;

        while((transform.position.x < _xDestination && isFacingRight) || (transform.position.x > _xOrigin && !isFacingRight)) {
            Vector3 vect = new Vector3(_Speed * boolMultiplier * Time.deltaTime, 0, 0);
            transform.position += vect;
            yield return null;
        }

        if(isFacingRight) transform.position = new Vector3(_xDestination, _yPosition, 0);
        else transform.position = new Vector3(_xOrigin, _yPosition, 0);

        yield return new WaitForSeconds(_WaitDuration);
        StartCoroutine(MoveLeftRightCoroutine(!isFacingRight));
    }
}
