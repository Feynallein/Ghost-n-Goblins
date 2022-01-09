using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Ashes : MonoBehaviour
{
    [SerializeField] private int _Damage;
    [SerializeField] private float _TimerToDestroy = 2.0f;
    [SerializeField] private AudioSource _Sound;

    private void Awake()
    {
        _Sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(_Damage);
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_Sound.clip)
            _Sound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, _TimerToDestroy);
    }
}
