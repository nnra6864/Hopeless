using Assets.Scripts;
using Assets.Scripts.Core;
using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [HideInInspector] public int BounceAmount, DamageAmount;
    [HideInInspector] public float BulletSpeed;
    [HideInInspector] public float MaxDistance;
    [HideInInspector] public bool MaxDistancePerBounce;
    private float _distanceTraveled;
    private Vector3 _previousPosition;
    [HideInInspector] public Vector3 Direction;
    [SerializeField] Rigidbody2D _rb;

    private void Start()
    {
        _previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        _rb.velocity = Direction * BulletSpeed;
        _distanceTraveled += Vector3.Distance(_previousPosition, transform.position);
        if (_distanceTraveled >= MaxDistance) Destroy(gameObject);
        _previousPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HitTarget(collision);
        Bounce(collision.contacts[0].normal);
        SFX.PlaySFX(gameObject, "BulletBounce", Prefs.Instance.SpatialAudio, true);
    }

    private void HitTarget(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<IHittable>(out var hittable)) return;
        hittable.GetHit(DamageAmount);
    }

    private void Bounce(Vector3 normal)
    {
        if (BounceAmount == 0) Destroy(gameObject);
        Direction = Vector2.Reflect(Direction, normal);
        BounceAmount--;
    }
}
