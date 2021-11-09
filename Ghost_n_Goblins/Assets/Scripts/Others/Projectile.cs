using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {
    [SerializeField]
    float m_speed = 10f;

    Rigidbody2D rb2D;

    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start() {

        rb2D.AddForce(transform.right * m_speed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update() {
        Destroy(gameObject, 3f);
    }
}
