using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Projectile
{
    protected override void Move()
	{
		_Rigidbody2D.AddForce(transform.right * _Speed, ForceMode2D.Impulse);
	}

	private void Update()
	{
		DestroyProjectile(0.3f);
	}

	// TO DO
	// Method to block enemies bullets
}
