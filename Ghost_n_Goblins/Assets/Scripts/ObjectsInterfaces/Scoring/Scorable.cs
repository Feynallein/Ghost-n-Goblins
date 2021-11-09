using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorable : MonoBehaviour, IScore {
    [SerializeField] int _Score;
    public int Score { get { return _Score; } }
}
