using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layers : MonoBehaviour {
    [SerializeField] public LayerMask GroundLayerMask;
    [SerializeField] public LayerMask LadderLayerMask;
    [SerializeField] public LayerMask MovingPlateformLayerMask;
    [SerializeField] public LayerMask PlayerLayerMask;

    static Layers _Instance;

    public static Layers Instance { get { return _Instance; } }

    private void Awake() {
        if (_Instance != null) Destroy(gameObject);
        else _Instance = this;
    }
}
