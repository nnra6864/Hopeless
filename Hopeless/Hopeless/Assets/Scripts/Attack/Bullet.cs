using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int BounceAmount, DamageAmount;
    [HideInInspector] public float BulletSpeed;
    [HideInInspector] public float Lifetime;
    [HideInInspector] public Vector3 Direction;
    [SerializeField] Rigidbody2D _rb;

    private void Start()
    {
        StartCoroutine(DeathTimer());
    }

    private void FixedUpdate()
    {
        _rb.velocity = Direction * BulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HitTarget(collision);
        Bounce(collision.contacts[0].normal);
    }

    private void HitTarget(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<IHittable>(out var hittable)) return;
        hittable.GetHit(DamageAmount);
    }

    private void Bounce(Vector3 normal)
    {
        if (BounceAmount == 0) Destroy(gameObject);
        Direction = Vector3.Reflect(Direction, normal);
        BounceAmount--;
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(Lifetime);
        Destroy(gameObject);
    }
}