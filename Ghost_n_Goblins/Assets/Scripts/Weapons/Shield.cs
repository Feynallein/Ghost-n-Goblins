using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Projectile
{
    protected override void Move()
	{
		_Rigidbody2D.AddForce(transform.right * _Speed, ForceMode2D.Impulse);
	}

	private void Start()
	{
		Move();
	}

	private void Update()
	{
		Destroy(gameObject, 0.3f);
	}

	// TO DO
	// Method to block enemies bullets
	private void OnCollisionEnter2D(Collision2D collision)
	{
		Enemy enemy = collision.gameObject.GetComponent<Enemy>();
		if (enemy != null)
		{
			if (_Sound.clip)
				_Sound.Play();

			enemy.TakeDamage(_Damage);
			Destroy(gameObject);
		}

		if (collision.gameObject.GetComponent<MonsterProjectile>())
		{
			if (_Sound.clip)
				_Sound.Play();

			Destroy(collision.gameObject);
			Destroy(gameObject);
		}
	}
}
