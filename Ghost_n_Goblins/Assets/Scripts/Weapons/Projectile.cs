using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Projectile : MonoBehaviour {
    [SerializeField] int _Damage;
    [SerializeField] protected float _Speed;

    protected Rigidbody2D _Rigidbody2D;

    protected virtual void Awake() 
	{
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 2f); //temporary
    }

    private void Start() 
	{
        Move();
    }

    protected abstract void Move();
    private void OnCollisionEnter2D(Collision2D collision) 
	{
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
		if (enemy != null)
		{ 
			enemy.TakeDamage(_Damage);
			Destroy(gameObject);
		}
    }
}