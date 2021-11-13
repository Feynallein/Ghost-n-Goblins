using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateform : MonoBehaviour {
    Collider2D _Collider;
    [SerializeField] LayerMask _PlayerLayerMask;

    private void Start() {
        _Collider = GetComponent<Collider2D>();
    }

    void IsTrigger(bool b) {
        _Collider.isTrigger = b;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //if ((_PlayerLayerMask & (1 << collision.gameObject.layer)) > 0) IsTrigger(false);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        //if ((_PlayerLayerMask & (1 << collision.gameObject.layer)) > 0) IsTrigger(true);
    }
}
