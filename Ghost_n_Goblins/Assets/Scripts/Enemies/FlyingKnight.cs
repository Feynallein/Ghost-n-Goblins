using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LinearInterpolation3D;

public class FlyingKnight : Enemy {
    [SerializeField] float _Speed;

    float _Y;

    CurveLinearInterpo interpo;

    void Start() {
        interpo = new CurveLinearInterpo(ParametricEquations.NegSin3, 0, 2 * Mathf.PI, 15);
        _Y = transform.position.y;
    }

    IEnumerator MovementCoroutine() {
        float distance = 0;
        Vector3 origin = transform.position;
        origin.y = _Y;
        while(distance < interpo.TotalLength) {
            transform.position = origin + interpo.GetPositionFromDistance(distance).toVector3();
            distance += _Speed;
            yield return null;
        }
        transform.position = origin + interpo.GetPositionFromDistance(interpo.TotalLength).toVector3();
        StartCoroutine(MovementCoroutine());
    }

    protected override void Attack() {
    }

    protected override void Move() {
    }

    protected override void PlayerDetected() {
        StartCoroutine(MovementCoroutine());
    }
}
