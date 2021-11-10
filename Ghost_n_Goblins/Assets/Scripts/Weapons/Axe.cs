using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Projectile
{
	private float Vx;
	private float Vy;
	private float gravity = 10f;
	protected override void Move()
	{
		//_Rigidbody2D.AddForce(transform.right * _Speed, ForceMode2D.Impulse);
	}
	private void Start()
	{
		AxeThrowing();
	}
	private void AxeThrowing()
	{
		Vy = 4f;
		Vx = _Rigidbody2D.velocity.x * 2;
	}

	private void FixedUpdate()
	{
		_Rigidbody2D.velocity = new Vector2(Vx, Vy);
		Vx -= (gravity * Time.deltaTime) / 10;
		Vy -= (gravity * Time.deltaTime);

		transform.Rotate(Vector3.back * 15);
	}
}
