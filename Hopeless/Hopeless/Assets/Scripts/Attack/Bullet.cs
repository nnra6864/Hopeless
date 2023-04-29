using Assets.Scripts;
using Assets.Scripts.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet : MonoBehaviour
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
        UpdateColor();
        _previousPosition = transform.position;
    }

    void UpdateColor()
    {
        var col = NnUtils.HexToRgba(PlayerPrefs.GetString("BulletColor", "#FFFFFF"), new Color32(255, 255, 255, 255));
        GetComponent<Renderer>().material.color = col;
        GetComponentInChildren<Light2D>().color = col;
        var tr = GetComponentInChildren<TrailRenderer>();
        tr.startColor = col;
        tr.endColor = col;
        tr.material.color = col;
    }

    private void FixedUpdate()
    {
        _rb.velocity = Direction * BulletSpeed;
        var traveled = Vector3.Distance(_previousPosition, transform.position);
        _distanceTraveled += traveled;
        if (_distanceTraveled >= MaxDistance) Destroy(gameObject);
        if (_distanceTraveled > 0 && traveled == 0) Destroy(gameObject);
        _previousPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HitTarget(collision);
        Bounce(collision.contacts[0].normal);
        SFX.PlaySFX(gameObject, "BulletBounce", Prefs.Instance.SpatialAudio, true);
        if (MaxDistancePerBounce) _distanceTraveled = 0;
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