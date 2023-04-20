using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.UIElements;

public class Obstacle : MonoBehaviour
{
    [SerializeField] int _damage;
    [SerializeField] float _damageRate;
    private Dictionary<GameObject, Coroutine> _targets = new();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<IHittable>(out var hittable)) return;
        _targets.TryAdd(collision.gameObject, StartCoroutine(DamageTarget(collision.gameObject, hittable)));
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!_targets.TryGetValue(collision.gameObject, out var damageRoutine)) return;
        StopCoroutine(damageRoutine);
        _targets.Remove(collision.gameObject);
    }

    void Update()
    {
        if (_targets.Count < 1) return;
        List<GameObject> remove = new();
        foreach (var t in _targets)
        {
            if (t.Key.GetComponent<Collider2D>().enabled) continue;
            StopCoroutine(t.Value);
            remove.Add(t.Key);
        }
        if (remove.Count < 1) return;
        foreach (var r in remove)
            _targets.Remove(r);
    }

    IEnumerator DamageTarget(GameObject targetObject, IHittable target)
    {
        while (true)
        {
            if (target == null)
            {
                _targets.Remove(targetObject);
                yield break;
            }
            target.GetHit(_damage);
            yield return new WaitForSeconds(_damageRate);
        }
    }
}
