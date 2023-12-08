using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;

    [SerializeField] private ParticleSystem _particleSystem;

    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector3 direction)
    {
        _rb.velocity = direction.normalized * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Healthable healthable))
        {
            healthable.Damage(_damage);
            _particleSystem.transform.SetParent(null);
            _particleSystem.Play();
        }

        Destroy(gameObject);
    }
}
