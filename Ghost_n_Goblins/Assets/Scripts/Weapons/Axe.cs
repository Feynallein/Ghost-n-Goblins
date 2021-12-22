using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Projectile
{
	[SerializeField]
	private float _ThrowSpeed = 10f;
	[SerializeField]
	private float _RotateSpeed = 10f;	

	protected override void Move()
	{
		_Rigidbody2D.AddForce(transform.right * _Speed, ForceMode2D.Impulse);
	}

	private void Start()
	{
		_Rigidbody2D.AddRelativeForce(transform.right * _ThrowSpeed, ForceMode2D.Impulse);
	}

	private void Update()
	{
		transform.Rotate(new Vector3(0, 0, -_RotateSpeed * Time.deltaTime));
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
			Destroy(gameObject);
	}
}
