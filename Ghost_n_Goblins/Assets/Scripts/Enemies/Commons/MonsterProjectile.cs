using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostsnGoblins;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterProjectile : MonoBehaviour {
    Enemy _Parent;
    float _ProjectileSpeed;
    [SerializeField] LayerMask _DestroyProjectile;

    private void Start() {
        GetComponent<Rigidbody2D>().AddForce(transform.forward * _ProjectileSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.transform.GetComponent<Player>();
        if (player != null) player.TakeDamage();
        if (player != null || (_DestroyProjectile.value & (1 << collision.gameObject.layer)) > 0) IsDestroyed();
    }

    void IsDestroyed() {
        _Parent.ProjectileDestroyed(gameObject);
        Destroy(gameObject);
    }

    public Enemy Enemy { set { _Parent = value; } }
    public float Speed { set { _ProjectileSpeed = value; } }
}

