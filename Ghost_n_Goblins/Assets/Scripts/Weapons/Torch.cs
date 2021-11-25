using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : Projectile
{
	[SerializeField]
	private float _RotateSpeed;
	[SerializeField]
	private float _ThrowSpeed;

	protected override void Move()
	{
		_Rigidbody2D.AddForce(transform.right * _Speed, ForceMode2D.Impulse);
	}

	// Start is called before the first frame update
	private void Start()
    {
		_Rigidbody2D.velocity = _ThrowSpeed / 2 * transform.up + _ThrowSpeed * transform.forward;
	}

    // Update is called once per frame
    private void Update()
    {
		transform.Rotate(new Vector3(0, 0, -_RotateSpeed * Time.deltaTime));
    }
}
