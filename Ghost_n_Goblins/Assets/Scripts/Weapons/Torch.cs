using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : Projectile
{
	[SerializeField]
	private float _RotateSpeed;
	protected override void Move()
	{
		
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
		transform.Rotate(new Vector3(_RotateSpeed * Time.deltaTime, 0, 0));
    }
}
