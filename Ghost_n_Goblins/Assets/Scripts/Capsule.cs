using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    [SerializeField]
    float playerSpeed = 10f;

    Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal") * playerSpeed;

        movement = new Vector3(xInput, 0, 0);
        movement *= Time.deltaTime;

        transform.Translate(movement);
    }
}
