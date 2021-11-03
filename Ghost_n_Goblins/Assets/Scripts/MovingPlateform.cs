using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlateform : MonoBehaviour {
    [Tooltip("The number of cases that the plateform has to travel")]
    [SerializeField] int _DestinationCase;
    [Tooltip("Speed of the platerform in m/s")]
    [SerializeField] float _Speed;
    [Tooltip("Time the plateform wait at each end in seconds")]
    [SerializeField] float _WaitingTime;

    public float Speed { get { return _Speed; } }

    // X origin of the plateform
    float _xOrigin;

    // X final destination of the plateform
    float _xDestination;

    // Y origin of the plateform
    float _yPosition;

    // Going right or left (true or false)
    bool _Direction;

    // Start method, set up the variable and start the coroutine
    private void Start() {
        _xOrigin = transform.position.x;
        _xDestination = _xOrigin + _DestinationCase * 1.2f;
        _yPosition = transform.position.y;
        _Direction = true;
        StartCoroutine(MovingCoroutine());
    }

    // Coroutine moving the plateformindefinitely
    IEnumerator MovingCoroutine() {
        // Moving to the left
        while (true) {
            if (_Direction) {
                while (transform.position.x < _xDestination) {
                    yield return null;
                    Vector3 vect = new Vector3(_Speed * Time.deltaTime, 0, 0);
                    transform.position += vect;
                }
                transform.position = new Vector3(_xDestination, _yPosition, 0);
                yield return new WaitForSeconds(_WaitingTime);
                _Direction = false;
            }

            // Moving to the left
            if (!_Direction) {
                while (transform.position.x > _xOrigin) {
                    yield return null;
                    Vector3 vect = new Vector3(_Speed * Time.deltaTime, 0, 0);
                    transform.position -= vect;
                }
                transform.position = new Vector3(_xOrigin, _yPosition, 0);
                yield return new WaitForSeconds(_WaitingTime);
                _Direction = true;
            }
        }
    }
}
