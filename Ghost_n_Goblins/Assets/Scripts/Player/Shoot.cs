using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject weaponGO;
    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootWeapon();
        }
    }

    void ShootWeapon()
    {
        Instantiate(weaponGO, spawnPoint.position, spawnPoint.rotation);
    }
}
