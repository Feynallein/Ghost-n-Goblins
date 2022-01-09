using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : Projectile
{
	[SerializeField]
	private float _RotateSpeed;
	[SerializeField]
	private float _ThrowSpeed;

	public GameObject _AshesPrefab;

	protected override void Move()
	{
		_Rigidbody2D.AddForce(transform.right * _Speed, ForceMode2D.Impulse);
	}

	// Start is called before the first frame update
	private void Start()
    {
		_Rigidbody2D.AddRelativeForce(transform.right * _ThrowSpeed, ForceMode2D.Impulse);
	}

    // Update is called once per frame
    private void Update()
    {
		transform.Rotate(new Vector3(0, 0, -_RotateSpeed * Time.deltaTime));
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Enemy enemy = collision.gameObject.GetComponent<Enemy>();
		if (enemy != null)
		{
			enemy.TakeDamage(_Damage);
			Destroy(gameObject);
			
		}

		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			if (_AshesPrefab)
				Instantiate(_AshesPrefab, gameObject.transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
		
	}
}
