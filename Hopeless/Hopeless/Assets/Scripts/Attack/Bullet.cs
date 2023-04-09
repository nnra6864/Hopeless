using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float FireRate;
    public int BounceAmount;
    public float BulletSpeed;
    public Vector3 Direction;
    public float Lifetime;
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
        //Attack the enemy etc.
        if (BounceAmount == 0) Destroy(gameObject);
        Direction *= -1;
        BounceAmount--;
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(Lifetime);
        Destroy(gameObject);
    }
}