using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterProjectile : MonoBehaviour {
    Enemy _Parent;
    float _ProjectileSpeed;

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.transform.GetComponent<Player>();
        if (player != null) player.TakeDamage();
        if (player != null || Layers.Instance.IsDestroyedByTerrain(collision)) IsDestroyed();
    }

    public void Shoot() {
        GetComponent<Rigidbody2D>().AddForce(transform.forward * _ProjectileSpeed, ForceMode2D.Impulse);
    }

    void IsDestroyed() {
        _Parent.ProjectileDestroyed(gameObject);
        Destroy(gameObject);
    }

    public Enemy Enemy { set { _Parent = value; } }
    public float Speed { set { _ProjectileSpeed = value; } }
}