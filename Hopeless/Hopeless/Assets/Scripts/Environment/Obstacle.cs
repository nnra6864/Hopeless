using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float damageRate;
    private Dictionary<GameObject, Coroutine> targets = new();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<IHittable>(out var hittable)) return;
        targets.TryAdd(collision.gameObject, StartCoroutine(DamageTarget(collision.gameObject, hittable)));
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!targets.TryGetValue(collision.gameObject, out var damageRoutine)) return;
        StopCoroutine(damageRoutine);
        targets.Remove(collision.gameObject);
    }

    IEnumerator DamageTarget(GameObject targetObject, IHittable target)
    {
        while (true)
        {
            if (target == null)
            {
                targets.Remove(targetObject);
                yield break;
            }
            target.GetHit(damage);
            yield return new WaitForSeconds(damageRate);
        }
    }
}
