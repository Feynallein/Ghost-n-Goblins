using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObject : MonoBehaviour, IScore {
    [Tooltip("Score gained by the object")]
    [SerializeField] int _Score;
    public int Score { get { return _Score; } }
}
