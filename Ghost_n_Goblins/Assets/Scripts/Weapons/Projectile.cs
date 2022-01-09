using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public abstract class Projectile : MonoBehaviour {
    [SerializeField] protected int _Damage;
    [SerializeField] protected float _Speed;
    [SerializeField] protected AudioSource _Sound;

    protected Rigidbody2D _Rigidbody2D;

    protected virtual void Awake() 
	{
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        _Sound = GetComponent<AudioSource>();
        Destroy(gameObject, 2f); //temporary
    }

    private void Start() 
	{
        Move();
        if (_Sound.clip)
            _Sound.Play();
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

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            Destroy(gameObject);
    }
}