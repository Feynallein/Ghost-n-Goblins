using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Projectile
{
	private float speed = 10f;
	public GameObject _TargetPoint;

	private float axePositionX;
	private float targetPositionX;

	private float dist;
	private float nextX;
	private float baseY;
	private float height;

	protected override void Move()
	{
		//_Rigidbody2D.AddForce(transform.right * _Speed, ForceMode2D.Impulse);
	}

	private void Awake()
	{
		_TargetPoint = GameObject.Find("TargetPoint");
	}

	private void Start()
	{
		
	}

	private void Update()
	{
		ThrowingAxe();
	}
	private void ThrowingAxe()
	{
		axePositionX = transform.position.x;
		targetPositionX = _TargetPoint.transform.position.x;

		dist = targetPositionX - axePositionX;
		nextX = Mathf.MoveTowards(transform.position.x, targetPositionX, speed * Time.deltaTime);
		baseY = Mathf.Lerp(transform.position.y, _TargetPoint.transform.position.y, (nextX - axePositionX) / dist);
		height = 2 * (nextX - axePositionX) * (nextX - targetPositionX) / (-0.25f * dist * dist);

		Vector3 movePosition = new Vector3(nextX, baseY + height, transform.position.z);

		transform.position = movePosition;
	}

	private void FixedUpdate()
	{
		//_Rigidbody2D.velocity = new Vector2(Vx, Vy);
		//Vx -= (gravity * Time.deltaTime) / 10;
		//Vy -= (gravity * Time.deltaTime);

		//transform.Rotate(Vector3.back * 15);
	}

	
}
